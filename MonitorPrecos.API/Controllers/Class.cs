using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MonitorPrecos.API.Data;
using MonitorPrecos.API.Models;

namespace MonitorPrecos.API.Controllers;

[ApiController]
[Route("api/[controller]")] // Isso define a URL como: api/precos
public class PrecosController : ControllerBase
{
    private readonly AppDbContext _context;

    // Injeção de Dependência: o .NET injeta o nosso banco de dados aqui automaticamente
    public PrecosController(AppDbContext context)
    {
        _context = context;
    }

    // 1. ENDPOINT PARA RECEBER OS DADOS DO N8N (POST)
    [HttpPost]
    public async Task<IActionResult> ReceberPreco([FromBody] PrecoCryptoDto dadosDoN8n)
    {
        // Validação básica de segurança/dados
        if (dadosDoN8n == null || dadosDoN8n.Preco == null)
        {
            return BadRequest("Dados inválidos ou mal formatados.");
        }

        // Criamos o objeto que vai para a tabela do banco de dados
        var novoRegistro = new RegistroPreco
        {
            Moeda = dadosDoN8n.Moeda,
            ValorUsd = dadosDoN8n.Preco.Usd,
            DataRegistro = DateTime.UtcNow
        };

        // Adiciona e salva no SQLite de forma assíncrona
        _context.RegistrosPrecos.Add(novoRegistro);
        await _context.SaveChangesAsync();

        // Retorna o status 200 OK avisando o n8n que deu tudo certo!
        return Ok(new { mensagem = "Preço registrado com sucesso com .NET 10!", id = novoRegistro.Id });
    }

    // 2. ENDPOINT PARA CONSULTAR OS PREÇOS SALVOS (GET)
    // Excelente para listar no seu portfólio ou em um dashboard posterior
    [HttpGet]
    public async Task<IActionResult> ObterHistorico()
    {
        var historico = await _context.RegistrosPrecos
            .OrderByDescending(p => p.DataRegistro)
            .ToListAsync();

        return Ok(historico);
    }
}