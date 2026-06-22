using MonitorPrecos.API.Domain.Entities;

namespace MonitorPrecos.API.Domain.Interfaces;

public interface IPrecoRepository
{
    Task AdicionarAsync(RegistroPreco registro);
    Task<IEnumerable<RegistroPreco>> ObterTodosAsync();
}