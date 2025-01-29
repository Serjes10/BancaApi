using BancaApi.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace BancaApi.Controllers;

[ApiController]
[Route("api/dev/v1/cuenta")]
public class CuentaController : ControllerBase
{
    private readonly ICuentaServices _cuentaService;

    public CuentaController(ICuentaServices cuentaServices)
    {
        _cuentaService = cuentaServices;
    }

    [HttpGet("{numeroCuenta}/cliente/{identificacion}")]
    public async Task<IActionResult> consultarSaldo(string numeroCuenta, string identificacion)
    {
        try
        {
            var cuenta = await _cuentaService.consultarSaldo(numeroCuenta, identificacion);

            return Ok(new Response<object>(cuenta));
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
    public async Task<IActionResult> crearCuenta([FromBody] CuentaRequest request){
        try
        {
            var cuenta = await _cuentaService.crearCuenta(request.idCliente, request.saldoInicial, request.tipoCuenta);

            return Ok(new Response<object>(cuenta));

        }
        catch (ArgumentException ex)
        {
            return BadRequest(new Response<string>(new List<string> { ex.Message }));
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return StatusCode(400, new Response<string>(new List<string> { "Ocurrió un error interno: " + ex.Message }));
        }
    }


}