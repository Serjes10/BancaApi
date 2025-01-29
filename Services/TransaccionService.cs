
using BancaApi.Data;
using BancaApi.Domain;
using BancaApi.Domain.Entities;
using BancaApi.Domain.Interface;
using BancaApi.Services;
using Microsoft.EntityFrameworkCore;

public class TransaccionService : ITransaccionService
{
    private readonly BancaDbContext _context;
    private readonly ICuentaServices _cuentaService;

    public Transaccion transaccion;


    public TransaccionService(BancaDbContext context, ICuentaServices cuentaService)
    {
        _context = context;
        _cuentaService = cuentaService;
    }


    public async Task<Transaccion> generarTransaccion(string tipoTransaccion, decimal monto, string numeroCuenta, string identificacion)
    {

        if (string.IsNullOrEmpty(tipoTransaccion))
        {
            throw new ArgumentException("El tipo de transaccion es requerido");
        }

        if (monto <= 0)
        {
            throw new ArgumentException("El monto no puede ser menor a igual a 0");
        }

        var cuenta = await _cuentaService.consultarSaldo(numeroCuenta, identificacion);

        if (tipoTransaccion.ToUpper() == "DEPOSITO")
        {
            var deposito = await _cuentaService.depositoCuenta(cuenta.numeroCuenta, monto);

            transaccion = await guardarTransaccion(deposito, monto, tipoTransaccion, "Exitoso");
        }
        else if(tipoTransaccion.ToUpper() == "RETIRO"){

            var retiro = await _cuentaService.retiroCuenta(numeroCuenta, monto);

            transaccion = await guardarTransaccion(retiro, monto, tipoTransaccion, "Exitoso");
        }

        return transaccion;

    }

    public async Task<Transaccion> guardarTransaccion(Cuenta cuenta, decimal montoTransaccion, string tipoTransaccion, string estado){

        var transaccion = new Transaccion{
            idCuenta = cuenta.id,
            tipoTransaccion = tipoTransaccion,
            estado = estado,
            fecha = DateTime.Now,
            monto = montoTransaccion,
            saldoFinal = cuenta.saldo,
            cuenta = cuenta
        };

        _context.Transacciones.Add(transaccion);
        await _context.SaveChangesAsync();

        return transaccion;
    }

    public async Task<ResumenTransaccionResponse> resumenTransaccion(string numeroCuenta, string identificacion){

        var cuenta = await _cuentaService.consultarSaldo(numeroCuenta, identificacion);
        var resumenTransaccion =  await _context.Transacciones.Where(t => t.idCuenta == cuenta.id).ToListAsync();
        
        var resumenTransaccionResponse = new ResumenTransaccionResponse{
            transacciones = resumenTransaccion,
            saldoDisponible = cuenta.saldo
        };

        return resumenTransaccionResponse;
    }


}