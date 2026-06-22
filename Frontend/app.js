const API_URL = "https://api.martodev.online/api/precos"

let meuGrafico = null

function formatarDataLocal(dataIso) {
  if (!dataIso) return "---"
  try {
    const stringUtc = dataIso.endsWith("Z") ? dataIso : dataIso + "Z"
    const data = new Date(stringUtc)

    if (isNaN(data.getTime())) return dataIso

    return data.toLocaleString("pt-BR", {
      timeZone: "America/Sao_Paulo",
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

function renderizarGrafico(lista) {
  const ctx = document.getElementById("price-chart").getContext("2d")

  const dadosInvertidos = [...lista].reverse()

  const rotulosHoras = dadosInvertidos.map((item) => {
    const dataCrua = item.dataRegistro ?? item.DataRegistro
    const stringUtc = dataCrua.endsWith("Z") ? dataCrua : dataCrua + "Z"
    const d = new Date(stringUtc)

    return d.toLocaleTimeString("pt-BR", {
      timeZone: "America/Sao_Paulo",
      hour: "2-digit",
      minute: "2-digit",
      second: "2-digit",
    })
  })

  const valoresPrecos = dadosInvertidos.map(
    (item) => item.valorUsd ?? item.valorUSD ?? item.Usd ?? item.ValorUsd ?? 0,
  )

  if (meuGrafico) {
    meuGrafico.destroy()
  }

  meuGrafico = new Chart(ctx, {
    type: "line",
    data: {
      labels: rotulosHoras,
      datasets: [
        {
          label: "Preço do BTC (USD)",
          data: valoresPrecos,
          borderColor: "#818cf8",
          backgroundColor: "rgba(129, 140, 248, 0.1)",
          borderWidth: 3,
          tension: 0.3,
          pointBackgroundColor: "#34d399",
          pointBorderColor: "#fff",
          pointRadius: 4,
          fill: true,
        },
      ],
    },
    options: {
      responsive: true,
      maintainAspectRatio: false,
      layout: {
        padding: {
          left: 15,
          right: 15,
          top: 15,
          bottom: 15,
        },
      },
      plugins: {
        legend: { display: false },
      },
      scales: {
        x: {
          grid: { color: "rgba(255, 255, 255, 0.05)" },
          ticks: { color: "#94a3b8" },
        },
        y: {
          grid: { color: "rgba(255, 255, 255, 0.05)" },

          grace: "5%",
          ticks: {
            color: "#94a3b8",
            callback: function (value) {
              return "U$ " + value.toLocaleString("en-US")
            },
          },
        },
      },
    },
  })
}

async function buscarDados() {
  try {
    console.log(
      "Buscando dados atualizados da API em:",
      new Date().toLocaleTimeString("pt-BR"),
    )
    const response = await fetch(API_URL)

    if (!response.ok)
      throw new Error(`Erro de rede: Código status ${response.status}`)

    const dados = await response.json()
    console.log("Dados recebidos com sucesso:", dados)

    if (dados && dados.length > 0) {
      const maisRecente = dados[0]

      const precoFinal =
        maisRecente.valorUsd ??
        maisRecente.valorUSD ??
        maisRecente.Usd ??
        maisRecente.ValorUsd
      const dataFinal = maisRecente.dataRegistro ?? maisRecente.DataRegistro

      if (precoFinal !== undefined && precoFinal !== null) {
        document.getElementById("btc-price").innerText =
          precoFinal.toLocaleString("en-US", {
            minimumFractionDigits: 2,
            maximumFractionDigits: 2,
          })
      }

      if (dataFinal) {
        document.getElementById("last-update").innerText =
          formatarDataLocal(dataFinal)
      }

      atualizarTabela(dados)

      renderizarGrafico(dados)
    } else {
      document.getElementById("btc-price").innerText = "Sem dados"
      document.getElementById("last-update").innerText = "Banco vazio."
    }
  } catch (error) {
    console.error("Erro no fluxo de atualização:", error)
    document.getElementById("btc-price").innerText = "Erro"
  }
}

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

setInterval(buscarDados, 300000)
