using BancaApi.Data;
using BancaApi.Domain.Entities;
using BancaApi.Domain.Interface;
using Microsoft.EntityFrameworkCore;

namespace BancaApi.Services;

public class ClienteService : IClienteServices
{
    private readonly BancaDbContext _context;

    public ClienteService(BancaDbContext context)
    {
        _context = context;
    }

    public async Task<List<Cliente>> consultarClientes()
    {
        var clientes = await _context.Clientes.ToListAsync();

        if (clientes == null)
        {
            throw new Exception("Ocurrio un error al consultar los clientes");
        }

        return clientes;
    }

    public async Task<Cliente> consultarClienteByIndentificacion(string identificacion)
    {
        var cliente = await _context.Clientes.FirstOrDefaultAsync(c => c.identificacion == identificacion);
        if (cliente == null)
        {
            throw new Exception("Cliente no encontrado");
        }

        return cliente;
    }

    public async Task<Cliente> consultarClienteById(int id)
    {
        var cliente = await _context.Clientes.FirstOrDefaultAsync(c => c.id == id);
        if (cliente == null)
        {
            throw new Exception("Cliente no encontrado");
        }

        return cliente;
    }

    public async Task<Cliente> crearCliente(string nombre, string identificacion, DateTime fechaNacimiento, string sexo, decimal ingresos)
    {

        var cliente = await consultarClienteByIndentificacion(identificacion);

        if(cliente != null){
            throw new Exception("El cliente ingresado ya existe");

        }

        // Crear cliente
        var nuevoCliente = new Cliente
        {
            nombre = nombre,
            identificacion = identificacion,
            fechaNacimiento = fechaNacimiento,
            sexo = sexo,
            ingresos = ingresos,
            fechaCreacion = DateTime.Now,
            estado = true
        };

        validarCliente(nuevoCliente);

        _context.Clientes.Add(nuevoCliente);
        await _context.SaveChangesAsync();

        return nuevoCliente;
    }

    public void validarCliente(Cliente cliente)
    {
        if (string.IsNullOrEmpty(cliente.nombre))
        {
            throw new ArgumentException("El nombre no puede ser nulo o vacío.");
        }

        if (string.IsNullOrEmpty(cliente.identificacion))
        {
            throw new ArgumentException("La identificacion no puede ser nulo o vacio.");
        }

        if (string.IsNullOrEmpty(cliente.sexo))
        {
            throw new ArgumentException("El sexo del cliente no puede ser nulo o vacio");
        }

        if (cliente.ingresos < 0)
        {
            throw new ArgumentException("Los ingresos deben ser mayores a cero");
        }

        if (cliente.fechaNacimiento == DateTime.MinValue)
        {
            throw new ArgumentException("La fecha de nacimiento no puede ser inválida.", nameof(cliente.fechaNacimiento));
        }

    }

}