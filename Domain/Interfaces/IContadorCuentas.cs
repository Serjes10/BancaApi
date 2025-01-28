
using BancaApi.Domain.Entities;

namespace BancaApi.Domain.Interface;

public interface IContadorCuentas
{

    Task<ContadorCuentas> obtenerContador(string tipoCuenta);

   
}