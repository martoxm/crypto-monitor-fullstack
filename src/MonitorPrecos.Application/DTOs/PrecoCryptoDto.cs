using System.Text.Json.Serialization;

namespace MonitorPrecos.Application.DTOs;

// Esta é a classe principal que o n8n vai enviar
public class PrecoCryptoDto
{
    // Avisa ao C# para ler "moeda" com 'm' minúsculo do JSON
    [JsonPropertyName("moeda")]
    public string Moeda { get; set; } = string.Empty;

    // Avisa ao C# para ler "preco" com 'p' minúsculo do JSON
    [JsonPropertyName("preco")]
    public PrecoDetalhe Preco { get; set; } = new PrecoDetalhe();
}

// Esta classe representa o que está dentro de "preco"
public class PrecoDetalhe
{
    // Avisa ao C# para ler "usd" com 'u' minúsculo do JSON
    [JsonPropertyName("usd")]
    public decimal Usd { get; set; }
}