using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BancaApi.Domain.Entities;


public class Cuenta
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int id {get; set;}
    public string numeroCuenta {get; set;}

    public string tipoCuenta {get; set;}
    public decimal saldo {get; set;}
    public int idCliente {get; set;}

    [ForeignKey("idCliente")]
    public Cliente cliente {get; set;}
    public DateTime fechaCreacion {get; set;}

    public bool estado{get; set;}
}
