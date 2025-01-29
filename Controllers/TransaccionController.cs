
using BancaApi.Domain.Entities;
using BancaApi.Domain.Interface;
using Microsoft.AspNetCore.Mvc;

namespace BancaApi.Controllers;

[ApiController]
[Route("api/dev/v1/transaccion")]
public class TransaccionController : ControllerBase
{
    private readonly ITransaccionService _transaccionService;

    public TransaccionController(ITransaccionService transaccionService)
    {
        _transaccionService = transaccionService;
    }

    [HttpPost("deposito")]
    public async Task<IActionResult> depositoCuenta([FromBody] TransaccionRequest request)
    {
        try
        {
            var deposito = await _transaccionService.generarTransaccion("Deposito", request.monto, request.numeroCuenta, request.identificacion);
            return Ok(new Response<object>(deposito));
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

    [HttpPost("retiro")]
    public async Task<IActionResult> retiroCuenta([FromBody] TransaccionRequest request)
    {
        try
        {
            var deposito = await _transaccionService.generarTransaccion("Retiro", request.monto, request.numeroCuenta, request.identificacion);

            return Ok(new Response<object>(deposito));
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

    [HttpGet("resumen/cuenta/{numeroCuenta}/cliente/{identificacion}")]
    public async Task<IActionResult> resumenTransacciones(string numeroCuenta, string identificacion)
    {
        try
        {
            var resumen = await _transaccionService.resumenTransaccion(numeroCuenta, identificacion);

            Console.WriteLine(resumen);
            return Ok(new Response<object>(resumen));
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