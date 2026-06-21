using Microsoft.EntityFrameworkCore;
using MonitorPrecos.API.Application.Services;
using MonitorPrecos.API.Domain.Interfaces;
using MonitorPrecos.API.Infrastructure.Data;
using MonitorPrecos.API.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRouting(option => option.LowercaseUrls = true);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=monitor.db"));

builder.Services.AddScoped<IPrecoRepository, PrecoRepository>();
builder.Services.AddScoped<PrecoService>();

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