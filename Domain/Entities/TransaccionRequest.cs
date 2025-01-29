
namespace BancaApi.Domain.Entities;

public class TransaccionRequest
{
    public string numeroCuenta { get; set; }
    public string identificacion { get; set; }

    public decimal monto { get; set; }
}

