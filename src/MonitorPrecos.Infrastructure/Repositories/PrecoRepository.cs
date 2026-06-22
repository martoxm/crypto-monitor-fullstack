using Microsoft.EntityFrameworkCore;
using MonitorPrecos.Domain.Entities;
using MonitorPrecos.Domain.Interfaces;
using MonitorPrecos.Infrastructure.Data;

namespace MonitorPrecos.Infrastructure.Repositories;

public class PrecoRepository(AppDbContext context) : IPrecoRepository
{
    private readonly AppDbContext _context = context;

    public async Task AdicionarAsync(RegistroPreco registro)
    {
        _context.RegistrosPrecos.Add(registro);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<RegistroPreco>> ObterTodosAsync()
    {
        return await _context.RegistrosPrecos
            .OrderByDescending(p => p.DataRegistro)
            .Take(10)
            .ToListAsync();
    }
}