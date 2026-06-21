using Microsoft.EntityFrameworkCore;
using MonitorPrecos.API.Data;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddRouting(option => option.LowercaseUrls = true);
// Configurando o banco de dados SQLite apontando para um arquivo chamado "monitor.db"
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=monitor.db"));

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();

// IMPORTANTE: Adicione esta linha abaixo para que o .NET saiba como rotear seus Controllers
app.MapControllers();

app.Run();