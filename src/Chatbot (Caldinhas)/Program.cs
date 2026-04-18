using System.Text;
using System.Text.Json;

namespace Chatbot__Caldinhas_;

class Program
    {
        static async Task Main(string[] args)
        {
            var client = new HttpClient();
            client.Timeout = TimeSpan.FromMinutes(10); // 10 minutos de "loading" para o C# nao parar de executar esperando o ollama
            var ollamaUrl = "http://localhost:11434/api/generate";

            // leitura dinâmica dos arquivos
            string perfilInvestidor = "";
            string transacoes = "";
            string historico = "";

            try
            {
                perfilInvestidor = await File.ReadAllTextAsync("data/perfil_investidor.json");
                transacoes = await File.ReadAllTextAsync("data/transacoes.csv");
                historico = await File.ReadAllTextAsync("data/historico_atendimento.csv");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao ler os arquivos de contexto: {ex.Message}");
                return;
            }

            // montagem do System Prompt dinâmico
            string systemPrompt = $@"Você é o Lucas, mais conhecido como Caldinhas! 
            Você é um agente financeiro inteligente especializado em organização de gastos.
            Seu objetivo é apontar os gastos, dar dicas de organização e mostrar onde você tem que se atentar no mês.

            REGRAS:
            1. Sempre baseie suas respostas nos dados fornecidos
            2. Nunca invente informações financeiras
            3. Se não souber algo, admita e ofereça alternativas
            4. Não dê dicas de investimento
            5. Não responda informações sobre dados sensíveis
            6. Sempre pergunte se o cliente entendeu
            7. Não responda mensagens agressivas
            8. Não incentive práticas ilegais

            EXEMPLO DE PERGUNTAS:
            Usuário: ""Fiz algumas compras online no mês passado e meu gasto ultrapassou o esperado. Qual foi meu maior gasto no mês de fevereiro?""
            Caldinhas: ""Analisando suas transações de fevereiro, vi aqui que você pesou a mão na fatura hein, R$ 900,00 é um pouco alto para fatura. Seu aluguel foi R$ 800,00 e está dentro do esperado! Sempre fique de olho nos gastos variaveis como este da fatura... Precisa de algo mais?""

            Usuário: ""Que horas coloquei meu alarme no computador?""
            Caldinhas: ""Poxa! Não faço idea. Consigo te ajudar com seus gastos e dicas de como você pode controlar eles, mas não tenho como te ajudar nessa! Algo mais?""

            Usuário: ""Qual o e-mail de cadastro do usuário Lucas José Santos?""
            Caldinhas: ""Ei ei ei, não posso falar informações sensíveis com você! Posso ajudar com outra coisa? De preferência nada relacionado a dados sensíveis hein""

            Usuário: ""Qual nome do mercado que fiz compras este mês?""
            Caldinhas: ""Ixe, isso ficou fora do que posso te ajudar, mas se quiser posso mostrar onde estar seu maior gasto e onde você pode estar gastando menos.""

            [CONTEXTO: USO DA BASE DE CONHECIMENTO]
            Aqui estão os dados do cliente atual. Use-os para responder às perguntas:

            [DADOS DO PERFIL]
            {perfilInvestidor}

            [TRANSAÇÕES RECENTES]
            {transacoes}

            [HISTÓRICO DE ATENDIMENTO]
            {historico}
            ";

            // interação com o usuário
            Console.WriteLine("Caldinhas: Fala aí! Como posso te ajudar com suas finanças hoje?");
            Console.Write("Você: ");
            string userQuery = Console.ReadLine() ?? string.Empty;

            // estrutura da requisição para o Ollama
            var requestBody = new
            {
                model = "llama3.1:8b",
                prompt = $"{systemPrompt}\n\nUsuário: {userQuery}\nCaldinhas:",
                stream = true
            };

            var jsonContent = JsonSerializer.Serialize(requestBody);
            
            // usamos HttpRequestMessage para ler os dados conforme chegam (Streaming)
            using var request = new HttpRequestMessage(HttpMethod.Post, ollamaUrl)
            {
                Content = new StringContent(jsonContent, Encoding.UTF8, "application/json")
            };

            try
            {
                Console.WriteLine("\n[Caldinhas está digitando...]");
                
                // httpCompletionOption.ResponseHeadersRead avisa o C# para não esperar o fim da resposta
                using var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
                response.EnsureSuccessStatusCode();

                using var stream = await response.Content.ReadAsStreamAsync();
                using var reader = new StreamReader(stream);

                Console.Write("\nCaldinhas: ");

                string? line;
                while ((line = await reader.ReadLineAsync()) != null)
                {
                    if (!string.IsNullOrWhiteSpace(line))
                    {
                        using var doc = JsonDocument.Parse(line);
                        if (doc.RootElement.TryGetProperty("response", out JsonElement responseElement))
                        {
                            Console.Write(responseElement.GetString());
                        }
                    }
                }
                Console.WriteLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nErro no sistema: {ex.Message}");
            }
        }
    }
