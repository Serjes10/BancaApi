using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace BancaApi.Domain.Entities;

public class Transaccion
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int id {get; set;}
    public string tipoTransaccion {get; set;}
    public decimal monto {get; set;}
    public DateTime fecha {get; set;}

    public int idCuenta {get; set;}
    
    [ForeignKey("idCuenta")]
    public Cuenta cuenta {get; set;}
}
