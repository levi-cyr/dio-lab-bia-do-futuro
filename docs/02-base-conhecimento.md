# Base de Conhecimento

## Dados Utilizados

Descreva se usou os arquivos da pasta `data`, por exemplo:

| Arquivo | Formato | Utilização no Agente |
|---------|---------|---------------------|
| `historico_atendimento.csv` | CSV | Contextualizar interações anteriores para ter um atendimento continuo em diversas sessões |
| `perfil_investidor.json` | JSON | Personalizar recomendações |
| `produtos_financeiros.json` | JSON | Sugerir produtos adequados ao perfil |
| `transacoes.csv` | CSV | Analisar padrão de gastos do cliente para dar as dicas de organização |

> [!TIP]
> **Quer um dataset mais robusto?** Você pode utilizar datasets públicos do [Hugging Face](https://huggingface.co/datasets) relacionados a finanças, desde que sejam adequados ao contexto do desafio.

---

## Adaptações nos Dados

> Você modificou ou expandiu os dados mockados? Descreva aqui.

Adaptei os dados ao meu uso modificando eles para dados que fiquem dentro do meu controle e familiaridade. Retirei as informações do CDB e tesouro selic para focar menos em investimentos e mais na organização pessoal. O perfil do investidor foi modificado para ficar um salário ajustado com o que alterei nas transações, que apenas fiz um ajuste de preços e datas.

---

## Estratégia de Integração

### Como os dados são carregados?
> Descreva como seu agente acessa a base de conhecimento.
```csharp
using CsvHelper; //biblioteca para ler os CSV (instale no terminal com o comando: dotnet add package CsvHelper)
using System.Globalization;
using System.Text.Json;

namespace Loader;

class Program
{
    static void Main(string[] args)
    {
        using var ReaderHistorico = new StreamReader("data/historico_atendimento.csv"); //passa e "lê" o arquivo csv e salva as informações
        using var CsvHistorico = new CsvReader(ReaderHistorico, CultureInfo.InvariantCulture); //com os dados gerados ele lê o arquivo respeitando o padrao universal

        var Historico = CsvHistorico.GetRecords<dynamic>().ToList(); //pega o historico_atendimentos.csv e adiciona em uma lista de forma dinamica (adicinoa enquanto tiver no arquivo)

        using var ReaderTransacoes = new StreamReader("data/transacoes.csv");
        using var CsvTransacoes = new CsvReader(ReaderTransacoes, CultureInfo.InvariantCulture);
        var Transacoes = CsvTransacoes.GetRecords<dynamic>().ToList();

        string JsonPerfil = File.ReadAllText("data/perfil_investidor.json"); //lê tudo que tá no arquivo json
        var Perfil = JsonSerializer.Deserialize<dynamic>(JsonPerfil); //deserializa tudo que a string JsonPerfil leu anteriormente e joga em uma nova variavel

        string JsonProdutos = File.ReadAllText("data/perfil_investidor.json");
        var produtos = JsonSerializer.Deserialize<dynamic>(JsonProdutos);

        Console.WriteLine("Dados carregados com sucesso!");
    }
}

```
[Os JSON/CSV são carregados no início da sessão e já ficão salvos na memória para uso do prompt]

### Como os dados são usados no prompt?
> Os dados vão no system prompt? São consultados dinamicamente?

[Sua descrição aqui]

---

## Exemplo de Contexto Montado

> Mostre um exemplo de como os dados são formatados para o agente.

```
Dados do Cliente:
- Nome: João Silva
- Perfil: Moderado
- Saldo disponível: R$ 5.000

Últimas transações:
- 01/11: Supermercado - R$ 450
- 03/11: Streaming - R$ 55
...
```
