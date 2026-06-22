using MonitorPrecos.Domain.Entities;

namespace MonitorPrecos.Domain.Interfaces;

public interface IPrecoRepository
{
    Task AdicionarAsync(RegistroPreco registro);
    Task<IEnumerable<RegistroPreco>> ObterTodosAsync();
}