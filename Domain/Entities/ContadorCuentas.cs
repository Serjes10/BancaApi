
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BancaApi.Domain.Entities;

public class ContadorCuentas{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int id {get; set;}

    public int contador{get; set;}

    public string tipoCuenta {get; set;}

    public string correlativo {get; set;}
}