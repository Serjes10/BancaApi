using BancaApi.Data;
using BancaApi.Domain.Entities;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BancaApi.Domain.Interface;

namespace BancaApi.Services
{
    public class CuentaService : ICuentaServices
    {
        private readonly BancaDbContext _context;

        private readonly IClienteServices _clienteServives;

        private readonly IContadorCuentas _contadorCuentasService;


        public CuentaService(BancaDbContext context, IClienteServices clienteServices, IContadorCuentas contadorCuentas)
        {
            _context = context;
            _clienteServives = clienteServices;
            _contadorCuentasService = contadorCuentas;
        }

        public async Task<Cuenta> CrearCuentaAsync(int idCliente, decimal saldoInicial, string tipoCuenta)
        {

            var cliente = await _clienteServives.consultarClienteById(idCliente);

            if (saldoInicial < 1000)
            {
                throw new Exception("El saldo de apertura de la cuenta debe ser mayor a 1000.00");
            }

            ContadorCuentas contadorCuentas = await _contadorCuentasService.obtenerContador(tipoCuenta);
            var cuenta = new Cuenta
            {
                id = cliente.id,
                saldo = saldoInicial,
                fechaCreacion = DateTime.Now,
                estado = true,
                numeroCuenta = contadorCuentas.correlativo + contadorCuentas.contador.ToString()

            };

            _context.Cuentas.Add(cuenta);
            await _context.SaveChangesAsync();

            return cuenta;
        }

        public async Task<Cuenta> consultarSaldoAsync(string numeroCuenta)
        {
            // Buscar la cuenta en la base de datos por su número de cuenta
            var cuenta = await _context.Cuentas.FirstOrDefaultAsync(c => c.numeroCuenta == numeroCuenta);
            if (cuenta == null)
            {
                throw new Exception("Cuenta no encontrada");
            }

            return cuenta;  // Devolver la cuenta encontrada
        }

        // Registrar una transacción (depósito o retiro)
        public async Task RegistrarTransaccionAsync(string numeroCuenta, string tipoTransaccion, decimal monto)
        {
            var cuenta = await _context.Cuentas
                                        .FirstOrDefaultAsync(c => c.numeroCuenta == numeroCuenta);

            if (cuenta == null)
            {
                throw new Exception("Cuenta no encontrada");
            }

            // Lógica para manejar el tipo de transacción (por ejemplo, depósito o retiro)
            if (tipoTransaccion == "DEP")
            {
                cuenta.saldo += monto;  // Aumentar el saldo por un depósito
            }
            else if (tipoTransaccion == "RET")
            {
                if (cuenta.saldo < monto)
                {
                    throw new Exception("Saldo insuficiente para retiro");
                }
                cuenta.saldo -= monto;  // Disminuir el saldo por un retiro
            }
            else
            {
                throw new Exception("Tipo de transacción no válido");
            }

            // Registrar la transacción en la base de datos (esto puede involucrar otra entidad de transacciones)
            var transaccion = new Transaccion
            {
                cuenta = cuenta,
                fecha = DateTime.Now,
                idCuenta = cuenta.id,
                tipoTransaccion = tipoTransaccion,
                monto = monto

            };

            _context.Transacciones.Add(transaccion);
            await _context.SaveChangesAsync();  // Guardar los cambios

            // Devolver el estado actualizado de la cuenta
            await _context.SaveChangesAsync();
        }


    }
}






