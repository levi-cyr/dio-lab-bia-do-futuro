# Como usar

## Instalação do Ollama

```powershell
# 1. Instale o Ollama (https://ollama.com/)

# 2. Escolha um modelo que melhor se adapte para seu uso. Iremos usar o llama3.1
ollama pull llama3.1:8b

# 3. Faz o teste para verificar se está tudo certo (após usar o comando, escreva algo para testar)
ollama run llama3.1:8b 

```

## Código completo

O código está dentro da pasta `Chatbot (Caldinhas)`

## Como rodar
```powershell
# 1. Verifique se o ollama está rodando (se não estiver, use o comando "ollama" no powershell, aperte "ESC" para sair e teste novamente o código abaixo)
ollama serve

# 2. Rode o programa (atenção, aqui vai depender de seu compilador, utilizo o vscode. Fique de olho na pasta que você está para executar o programa)
dotnet run
```
