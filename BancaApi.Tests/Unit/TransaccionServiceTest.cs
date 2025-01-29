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


public class TransaccionServiceTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public TransaccionServiceTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    /*Prueba para validar el servicio de deposito de cuenta*/
    [Fact]
    public async Task GenerarTransaccion_DepositoOkResponse()
    {
        using var scope = _factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<BancaDbContext>();

        await context.Database.EnsureCreatedAsync();

        // Preparación de los datos
        var nuevoCliente = new Cliente
        {
            nombre = "Carlos Mendoza",
            identificacion = "9875643187",
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

        var cuenta = await cuentaService.crearCuenta(nuevoCliente.id, 1500, "Ahorros");

        var transaccionService = new TransaccionService(
            context,
            cuentaService
        );

        var result = await transaccionService.generarTransaccion("DEPOSITO", 500, cuenta.numeroCuenta, nuevoCliente.identificacion);

        Assert.NotNull(result);
        Assert.Equal("DEPOSITO", result.tipoTransaccion);
        Assert.Equal(500, result.monto);
        Assert.Equal("Exitoso", result.estado);
        Assert.Equal(2000, result.saldoFinal);
    }

    /*Prueba para validar que el monto del deposito debe ser mayor a 0*/
    [Fact]
    public async Task GenerarTransaccion_AmountBadRequestResponse()
    {
        using var scope = _factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<BancaDbContext>();

        await context.Database.EnsureCreatedAsync();

        var nuevoCliente = new Cliente
        {
            nombre = "Carlos Mendoza",
            identificacion = "9875643184",
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

        var cuenta = await cuentaService.crearCuenta(nuevoCliente.id, 1500, "Ahorros");

        var transaccionService = new TransaccionService(
            context,
            cuentaService
        );

        var exception = await Assert.ThrowsAsync<ArgumentException>(async () => await transaccionService.generarTransaccion("DEPOSITO", 0, cuenta.numeroCuenta, nuevoCliente.identificacion));

        Assert.Equal("El monto no puede ser menor o igual a 0", exception.Message);

    }

    /*Prueba para verificar el servicio de retiro*/
    [Fact]
    public async Task GenerarTransaccion_RetiroOkResponse()
    {
        using var scope = _factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<BancaDbContext>();

        await context.Database.EnsureCreatedAsync();

        var nuevoCliente = new Cliente
        {
            nombre = "Carlos Mendoza",
            identificacion = "4875643189",
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

        var cuenta = await cuentaService.crearCuenta(nuevoCliente.id, 1500, "Ahorros");

        var transaccionService = new TransaccionService(
            context,
            cuentaService
        );

        var result = await transaccionService.generarTransaccion("RETIRO", 500, cuenta.numeroCuenta, nuevoCliente.identificacion);

        Assert.NotNull(result);
        Assert.Equal("RETIRO", result.tipoTransaccion);
        Assert.Equal(500, result.monto);
        Assert.Equal("Exitoso", result.estado);
        Assert.Equal(1000, result.saldoFinal);
    }

    /*Prueba para verificar que el servicio de retiro no permita retirar fondos mayores al saldo disponible*/
    [Fact]
    public async Task GenerarTransaccion_AmountInsufficienteResponse()
    {
        using var scope = _factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<BancaDbContext>();

        await context.Database.EnsureCreatedAsync();

        var nuevoCliente = new Cliente
        {
            nombre = "Carlos Mendoza",
            identificacion = "5875643189",
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

        var cuenta = await cuentaService.crearCuenta(nuevoCliente.id, 1500, "Ahorros");

        var transaccionService = new TransaccionService(
            context,
            cuentaService
        );


        var exception = await Assert.ThrowsAsync<ArgumentException>(async () => await transaccionService.generarTransaccion("RETIRO", 2500, cuenta.numeroCuenta, nuevoCliente.identificacion));

        Assert.Equal("La cuenta del cliente no cuenta con el saldo suficiente", exception.Message);
    }


    /*Prueba para verificar el servicio de consulta de resumen de transacciones*/
    [Fact]
    public async Task ResumenTransaccion_OkResponse()
    {
        using var scope = _factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<BancaDbContext>();

        await context.Database.EnsureCreatedAsync();

        // Preparación de los datos
        var nuevoCliente = new Cliente
        {
            nombre = "Carlos Mendoza",
            identificacion = "8875643181",
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

        var cuenta = await cuentaService.crearCuenta(nuevoCliente.id, 1500, "Ahorros");

        var transaccionService = new TransaccionService(
            context,
            cuentaService
        );

        await transaccionService.generarTransaccion("DEPOSITO", 500, cuenta.numeroCuenta, nuevoCliente.identificacion);
        await transaccionService.generarTransaccion("RETIRO", 200, cuenta.numeroCuenta, nuevoCliente.identificacion);

        var result = await transaccionService.resumenTransaccion(cuenta.numeroCuenta, nuevoCliente.identificacion);

        Assert.NotNull(result);
        Assert.Equal(2, result.transacciones.Count); // Asegúrate de que haya 2 transacciones (1 depósito y 1 retiro)
        Assert.Equal(500, result.transacciones[0].monto); // Asegúrate de que el primer monto de transacción sea 500 (depósito)
        Assert.Equal("DEPOSITO", result.transacciones[0].tipoTransaccion); // Asegúrate de que la primera transacción sea un depósito
        Assert.Equal(200, result.transacciones[1].monto); // Asegúrate de que el segundo monto de transacción sea 200 (retiro)
        Assert.Equal("RETIRO", result.transacciones[1].tipoTransaccion); // Asegúrate de que la segunda transacción sea un retiro
    }



}