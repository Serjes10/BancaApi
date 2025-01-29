namespace BancaApi.Domain.Entities;

public class CuentaRequest{
    public int idCliente{get; set;}
    public decimal saldoInicial {get; set;}

    public string tipoCuenta {get; set;}
}

