using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
namespace BancaApi.Domain.Entities;

public class Transaccion
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int id {get; set;}
    public string tipoTransaccion {get; set;}
    public decimal monto {get; set;}

    public decimal saldoFinal {get; set;}
    public DateTime fecha {get; set;}

    public int idCuenta {get; set;}
    
    [ForeignKey("idCuenta")]
    [JsonIgnore]
    public Cuenta cuenta {get; set;}

    public string estado {get; set;}
}
