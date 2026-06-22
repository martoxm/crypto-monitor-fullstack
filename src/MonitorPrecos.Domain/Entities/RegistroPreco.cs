namespace MonitorPrecos.Domain.Entities;

public class RegistroPreco
{
    public int Id { get; set; }

    public string Moeda { get; set; } = string.Empty;

    public decimal ValorUsd { get; set; }

    public DateTime DataRegistro { get; set; } = DateTime.Now;
}