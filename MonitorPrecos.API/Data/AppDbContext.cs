using Microsoft.EntityFrameworkCore;
using MonitorPrecos.API.Models;

namespace MonitorPrecos.API.Data;

public class AppDbContext : DbContext
{
    // O construtor repassa as configurações de conexão para o Entity Framework
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    // Esta propriedade avisa ao EF que queremos criar uma tabela chamada "RegistrosPrecos" baseada na classe RegistroPreco
    public DbSet<RegistroPreco> RegistrosPrecos { get; set; }
}