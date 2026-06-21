using Microsoft.EntityFrameworkCore;
using MonitorPrecos.API.Application.Services;
using MonitorPrecos.API.Domain.Interfaces;
using MonitorPrecos.API.Infrastructure.Data;
using MonitorPrecos.API.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// ---------------------------------------------------------
// 1. CONFIGURAÇÃO DOS SERVIÇOS (Injeção de Dependência)
// ---------------------------------------------------------

builder.Services.AddRouting(option => option.LowercaseUrls = true);

// Configuração do Banco de Dados SQLite
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=monitor.db"));

// Correção da Injeção de Dependência (Padrão DDD / SOLID)
builder.Services.AddScoped<IPrecoRepository, PrecoRepository>();
builder.Services.AddScoped<PrecoService>();

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();

// Configuração da política de CORS
builder.Services.AddCors(options => {
    options.AddPolicy("AllowAll", policy => {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// ---------------------------------------------------------
// 2. CONFIGURAÇÃO DOS MIDDLEWARES (Ordem de Execução)
// ---------------------------------------------------------

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

// REGRA DE OURO: O CORS precisa vir antes de QUALQUER redirecionamento ou segurança
app.UseCors("AllowAll");

app.UseHttpsRedirection();
app.UseAuthorization();

// Roteamento dos seus Controllers da API
app.MapControllers();

// Executa o escopo de inicialização para garantir que o banco e as tabelas existam na VM
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<MonitorPrecos.API.Infrastructure.Data.AppDbContext>();
    // Esta linha cria o arquivo .db e todas as tabelas automaticamente se elas não existirem
    context.Database.EnsureCreated();
}
app.Run();