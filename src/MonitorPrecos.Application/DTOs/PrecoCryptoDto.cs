using System.Text.Json.Serialization;

namespace MonitorPrecos.Application.DTOs;

public class PrecoCryptoDto
{
    [JsonPropertyName("moeda")]
    public string Moeda { get; set; } = string.Empty;

    [JsonPropertyName("preco")]
    public PrecoDetalhe Preco { get; set; } = new PrecoDetalhe();
}

public class PrecoDetalhe
{
    [JsonPropertyName("usd")]
    public decimal Usd { get; set; }
}