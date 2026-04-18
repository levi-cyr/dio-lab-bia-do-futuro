# Base de Conhecimento

## Dados Utilizados

Descreva se usou os arquivos da pasta `data`, por exemplo:

| Arquivo | Formato | Utilização no Agente |
|---------|---------|---------------------|
| `historico_atendimento.csv` | CSV | Contextualizar interações anteriores para ter um atendimento continuo em diversas sessões |
| `perfil_investidor.json` | JSON | Personalizar recomendações |
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
using System;
using System.IO;
using System.Threading.Tasks;

namespace ChatbotCaldinhas
{
    class Loader
    {
        static async Task Main(string[] args)
        {
            //onde a pasta data vai estar
            string basePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "data");

            //vai ler os arquivos
            string perfilInvestidor = "";
            string transacoes = "";
            string historico = "";

            try
            {
                perfilInvestidor = await File.ReadAllTextAsync(Path.Combine(basePath, "perfil_investidor.json"));
                transacoes = await File.ReadAllTextAsync(Path.Combine(basePath, "transacoes.csv"));
                historico = await File.ReadAllTextAsync(Path.Combine(basePath, "historico_atendimento.csv"));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao ler os arquivos de contexto: {ex.Message}"); //se der ruim ele da erro e mostra qual seria
                return;
            }
        }
    }
}
```
[Os JSON/CSV são carregados no início da sessão e já ficam salvos na memória para uso do prompt]

### Como os dados são usados no prompt?
> Os dados vão no system prompt? São consultados dinamicamente?

> Os dados são consultados dinamicamente no prompt, abaixo tem o exemplo de como eles são mostrados (é ilustrativo, pois é preciso deserializar o Json e depois passar novamente, então ficaria mais ou menos igual aos dados iniciais do arquivo JSON)

Historico de atendimento (data/historico_atendimento.csv):
- Data: 2026-02-15
- Canal: Chat
- Tema: Gastos
- Resumo: Cliente perguntou sobre gastos do mês
- Resolvido: sim

Perfil do Investidor (data/perfil_investidor.json):
- Nome: Levi
- Idade: 27
- Profissao: Analista de Sistemas
- Renda mensal: R$ 4500.00
- Perfil investidor: Moderado
- Objetivo principal: Construir reserva de emergência
- Patrimonio_total: R$ 8000.00
- Reserva_emergencia_atual: R$ 5000.00
- Aceita risco: false

Transações (data/transacoes.csv):
- Data: 2026-02-05
- Descricao: Salario
- Categoria: receita
- Valor: R$ 3000.00
- Tipo: entrada

---
