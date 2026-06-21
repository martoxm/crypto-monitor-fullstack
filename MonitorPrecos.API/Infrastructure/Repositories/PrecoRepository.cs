using Microsoft.EntityFrameworkCore;
using MonitorPrecos.API.Domain.Entities;
using MonitorPrecos.API.Domain.Interfaces;
using MonitorPrecos.API.Infrastructure.Data;

namespace MonitorPrecos.API.Infrastructure.Repositories;

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
        // O comando Take(10) garante que o banco de dados só trará as 10 últimas capturas
        return await _context.RegistrosPrecos
            .OrderByDescending(p => p.DataRegistro)
            .Take(10)
            .ToListAsync();
    }
}