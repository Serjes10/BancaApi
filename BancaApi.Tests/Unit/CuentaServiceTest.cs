using BancaApi.Domain.Entities;
using BancaApi.Data;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using BancaApi.Services;
using Xunit;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BancaApi.Domain.Interface;

[Collection("Sequential")]
public class CuentaServiceTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public CuentaServiceTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }


    /*Prueba para validar el servicio de creacion de cuentas*/
    [Fact]
    public async Task CrearCuenta_CreateAccountOkResponse()
    {
        using var scope = _factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<BancaDbContext>();

        await context.Database.EnsureCreatedAsync();

        var nuevoCliente = new Cliente
        {
            nombre = "Carlos Mendoza",
            identificacion = "9875643181",
            fechaNacimiento = DateTime.Parse("1985-11-10"),
            sexo = "Masculino",
            ingresos = 12000
        };

        context.Clientes.RemoveRange(context.Clientes);
        await context.SaveChangesAsync();

        context.Clientes.Add(nuevoCliente);
        await context.SaveChangesAsync();

        var cuentaService = new CuentaService(
            context,
            scope.ServiceProvider.GetRequiredService<IClienteServices>(),
            scope.ServiceProvider.GetRequiredService<IContadorCuentas>()
        );

        var result = await cuentaService.crearCuenta(nuevoCliente.id, 1500, "Ahorros");

        // Aserciones
        Assert.NotNull(result);
        Assert.Equal(nuevoCliente.id, result.idCliente);
        Assert.Equal(1500, result.saldo);
        Assert.Equal("Ahorros", result.tipoCuenta);
    }

    /*Prueba para validar el servicio de creacion de cuentas no permita creacion de cuentas con saldo inicial menor a 1000*/

    [Fact]
    public async Task CrearCuenta_CreateAccountBadResponseResponse()
    {
        using var scope = _factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<BancaDbContext>();

        await context.Database.EnsureCreatedAsync();

        var nuevoCliente = new Cliente
        {
            nombre = "Carlos Mendoza",
            identificacion = "9875643182",
            fechaNacimiento = DateTime.Parse("1985-11-10"),
            sexo = "Masculino",
            ingresos = 1500
        };

        context.Clientes.RemoveRange(context.Clientes);
        await context.SaveChangesAsync();

        context.Clientes.Add(nuevoCliente);
        await context.SaveChangesAsync();

        var cuentaService = new CuentaService(
            context,
            scope.ServiceProvider.GetRequiredService<IClienteServices>(),
            scope.ServiceProvider.GetRequiredService<IContadorCuentas>()
        );

        var exception = await Assert.ThrowsAsync<ArgumentException>(async () => await cuentaService.crearCuenta(nuevoCliente.id, 500, "Ahorros"));


        // Aserciones
        Assert.Equal("El saldo de apertura de la cuenta debe ser mayor a 1000.00", exception.Message);


    }

    /*Prueba para verificar el servicio de consulta de saldo*/
    [Fact]
    public async Task ConsultaSaldo_ConsultaSaldoOkResponse()
    {
        using var scope = _factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<BancaDbContext>();

        await context.Database.EnsureCreatedAsync();

        // Preparación de los datos
        var nuevoCliente = new Cliente
        {
            nombre = "Carlos Mendoza",
            identificacion = "2875643181",
            fechaNacimiento = DateTime.Parse("1985-11-10"),
            sexo = "Masculino",
            ingresos = 12000
        };

        context.Clientes.RemoveRange(context.Clientes); // Limpieza de datos existentes
        await context.SaveChangesAsync();

        context.Clientes.Add(nuevoCliente);
        await context.SaveChangesAsync();

        var cuentaService = new CuentaService(
            context,
            scope.ServiceProvider.GetRequiredService<IClienteServices>(),
            scope.ServiceProvider.GetRequiredService<IContadorCuentas>()
        );

        var cuenta = await cuentaService.crearCuenta(nuevoCliente.id, 1500, "Ahorros");

        // Ejecutar la operación de consulta de saldo
        var transaccionService = new TransaccionService(
            context,
            cuentaService
        );

        var result = await cuentaService.consultarSaldo(cuenta.numeroCuenta, nuevoCliente.identificacion);

        // Aserciones
        Assert.NotNull(result);
        Assert.Equal(cuenta.numeroCuenta, result.numeroCuenta);
        Assert.Equal(1500, result.saldo);
    }
}