using BancaApi.Domain.Entities;

namespace BancaApi.Domain.Interface;
public interface IClienteServices{
    Task<List<Cliente>> consultarClientes();

    Task<Cliente> consultarClienteByIndentificacion(string identificacion);

    Task<Cliente> consultarClienteById(int id);

    Task<Cliente> crearCliente(string nombre, string identificacion, DateTime fechaNacimiento, string sexo, decimal ingresos);

}