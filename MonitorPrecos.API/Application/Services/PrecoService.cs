using MonitorPrecos.API.Domain.Entities;
using MonitorPrecos.API.Domain.Interfaces;
using MonitorPrecos.API.Models;

namespace MonitorPrecos.API.Application.Services;

public class PrecoService(IPrecoRepository repository)
{
    private readonly IPrecoRepository _repository = repository;

    public async Task<RegistroPreco> ProcessarESalvarPrecoAsync(PrecoCryptoDto dto)
    {
        // Aqui aplicaríamos regras de negócio se necessário (SOLID - Responsabilidade Única)
        var novoRegistro = new RegistroPreco
        {
            Moeda = dto.Moeda,
            ValorUsd = dto.Preco.Usd,
            DataRegistro = DateTime.Now // 
        };

        await _repository.AdicionarAsync(novoRegistro);
        return novoRegistro;
    }

    public async Task<IEnumerable<RegistroPreco>> BuscarHistoricoAsync()
    {
        return await _repository.ObterTodosAsync();
    }
}