using BancaApi.Domain.Entities;

public interface ICuentaServices
{
    Task<Cuenta> CrearCuentaAsync(int idCliente, decimal saldoInicial, string tipoCuenta);
    Task<Cuenta> consultarSaldoAsync(string numeroCuenta);
}
