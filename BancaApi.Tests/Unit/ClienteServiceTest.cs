using BancaApi.Domain.Entities;
using BancaApi.Data;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using BancaApi.Services;
using Xunit;
using System.Collections.Generic;
using System.Threading.Tasks;


[Collection("Sequential")]
public class ClienteServiceTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public ClienteServiceTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    /*Prueba para verificar la consulta de clientes*/
    [Fact]
    public async Task ConsultarClientes_ReturnsClientes()
    {
        using var scope = _factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<BancaDbContext>();

        await context.Database.EnsureCreatedAsync();

        context.Cuentas.RemoveRange(context.Cuentas);
        context.Clientes.RemoveRange(context.Clientes);

        var clientes = new List<Cliente>
        {
            new Cliente { id = 1, nombre = "Jose", identificacion = "98756431771", sexo = "Masculino" },
            new Cliente { id = 2, nombre = "Maria", identificacion = "98756431782", sexo = "Femenino"  }
        };
        context.Clientes.AddRange(clientes);
        await context.SaveChangesAsync();

        var clienteService = new ClienteService(context);

        var result = await clienteService.consultarClientes();

        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
        Assert.Equal("Jose", result[0].nombre);
    }

    /*Prueba para verificar la creacion de clientes exitosa*/
    [Fact]
    public async Task CrearCliente_CreateClientOkResponse()
    {
        using var scope = _factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<BancaDbContext>();

        await context.Database.EnsureCreatedAsync();

        var nuevoCliente = new Cliente
        {
            nombre = "Mario Moncada",
            identificacion = "9875643180",
            fechaNacimiento = DateTime.Parse("1990-05-10"),
            sexo = "Masculino",
            ingresos = 15000
        };


        context.Clientes.RemoveRange(context.Clientes);
        await context.SaveChangesAsync();

        var clienteService = new ClienteService(context);

        var result = await clienteService.crearCliente(
            nuevoCliente.nombre,
            nuevoCliente.identificacion,
            nuevoCliente.fechaNacimiento,
            nuevoCliente.sexo,
            nuevoCliente.ingresos
        );

        Assert.NotNull(result);
        Assert.Equal(nuevoCliente.nombre, result.nombre);
        Assert.Equal(nuevoCliente.identificacion, result.identificacion);
    }

    /*Prueba para verificar que no exista duplicidad de clientes*/
    [Fact]
    public async Task CrearCliente_ThrowsArgumentException_WhenClienteAlreadyExists()
    {
        using var scope = _factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<BancaDbContext>();

        await context.Database.EnsureCreatedAsync();

        var clienteExistente = new Cliente
        {
            nombre = "Carlos",
            identificacion = "9875643180",
            fechaNacimiento = DateTime.Parse("1990-05-10"),
            sexo = "Masculino",
            ingresos = 15000
        };

        await context.Database.EnsureCreatedAsync(); 

        context.Clientes.Add(clienteExistente);
        await context.SaveChangesAsync();

        var clienteService = new ClienteService(context);


        var exception = await Assert.ThrowsAsync<ArgumentException>(() => clienteService.crearCliente(
            clienteExistente.nombre,
            clienteExistente.identificacion,
            clienteExistente.fechaNacimiento,
            clienteExistente.sexo,
            clienteExistente.ingresos
        ));

        Assert.Equal("El cliente ingresado ya existe", exception.Message);
    }



}
