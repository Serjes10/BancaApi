
using BancaApi.Domain.Entities;

namespace BancaApi.Domain;
public class ResumenTransaccionResponse
{
    public List<Transaccion> transacciones { get; set;}
    public decimal saldoDisponible {get; set;}
}