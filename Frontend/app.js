// ==========================================
// CONFIGURAÇÃO DA URL DA API
// ==========================================
// IMPORTANTE: Enquanto estiver testando localmente, certifique-se de que a API
// está rodando no Visual Studio e que o túnel do ngrok está ativo.
const API_URL = "https://localhost:7124/api/precos"

// ==========================================
// FUNÇÃO PRINCIPAL: BUSCAR DADOS DO BACKEND
// ==========================================
async function buscarDados() {
  try {
    console.log("Iniciando requisição para:", API_URL)

    const response = await fetch(API_URL)

    // Se a resposta não for bem-sucedida (ex: 404, 500 ou erro de CORS), lança um erro
    if (!response.ok) {
      throw new Error(`Erro de rede: Código status ${response.status}`)
    }

    const dados = await response.json()

    // Exibe no console do navegador (F12) o array exato vindo do C# para inspeção
    console.log("Dados recebidos da API com sucesso:", dados)

    // Verifica se a API retornou registros
    if (dados && dados.length > 0) {
      // O primeiro elemento ([0]) é o registro mais recente devido à ordenação da API
      const maisRecente = dados[0]

      // MAPEAMENTO SEGURO: Tenta ler 'valorUsd' (CamelCase) ou 'ValorUsd'/'Usd' (PascalCase)
      const precoFinal =
        maisRecente.valorUsd ??
        maisRecente.valorUSD ??
        maisRecente.Usd ??
        maisRecente.valorUsd
      const dataFinal = maisRecente.dataRegistro ?? maisRecente.DataRegistro
      const moedaFinal = maisRecente.moeda ?? maisRecente.Moeda ?? "BTC"

      // 1. Atualiza o mostrador de preço principal
      if (precoFinal !== undefined && precoFinal !== null) {
        document.getElementById("btc-price").innerText =
          precoFinal.toLocaleString("en-US", {
            minimumFractionDigits: 2,
            maximumFractionDigits: 2,
          })
      } else {
        document.getElementById("btc-price").innerText = "---"
        console.warn(
          "Propriedade de preço não encontrada no objeto:",
          maisRecente,
        )
      }

      // 2. Atualiza a mensagem de última atualização
      if (dataFinal) {
        const dataFormatada = new Date(dataFinal).toLocaleString("pt-BR")
        document.getElementById("last-update").innerText = dataFormatada
      } else {
        document.getElementById("last-update").innerText = "Data indisponível"
      }

      // 3. Renderiza as linhas da tabela de histórico
      atualizarTabela(dados)
    } else {
      document.getElementById("btc-price").innerText = "Sem dados"
      document.getElementById("last-update").innerText =
        "O banco de dados está vazio."
      console.log("A API retornou sucesso, mas a tabela no banco está vazia.")
    }
  } catch (error) {
    // Captura falhas de conexão, URL errada ou bloqueio de CORS
    console.error("Erro detalhado capturado no fluxo:", error)
    document.getElementById("btc-price").innerText = "Erro"
    document.getElementById("last-update").innerText =
      "Verifique o console (F12) para detalhes técnicos."
  }
}

// ==========================================
// FUNÇÃO AUXILIAR: POPULAR A TABELA HTML
// ==========================================
function atualizarTabela(lista) {
  const tbody = document.getElementById("history-table-body")

  // Limpa qualquer linha antiga que estava na tabela
  tbody.innerHTML = ""

  // Percorre cada registro retornado pela API e cria uma nova linha (tr)
  lista.forEach((item) => {
    // Mapeamentos de segurança para evitar que a coluna renderize vazia ou como 'undefined'
    const idFinal = item.id ?? item.Id ?? "---"
    const moedaFinal = item.moeda ?? item.Moeda ?? "---"
    const precoFinal = item.valorUsd ?? item.valorUSD ?? item.Usd ?? 0
    const dataFinal = item.dataRegistro ?? item.DataRegistro

    const dataFormatada = dataFinal
      ? new Date(dataFinal).toLocaleString("pt-BR")
      : "---"

    const tr = document.createElement("tr")
    tr.innerHTML = `
            <td>${idFinal}</td>
            <td style="font-weight: 600; color: #818cf8;">${moedaFinal}</td>
            <td>U$ ${precoFinal.toLocaleString("en-US", { minimumFractionDigits: 2, maximumFractionDigits: 2 })}</td>
            <td>${dataFormatada}</td>
        `

    tbody.appendChild(tr)
  })
}

// ==========================================
// EVENTOS DE DISPARO DA PÁGINA
// ==========================================

// Vincula a ação de clique do botão "Atualizar Painel" à nossa função de busca
document.getElementById("refresh-btn").addEventListener("click", buscarDados)

// Executa a busca automaticamente assim que a estrutura da página terminar de carregar
window.addEventListener("DOMContentLoaded", buscarDados)
