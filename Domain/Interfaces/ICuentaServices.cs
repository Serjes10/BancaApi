using BancaApi.Domain.Entities;

public interface ICuentaServices
{
    Task<Cuenta> crearCuenta(int idCliente, decimal saldoInicial, string tipoCuenta);
    Task<Cuenta> consultarSaldo(string numeroCuenta, string identificacion);

    Task<Cuenta> depositoCuenta(string numeroCuenta, decimal monto);

    Task<Cuenta> retiroCuenta(string numeroCuenta, decimal monto);

}
