using Microsoft.AspNetCore.Mvc;
using MonitorPrecos.API.Application.Services;
using MonitorPrecos.API.Models;

namespace MonitorPrecos.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PrecosController(PrecoService precoService) : ControllerBase
{
    private readonly PrecoService _precoService = precoService;

    [HttpPost]
    public async Task<IActionResult> ReceberPreco([FromBody] PrecoCryptoDto dadosDoN8n)
    {
        if (dadosDoN8n == null || dadosDoN8n.Preco == null)
            return BadRequest("Dados inválidos.");

        var resultado = await _precoService.ProcessarESalvarPrecoAsync(dadosDoN8n);

        return Ok(new { mensagem = "Preço registrado via DDD!", id = resultado.Id });
    }

    [HttpGet]
    public async Task<IActionResult> ObterHistorico()
    {
        var historical = await _precoService.BuscarHistoricoAsync();
        return Ok(historical);
    }
}