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

        public async Task<Cuenta> crearCuenta(int idCliente, decimal saldoInicial, string tipoCuenta)
        {

            var cliente = await _clienteServives.consultarClienteById(idCliente);

            if (cliente == null)
            {
                throw new ArgumentException("El cliente ingresado no existe");
            }

            if (saldoInicial < 1000)
            {
                throw new ArgumentException("El saldo de apertura de la cuenta debe ser mayor a 1000.00");
            }

            if (string.IsNullOrEmpty(tipoCuenta))
            {
                throw new ArgumentException("El tipo de cuenta es requerido");
            }

            ContadorCuentas contadorCuentas = await _contadorCuentasService.obtenerContador(tipoCuenta);

            var cuenta = new Cuenta
            {
                idCliente = cliente.id,
                saldo = saldoInicial,
                fechaCreacion = DateTime.Now,
                estado = true,
                numeroCuenta = contadorCuentas.correlativo + contadorCuentas.contador.ToString(),
                tipoCuenta = tipoCuenta

            };

            _context.Cuentas.Add(cuenta);
            await _context.SaveChangesAsync();

            return cuenta;
        }

        public async Task<Cuenta> consultarSaldo(string numeroCuenta, string identificacion)
        {

            if (string.IsNullOrEmpty(numeroCuenta))
            {
                throw new ArgumentException("El numero de cuenta es requerido");
            }

            if (string.IsNullOrEmpty(numeroCuenta))
            {
                throw new ArgumentException("La identificacion del cliente es requerido");
            }

            var cuenta = await _context.Cuentas.FirstOrDefaultAsync(c => c.numeroCuenta == numeroCuenta);

            if (cuenta == null)
            {
                throw new ArgumentException("Cuenta no encontrada");
            }

            var cliente = await _clienteServives.consultarClienteById(cuenta.idCliente);

            if (cliente.identificacion != identificacion)
            {
                throw new ArgumentException("La cuenta no pertenece al cliente con identificacion " + identificacion);
            }

            return cuenta;
        }

        public async Task<Cuenta> depositoCuenta(string numeroCuenta, decimal monto)
        {
            if (string.IsNullOrEmpty(numeroCuenta))
            {
                throw new ArgumentException("El numero de cuenta es requerido");
            }

            var cuenta = await _context.Cuentas.FirstOrDefaultAsync(c => c.numeroCuenta == numeroCuenta);

            if (cuenta == null)
            {
                throw new ArgumentException("Cuenta no encontrada");
            }

            if(!cuenta.estado){
                throw new ArgumentException("La Cuenta se encuentra inactiva");
            }

            cuenta.saldo = cuenta.saldo + monto;
            await _context.SaveChangesAsync();

            return cuenta;
        }

        public async Task<Cuenta> retiroCuenta(string numeroCuenta, decimal monto)
        {
            if (string.IsNullOrEmpty(numeroCuenta))
            {
                throw new ArgumentException("El numero de cuenta es requerido");
            }

            var cuenta = await _context.Cuentas.FirstOrDefaultAsync(c => c.numeroCuenta == numeroCuenta);

            if (cuenta == null)
            {
                throw new ArgumentException("Cuenta no encontrada");
            }

            if(!cuenta.estado){
                throw new ArgumentException("La Cuenta se encuentra inactiva");
            }

            if(cuenta.saldo < monto){
                throw new ArgumentException("La cuenta del cliente no cuenta con el saldo suficiente");
            }

            cuenta.saldo = cuenta.saldo - monto;
            await _context.SaveChangesAsync();

            return cuenta;
        }

        




    }
}






