using MonitorPrecos.Application.DTOs;
using MonitorPrecos.Domain.Entities;
using MonitorPrecos.Domain.Interfaces;

namespace MonitorPrecos.Application.UseCase;

public class PrecoUseCase(IPrecoRepository repository)
{
    private readonly IPrecoRepository _repository = repository;

    public async Task<RegistroPreco> ProcessarESalvarPrecoAsync(PrecoCryptoDto dto)
    {
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