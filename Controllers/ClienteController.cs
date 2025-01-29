using BancaApi.Domain.Entities;
using BancaApi.Domain.Interface;
using Microsoft.AspNetCore.Mvc;

namespace BancaApi.Controllers;

[ApiController]
[Route("api/dev/v1/cliente")]
public class ClienteController : ControllerBase
{
    private readonly IClienteServices _clienteServives;

    public ClienteController(IClienteServices clienteServices)
    {
        _clienteServives = clienteServices;
    }

    [HttpGet]
    public async Task<IActionResult> consultarClientes()
    {
        try
        {
            var clientes = await _clienteServives.consultarClientes();

            return Ok(new Response<object>(clientes));
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new Response<string>(new List<string> { ex.Message }));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new Response<string>(new List<string> { "Ocurrió un error interno: " + ex.Message }));
        }
    }

    [HttpGet("{identificacion}")]
    public async Task<IActionResult> ConsultarCliente(string identificacion)
    {
        try
        {
            var cliente = await _clienteServives.consultarClienteByIndentificacion(identificacion);

            if (cliente == null)
            {
                return StatusCode(400, new Response<string>(new List<string> { "Cliente no encontrado." }));
            }

            return Ok(new Response<object>(cliente));
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new Response<string>(new List<string> { ex.Message }));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new Response<string>(new List<string> { "Ocurrió un error interno: " + ex.Message }));
        }

    }

    [HttpPost]
    public async Task<IActionResult> crearCliente([FromBody] Cliente request)
    {
        try
        {
            var clienteCreado = await _clienteServives.crearCliente(
                request.nombre,
                request.identificacion,
                request.fechaNacimiento,
                request.sexo,
                request.ingresos
            );

            return Ok(new Response<object>(clienteCreado));
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new Response<string>(new List<string> { ex.Message }));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new Response<string>(new List<string> { "Ocurrió un error interno: " + ex.Message }));
        }
    }

}