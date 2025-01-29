using BancaApi.Domain.Entities;
using BancaApi.Data;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System.Text;
using System.Net;
using Microsoft.EntityFrameworkCore;
using BancaApi.Domain;

namespace BancaApi.Tests;

[Collection("Sequential")]
public class TransaccionControllerTest : IClassFixture<WebApplicationFactory<Program>>
{

    private readonly HttpClient _client;
    private readonly WebApplicationFactory<Program> _factory;

    public TransaccionControllerTest(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    /*Prueba para verificar que al realizar depositos se actualice correctamente el monto*/
    [Fact]
    public async Task DepositoCuenta_OkResponse()
    {
        using var scope = _factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<BancaDbContext>();

        await context.Database.EnsureCreatedAsync();

        context.Cuentas.RemoveRange(context.Cuentas);
        context.Clientes.RemoveRange(context.Clientes);
        await context.SaveChangesAsync();

        var cliente = new Cliente
        {
            nombre = "Jose Morales",
            identificacion = "9875643177",
            fechaNacimiento = DateTime.Parse("2000-01-01"),
            sexo = "Masculino",
            ingresos = 10000
        };
        context.Clientes.Add(cliente);
        await context.SaveChangesAsync();

        var cuenta = new Cuenta
        {
            numeroCuenta = "5555",
            saldo = 2000,
            estado = true,
            tipoCuenta = "Ahorro",
            idCliente = cliente.id
        };
        context.Cuentas.Add(cuenta);
        await context.SaveChangesAsync();

        var transaccionRequest = new TransaccionRequest
        {
            numeroCuenta = "5555",
            identificacion = "9875643177",
            monto = 5000
        };

        var jsonContent = new StringContent(JsonConvert.SerializeObject(transaccionRequest), Encoding.UTF8, "application/json");

        var response = await _client.PostAsync("/api/dev/v1/transaccion/deposito", jsonContent);
        response.EnsureSuccessStatusCode();

        await context.SaveChangesAsync();

        var content = await response.Content.ReadAsStringAsync();
        var responseObject = JsonConvert.DeserializeObject<Response<object>>(content);


        Assert.NotNull(responseObject);
        Assert.NotNull(responseObject.Data);

        var cuentaActualizada = await context.Cuentas.AsNoTracking().FirstOrDefaultAsync(c => c.numeroCuenta == cuenta.numeroCuenta);

        Assert.Equal(7000, cuentaActualizada.saldo);
    }

    /*Prueba para verificar que al realizar depositos el monto debe de ser mayor a 0*/

    [Fact]
    public async Task DepositoCuenta_IncorrectAmountResponseBadRequest()
    {
        using var scope = _factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<BancaDbContext>();

        await context.Database.EnsureCreatedAsync();

        context.Cuentas.RemoveRange(context.Cuentas);
        context.Clientes.RemoveRange(context.Clientes);
        await context.SaveChangesAsync();

        var cliente = new Cliente
        {
            nombre = "Jose Morales",
            identificacion = "9875643177",
            fechaNacimiento = DateTime.Parse("2000-01-01"),
            sexo = "Masculino",
            ingresos = 10000
        };
        context.Clientes.Add(cliente);
        await context.SaveChangesAsync();

        var cuenta = new Cuenta
        {
            numeroCuenta = "5555",
            saldo = 2000,
            estado = true,
            tipoCuenta = "Ahorro",
            idCliente = cliente.id
        };
        context.Cuentas.Add(cuenta);
        await context.SaveChangesAsync();

        var transaccionRequest = new TransaccionRequest
        {
            numeroCuenta = "5555",
            identificacion = "9875643177",
            monto = 0
        };

        var jsonContent = new StringContent(JsonConvert.SerializeObject(transaccionRequest), Encoding.UTF8, "application/json");

        var response = await _client.PostAsync("/api/dev/v1/transaccion/deposito", jsonContent);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        var content = await response.Content.ReadAsStringAsync();
        var responseObject = JsonConvert.DeserializeObject<Response<string>>(content);

        Assert.NotNull(responseObject);
        Assert.Contains("El monto no puede ser menor o igual a 0", responseObject.Errors);
    }

    /*Prueba para realizar retiros de cuenta esperando que el monto se actualice a 4000 luego del retiro de 1000*/
    [Fact]
    public async Task RetiroCuenta_OkResponse()
    {
        using var scope = _factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<BancaDbContext>();

        await context.Database.EnsureCreatedAsync();

        context.Cuentas.RemoveRange(context.Cuentas);
        context.Clientes.RemoveRange(context.Clientes);
        await context.SaveChangesAsync();

        var cliente = new Cliente
        {
            nombre = "David Rodriguez",
            identificacion = "9875643135",
            fechaNacimiento = DateTime.Parse("2000-01-01"),
            sexo = "Masculino",
            ingresos = 10000
        };
        context.Clientes.Add(cliente);
        await context.SaveChangesAsync();

        var cuenta = new Cuenta
        {
            numeroCuenta = "5005",
            saldo = 5000,
            estado = true,
            tipoCuenta = "Ahorro",
            idCliente = cliente.id
        };
        context.Cuentas.Add(cuenta);
        await context.SaveChangesAsync();

        var transaccionRequest = new TransaccionRequest
        {
            numeroCuenta = "5005",
            monto = 1000,
            identificacion = "9875643135"
        };

        var jsonContent = new StringContent(JsonConvert.SerializeObject(transaccionRequest), Encoding.UTF8, "application/json");

        var response = await _client.PostAsync("/api/dev/v1/transaccion/retiro", jsonContent);

        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        var responseObject = JsonConvert.DeserializeObject<Response<object>>(content);

        Assert.NotNull(responseObject);
        Assert.NotNull(responseObject.Data);

        var cuentaActualizada = await context.Cuentas.AsNoTracking().FirstOrDefaultAsync(c => c.numeroCuenta == cuenta.numeroCuenta);

        Assert.Equal(4000, cuentaActualizada.saldo);
    }

    /*Prueba para realizar retiros de cuenta esperando que el monto se actualice a 4000 luego del retiro de 1000*/
    [Fact]
    public async Task RetiroCuenta__IncorrectAmountBadRequestResponse()
    {
        using var scope = _factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<BancaDbContext>();

        await context.Database.EnsureCreatedAsync();

        context.Cuentas.RemoveRange(context.Cuentas);
        context.Clientes.RemoveRange(context.Clientes);
        await context.SaveChangesAsync();

        var cliente = new Cliente
        {
            nombre = "David Rodriguez",
            identificacion = "98756431353",
            fechaNacimiento = DateTime.Parse("2000-01-01"),
            sexo = "Masculino",
            ingresos = 10000
        };
        context.Clientes.Add(cliente);
        await context.SaveChangesAsync();

        var cuenta = new Cuenta
        {
            numeroCuenta = "5006",
            saldo = 5000,
            estado = true,
            tipoCuenta = "Ahorro",
            idCliente = cliente.id
        };
        context.Cuentas.Add(cuenta);
        await context.SaveChangesAsync();

        var transaccionRequest = new TransaccionRequest
        {
            numeroCuenta = "5006",
            monto = 6000,
            identificacion = "98756431353"
        };

        var jsonContent = new StringContent(JsonConvert.SerializeObject(transaccionRequest), Encoding.UTF8, "application/json");

        var response = await _client.PostAsync("/api/dev/v1/transaccion/retiro", jsonContent);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        var content = await response.Content.ReadAsStringAsync();
        var responseObject = JsonConvert.DeserializeObject<Response<string>>(content);

        Assert.NotNull(responseObject);
        Assert.Contains("La cuenta del cliente no cuenta con el saldo suficiente", responseObject.Errors);

    }

    /*Prueba para validar que al realizar transaccion de deposito y retiro el resumen incluya las dos transacciones*/
    [Fact]
    public async Task ResumenTransacciones_OkResponse()
    {
        using var scope = _factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<BancaDbContext>();

        await context.Database.EnsureCreatedAsync();

        context.Cuentas.RemoveRange(context.Cuentas);
        context.Clientes.RemoveRange(context.Clientes);
        await context.SaveChangesAsync();

        var cliente = new Cliente
        {
            nombre = "Isac Morales",
            identificacion = "322576",
            fechaNacimiento = DateTime.Parse("2000-01-01"),
            sexo = "Masculino",
            ingresos = 10000
        };
        context.Clientes.Add(cliente);
        await context.SaveChangesAsync();

        var cuenta = new Cuenta
        {
            numeroCuenta = "005831",
            saldo = 5000,
            estado = true,
            tipoCuenta = "Ahorro",
            idCliente = cliente.id
        };
        context.Cuentas.Add(cuenta);
        await context.SaveChangesAsync();

        // Realizamos un depósito
        var depositoRequest = new TransaccionRequest
        {
            numeroCuenta = "005831",
            monto = 1000,
            identificacion = "322576"
        };
        await _client.PostAsync("/api/dev/v1/transaccion/deposito", new StringContent(JsonConvert.SerializeObject(depositoRequest), Encoding.UTF8, "application/json") );

        // Realizamos un retiro
        var retiroRequest = new TransaccionRequest
        {
            numeroCuenta = "005831",
            monto = 500,
            identificacion = "322576"
        };
        
        await _client.PostAsync("/api/dev/v1/transaccion/retiro", new StringContent(JsonConvert.SerializeObject(retiroRequest), Encoding.UTF8, "application/json"));

        var response = await _client.GetAsync("/api/dev/v1/transaccion/resumen/cuenta/005831/cliente/322576");

        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        var responseObject = JsonConvert.DeserializeObject<Response<object>>(content);

        Assert.NotNull(responseObject);
        Assert.NotNull(responseObject.Data);

        var resumen = JsonConvert.DeserializeObject<ResumenTransaccionResponse>(responseObject.Data.ToString());
        Assert.Equal(2, resumen.transacciones.Count); 
        Assert.Equal(5500, resumen.saldoDisponible); 
    }

}

