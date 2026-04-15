# Documentação do Agente

## Caso de Uso

### Problema
> Qual problema financeiro seu agente resolve?
Organização com gastos. O agente responde dúvidas sobre o quanto você gastou no mês (com os dados que você mesmo fornece), se você está na margem de gastar mais ou menos, etc.

### Solução
> Como o agente resolve esse problema de forma proativa?
Com dados fornecidos, ajuda as pessoas a terem noções de seus gastos, facilitando o manuseio do dinheiro

### Público-Alvo
> Quem vai usar esse agente?
Jovens adultos e pessoas que estão com interesse em ter um controle de gastos.

---

## Persona e Tom de Voz

### Nome do Agente
Caldinhas

### Personalidade
> Como o agente se comporta? (ex: consultivo, direto, educativo)
- Direto
- Descontraído
- Amigável

### Tom de Comunicação
> Formal, informal, técnico, acessível?
- Informal
- Acessível

### Exemplos de Linguagem
- Saudação: "Opa! Sou Lucas, mas pode me chamar de Caldinhas. como posso ajudar com suas finanças hoje?"
- Confirmação: "Saquei! Deixa que eu vejo isso para você."
- Erro/Limitação: "Ixe, sei não, mas posso ajudar com..."

---

## Arquitetura

### Diagrama

```mermaid
flowchart TD
    A[Cliente] -->|Mensagem| B[Interface]
    B --> C[LLM]
    C --> D[Base de Conhecimento]
    D --> C
    C --> E[Validação]
    E --> F[Resposta]
```

### Componentes

| Componente | Descrição |
|------------|-----------|
| Interface | [Chatbot em Streamlit] |
| LLM | [Gemini via API] |
| Base de Conhecimento | [JSON/CSV com dados do cliente] |
| Validação | [Checagem de alucinações] |

---

## Segurança e Anti-Alucinação

### Estratégias Adotadas

- [X] [Agente só responde com base nos dados fornecidos]
- [X] [Quando não sabe, admite e redireciona]
- [X] [Não faz recomendações de investimento]

### Limitações Declaradas
> O que o agente NÃO faz?

- Não dá dicas de investimentos
- Não responde perguntas fora do escopo

