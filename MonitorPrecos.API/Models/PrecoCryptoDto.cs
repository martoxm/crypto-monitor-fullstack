namespace MonitorPrecos.API.Models;

// Esta é a classe principal que o n8n vai enviar
public class PrecoCryptoDto
{
    // Mapeia a propriedade "moeda" ("BTC")
    public string Moeda { get; set; } = string.Empty;

    // Mapeia o objeto "preco" que vimos na imagem
    public PrecoDetalhe Preco { get; set; } = new PrecoDetalhe();
}

// Esta classe representa o que está dentro de "preco"
public class PrecoDetalhe
{
    // Mapeia o valor "usd" (64203)
    public decimal Usd { get; set; }
}