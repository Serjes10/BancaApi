using BancaApi.Domain.Entities;
using BancaApi.Data;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System.Text;

namespace BancaApi.Tests
{
    public class ClienteControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;
        private readonly WebApplicationFactory<Program> _factory;

        public ClienteControllerTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
            _client = factory.CreateClient();
        }

        /* Prueba para validar que al obtener todos los clientes devuelva la informacion*/

        [Fact]
        public async Task ConsultarClientes_OkReponse()
        {
            var response = await _client.GetAsync("/api/dev/v1/cliente");

            response.EnsureSuccessStatusCode();  
            var content = await response.Content.ReadAsStringAsync();

            var responseObject = JsonConvert.DeserializeObject<Response<object>>(content);
            Assert.NotNull(responseObject);
            Assert.NotNull(responseObject.Data); 
        }

        /* Prueba para validar que al insertar un cliente y luego consultarlo por la identificacion retorne un codigo 200 y los datos del cliente*/
        [Fact]
        public async Task ConsultarClienteByIdentificacion_OkResponse()
        {
            using var scope = _factory.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<BancaDbContext>();

            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            var cliente = new Cliente
            {
                nombre = "Sergio Ortega",
                identificacion = "1234",
                fechaNacimiento = DateTime.Parse("1980-01-01"),
                sexo = "Masculino",
                ingresos = 5000,
                fechaCreacion = DateTime.Now,
                estado = true
            };

            context.Clientes.Add(cliente);
            await context.SaveChangesAsync();

            var response = await _client.GetAsync("/api/dev/v1/cliente/1234");

            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();

            var responseObject = JsonConvert.DeserializeObject<Response<Cliente>>(content);
            Assert.NotNull(responseObject);
            Assert.NotNull(responseObject.Data);
            Assert.Equal("Sergio Ortega", responseObject.Data.nombre);
        }

        /* Prueba para validar que al consultar un cliente que no existe devuelva un 400 y un mensaje del Cliente no encontrado*/
        [Fact]
        public async Task ConsultarClienteByIdentificacion_BadRequestResponse()
        {
            var response = await _client.GetAsync("/api/dev/v1/cliente/9999");

            Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
            var content = await response.Content.ReadAsStringAsync();

            var responseObject = JsonConvert.DeserializeObject<Response<string>>(content);
            Assert.NotNull(responseObject);
            Assert.Contains("Cliente no encontrado", responseObject.Errors);
        }

        /* En esta prueba se realiza la insercion de un cliente*/
        [Fact]
        public async Task CrearCliente_ReturnOK()
        {
            var nuevoCliente = new Cliente
            {
                nombre = "Jorge Morales",
                identificacion = "12345",
                fechaNacimiento = DateTime.Parse("2000-01-01"),
                sexo = "Masculino",
                ingresos = 10000
            };

            var jsonContent = JsonConvert.SerializeObject(nuevoCliente);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("/api/dev/v1/cliente", content);

            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
            var responseContent = await response.Content.ReadAsStringAsync();

            var responseObject = JsonConvert.DeserializeObject<Response<Cliente>>(responseContent);
            Assert.NotNull(responseObject);
            Assert.NotNull(responseObject.Data);
            Assert.Equal("12345", responseObject.Data.identificacion);
        }


        /* En esta prueba se realiza la insercion de dos clientes con los mismos datos para validar que no genere duplicados*/
        [Fact]
        public async Task CrearCliente_ClientExist()
        {
            using var scope = _factory.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<BancaDbContext>();

            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            var clienteExistente = new Cliente
            {
                nombre = "Sergio Ortega",
                identificacion = "1234",
                fechaNacimiento = DateTime.Parse("1980-01-01"),
                sexo = "Masculino",
                ingresos = 5000
            };

            context.Clientes.Add(clienteExistente);
            await context.SaveChangesAsync();

            var clienteDuplicado = new Cliente
            {
                nombre = "Sergio Ortega",
                identificacion = "1234", 
                fechaNacimiento = DateTime.Parse("1980-01-01"),
                sexo = "Masculino",
                ingresos = 5000
            };

            var jsonContent = JsonConvert.SerializeObject(clienteDuplicado);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("/api/dev/v1/cliente", content);

            Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
            var responseContent = await response.Content.ReadAsStringAsync();

            var responseObject = JsonConvert.DeserializeObject<Response<string>>(responseContent);
            Assert.NotNull(responseObject);
            Assert.Contains("El cliente ingresado ya existe", responseObject.Errors);
        }



    }
}
