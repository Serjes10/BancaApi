using BancaApi.Domain.Entities;
using BancaApi.Data;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System.Text;
using System.Net;

namespace BancaApi.Tests;

[Collection("Sequential")]
public class CuentaControllerTests : IClassFixture<WebApplicationFactory<Program>>
{

    private readonly HttpClient _client;
    private readonly WebApplicationFactory<Program> _factory;

    public CuentaControllerTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    /* Prueba para validar la consulta de saldo de la cuenta creada*/

    [Fact]
    public async Task ConsultarSaldo_OkResponse()
    {
        using var scope = _factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<BancaDbContext>();

        await context.Database.EnsureCreatedAsync();

        context.Cuentas.RemoveRange(context.Cuentas);
        context.Clientes.RemoveRange(context.Clientes);
        await context.SaveChangesAsync();

        var cliente = new Cliente
        {
            nombre = "Jorge Morales",
            identificacion = "98756431",
            fechaNacimiento = DateTime.Parse("2000-01-01"),
            sexo = "Masculino",
            ingresos = 10000
        };
        context.Clientes.Add(cliente);
        await context.SaveChangesAsync();

        var cuenta = new Cuenta
        {
            numeroCuenta = "1009",
            saldo = 5000,
            estado = true,
            tipoCuenta = "Ahorro",
            idCliente = cliente.id
        };
        context.Cuentas.Add(cuenta);
        await context.SaveChangesAsync();

        var response = await _client.GetAsync($"/api/dev/v1/cuenta/1009/cliente/98756431");

        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        var responseObject = JsonConvert.DeserializeObject<Response<Cuenta>>(content);
        Assert.NotNull(responseObject);
        Assert.NotNull(responseObject.Data);
        Assert.Equal(5000, responseObject.Data.saldo);
    }

    /* Prueba para validar la consulta de saldo de una cuenta que no existe*/

    [Fact]
    public async Task ConsultarSaldo_NotFoundAccountReponse()
    {
        using var scope = _factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<BancaDbContext>();

        await context.Database.EnsureCreatedAsync();

        context.Cuentas.RemoveRange(context.Cuentas);
        context.Clientes.RemoveRange(context.Clientes);
        await context.SaveChangesAsync();
        var response = await _client.GetAsync("/api/dev/v1/cuenta/8888/cliente/1234");
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        var content = await response.Content.ReadAsStringAsync();
        var responseObject = JsonConvert.DeserializeObject<Response<string>>(content);
        Assert.NotNull(responseObject);
        Assert.Contains("Cuenta no encontrada", responseObject.Errors);
    }

    /* Prueba para validar la creacion de cuenta*/

    [Fact]
    public async Task CrearCuenta_OkResponse()
    {
        using var scope = _factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<BancaDbContext>();

        await context.Database.EnsureCreatedAsync();

        context.Cuentas.RemoveRange(context.Cuentas);
        context.Clientes.RemoveRange(context.Clientes);
        await context.SaveChangesAsync();

        var cliente = new Cliente
        {
            nombre = "Jorge Morales",
            identificacion = "98756431",
            fechaNacimiento = DateTime.Parse("2000-01-01"),
            sexo = "Masculino",
            ingresos = 10000
        };
        context.Clientes.Add(cliente);
        await context.SaveChangesAsync();

        var nuevaCuenta = new { idCliente = cliente.id, saldoInicial = 1500, tipoCuenta = "Ahorro" };
        var jsonContent = new StringContent(JsonConvert.SerializeObject(nuevaCuenta), Encoding.UTF8, "application/json");

        var response = await _client.PostAsync("/api/dev/v1/cuenta", jsonContent);
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        var responseObject = JsonConvert.DeserializeObject<Response<Cuenta>>(content);
        Assert.NotNull(responseObject);
        Assert.NotNull(responseObject.Data);
        Assert.Equal(1500, responseObject.Data.saldo);

    }

    /* Prueba para validar la creacion de cuenta de un cliente que no existe esperando el mensaje de error cliente no encontrado*/

    [Fact]
    public async Task CrearCuenta_NotFounClientResponseError()
    {
        var nuevaCuenta = new { idCliente = 99, saldoInicial = 2000, tipoCuenta = "Ahorro" };
        var jsonContent = new StringContent(JsonConvert.SerializeObject(nuevaCuenta), Encoding.UTF8, "application/json");

        var response = await _client.PostAsync("/api/dev/v1/cuenta", jsonContent);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        var content = await response.Content.ReadAsStringAsync();
        var responseObject = JsonConvert.DeserializeObject<Response<string>>(content);
        Assert.NotNull(responseObject);
        Assert.Contains("Cliente no encontrado", responseObject.Errors);
    }

}