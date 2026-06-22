using Microsoft.EntityFrameworkCore;
using MonitorPrecos.Application.UseCase;
using MonitorPrecos.Domain.Interfaces;
using MonitorPrecos.Infrastructure.Data;
using MonitorPrecos.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRouting(option => option.LowercaseUrls = true);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=monitor.db"));

builder.Services.AddScoped<IPrecoRepository, PrecoRepository>();
builder.Services.AddScoped<PrecoUseCase>();

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll");

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    context.Database.EnsureCreated();
}
app.Run();