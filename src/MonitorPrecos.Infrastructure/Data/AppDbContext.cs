using Microsoft.EntityFrameworkCore;
using MonitorPrecos.API.Domain.Entities;

namespace MonitorPrecos.API.Infrastructure.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{

    // Esta propriedade avisa ao EF que queremos criar uma tabela chamada "RegistrosPrecos" baseada na classe RegistroPreco
    public DbSet<RegistroPreco> RegistrosPrecos { get; set; }
}