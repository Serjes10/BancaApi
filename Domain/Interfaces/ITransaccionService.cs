
using BancaApi.Domain.Entities;

namespace BancaApi.Domain.Interface;

public interface ITransaccionService{
    Task<Transaccion> generarTransaccion(string tipoTransaccion, decimal monto, string numeroCuenta, string identificacion);

    Task<ResumenTransaccionResponse> resumenTransaccion(string numeroCuenta, string identificacion);

} 