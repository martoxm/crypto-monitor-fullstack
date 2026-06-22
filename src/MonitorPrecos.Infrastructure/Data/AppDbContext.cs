using Microsoft.EntityFrameworkCore;
using MonitorPrecos.Domain.Entities;

namespace MonitorPrecos.Infrastructure.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<RegistroPreco> RegistrosPrecos { get; set; }
}