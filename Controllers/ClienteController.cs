using BancaApi.Domain.Entities;
using BancaApi.Domain.Interface;
using Microsoft.AspNetCore.Mvc;

namespace BancaApi.Controllers;

[ApiController]
[Route("api/dev/v1/cliente")]
public class ClienteController : ControllerBase
{
    private readonly IClienteServices _clienteServives;
    private readonly ILogger<ClienteController> _logger;

    public ClienteController(ILogger<ClienteController> logger, IClienteServices clienteServices)
    {
        _clienteServives = clienteServices;
        _logger = logger;
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
            return StatusCode(400, new Response<string>(new List<string> { "Ocurrió un error interno: " + ex.Message }));
        }
    }

    [HttpGet("{identificacion}")]
    public async Task<IActionResult> ConsultarCliente(string identificacion)
    {
        try
        {
            var cliente = await _clienteServives.consultarClienteByIndentificacion(identificacion);

            return Ok(new Response<object>(cliente));
        }
        catch (ArgumentException ex) 
        {
            return BadRequest(new Response<string>(new List<string> { ex.Message }));
        }
        catch (Exception ex)
        {
            return StatusCode(400, new Response<string>(new List<string> { "Ocurrió un error interno: " + ex.Message }));
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
            return StatusCode(400, new Response<string>(new List<string> { "Ocurrió un error interno: " + ex.Message }));
        }
    }

}