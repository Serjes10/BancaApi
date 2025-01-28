using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace BancaApi.Domain.Entities;


public class Cliente
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)] 
    public int id {get; set;}

    public string nombre {get; set;}

    public string identificacion{get; set;}
    public DateTime fechaNacimiento {get; set;}
    
    public string sexo {get; set;}

    public decimal ingresos {get; set;}

    public DateTime fechaCreacion {get; set;}

    public Boolean estado {get; set;}
}
