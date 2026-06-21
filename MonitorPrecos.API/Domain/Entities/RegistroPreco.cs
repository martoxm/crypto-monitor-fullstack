using System;

namespace MonitorPrecos.API.Domain.Entities;

public class RegistroPreco
{
    // O EF Core entende que o nome "Id" será a Chave Primária (Auto-incremento) da tabela
    public int Id { get; set; }

    public string Moeda { get; set; } = string.Empty;

    // Salvamos o valor final direto como decimal para facilitar cálculos futuros
    public decimal ValorUsd { get; set; }

    // Registra o momento exato em que o dado entrou no nosso banco
    public DateTime DataRegistro { get; set; } = DateTime.UtcNow;
}