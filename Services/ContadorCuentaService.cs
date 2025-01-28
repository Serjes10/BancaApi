using BancaApi.Data;
using BancaApi.Domain.Entities;
using BancaApi.Domain.Interface;
using Microsoft.EntityFrameworkCore;

namespace BancaApi.Services;

public class ContadorCuentaService : IContadorCuentas
{
    private readonly BancaDbContext _context;

    public ContadorCuentaService(BancaDbContext context)
    {
        _context = context;
    }

    public async Task<ContadorCuentas>obtenerContador(string tipoCuenta){
        
        var contador = await _context.ContadorCuentas.FirstOrDefaultAsync(c => c.tipoCuenta ==  tipoCuenta);
        ContadorCuentas contadorCuentas;
        
        if(contador == null){
            contadorCuentas = new ContadorCuentas{
                tipoCuenta = tipoCuenta,
                contador = 1,
                correlativo = obtenerCorrelativo(tipoCuenta)
            };

            _context.ContadorCuentas.Add(contadorCuentas);
            await _context.SaveChangesAsync();

        }else{
            contador.contador++;
            await _context.SaveChangesAsync();
            contadorCuentas = new ContadorCuentas{
                tipoCuenta = tipoCuenta,
                contador = contador.contador,
                correlativo = contador.correlativo
            };
        }

        return contadorCuentas;
    }

    public string obtenerCorrelativo(string tipoCuenta){
        if(tipoCuenta.ToUpper() == "AHORRO"){
            return "1000";
        }
        else if(tipoCuenta.ToUpper() == "CHEQUE"){
            return "2000";
        }
        else{
            return "3000";
        }
    }
}