// ==========================================
// CONFIGURAÇÃO DA URL DA API
// ==========================================
const API_URL = "https://localhost:7124/api/precos"

// ==========================================
// FUNÇÃO AUXILIAR: EXIBIÇÃO DE DATA FORMATADA
// ==========================================
function formatarDataLocal(dataIso) {
  if (!dataIso) return "---"

  try {
    const data = new Date(dataIso)
    if (isNaN(data.getTime())) return dataIso

    // Retorna formatado lindamente no padrão brasileiro
    return data.toLocaleString("pt-BR", {
      day: "2-digit",
      month: "2-digit",
      year: "numeric",
      hour: "2-digit",
      minute: "2-digit",
      second: "2-digit",
    })
  } catch (e) {
    return dataIso
  }
}

// ==========================================
// FUNÇÃO PRINCIPAL: BUSCAR DADOS DO BACKEND
// ==========================================
async function buscarDados() {
  try {
    console.log("Iniciando requisição para:", API_URL)
    const response = await fetch(API_URL)

    if (!response.ok)
      throw new Error(`Erro de rede: Código status ${response.status}`)

    const dados = await response.json()
    console.log("Dados recebidos da API:", dados)

    if (dados && dados.length > 0) {
      const maisRecente = dados[0]

      const precoFinal =
        maisRecente.valorUsd ??
        maisRecente.valorUSD ??
        maisRecente.Usd ??
        maisRecente.ValorUsd
      const dataFinal = maisRecente.dataRegistro ?? maisRecente.DataRegistro

      // 1. Preço principal
      if (precoFinal !== undefined && precoFinal !== null) {
        document.getElementById("btc-price").innerText =
          precoFinal.toLocaleString("en-US", {
            minimumFractionDigits: 2,
            maximumFractionDigits: 2,
          })
      }

      // 2. Data de atualização
      if (dataFinal) {
        document.getElementById("last-update").innerText =
          formatarDataLocal(dataFinal)
      }

      // 3. Tabela
      atualizarTabela(dados)
    } else {
      document.getElementById("btc-price").innerText = "Sem dados"
      document.getElementById("last-update").innerText = "Banco vazio."
    }
  } catch (error) {
    console.error("Erro no fluxo:", error)
    document.getElementById("btc-price").innerText = "Erro"
  }
}

// ==========================================
// FUNÇÃO AUXILIAR: POPULAR A TABELA HTML
// ==========================================
function atualizarTabela(lista) {
  const tbody = document.getElementById("history-table-body")
  tbody.innerHTML = ""

  lista.forEach((item) => {
    const idFinal = item.id ?? item.Id ?? "---"
    const moedaFinal = item.moeda ?? item.Moeda ?? "---"
    const precoFinal =
      item.valorUsd ?? item.valorUSD ?? item.Usd ?? item.ValorUsd ?? 0
    const dataFinal = item.dataRegistro ?? item.DataRegistro

    const tr = document.createElement("tr")
    tr.innerHTML = `
            <td>${idFinal}</td>
            <td style="font-weight: 600; color: #818cf8;">${moedaFinal}</td>
            <td>U$ ${precoFinal.toLocaleString("en-US", { minimumFractionDigits: 2, maximumFractionDigits: 2 })}</td>
            <td>${formatarDataLocal(dataFinal)}</td>
        `
    tbody.appendChild(tr)
  })
}

document.getElementById("refresh-btn").addEventListener("click", buscarDados)
window.addEventListener("DOMContentLoaded", buscarDados)
