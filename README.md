# RotaViagem
# Simulador de Rotas de Voo - API .NET 9

## Descrição do Projeto

Este projeto é uma API RESTful desenvolvida em .NET 9 que permite gerenciar rotas de voo entre aeroportos. A aplicação oferece funcionalidades completas de CRUD para rotas, persistência dos dados em arquivos JSON e cálculo da rota mais barata entre dois aeroportos.

A arquitetura segue os princípios da Clean Architecture, garantindo separação clara entre domínio, aplicação e interface, além de facilitar manutenção, testes e escalabilidade.

---

## Funcionalidades Principais

- CRUD completo para rotas de voo (criar, ler, atualizar, deletar).
- Persistência dos dados em arquivo JSON.
- Cálculo da rota mais barata entre dois aeroportos via algoritmo de Dijkstra.
- Validação robusta dos dados usando FluentValidation.
- Uso de DTOs para transferência de dados entre API e domínio.
- Documentação automática via Swagger.
- Testes unitários cobrindo serviços, controllers e mapeamentos.

---

## Tecnologias Utilizadas

- .NET 9
- C#
- FluentValidation
- Swagger (OpenAPI)
- Moq e xUnit para testes unitários
- Docker (opcional para containerização)

---

## Como Rodar o Projeto

### Pré-requisitos

- [.NET 9 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/9.0)
- [Docker](https://www.docker.com/get-started) (opcional, recomendado)

---

### Rodando localmente

1. Clone o repositório:

```bash
git clone https://github.com/seu-usuario/seu-repositorio.git
cd seu-repositorio
```

### Rodando com Docker (Recomendado)

Para facilitar o deploy e garantir que o avaliador consiga rodar a aplicação sem problemas de ambiente, recomendamos usar Docker.

#### Passos para rodar a aplicação via Docker

1. **Certifique-se que o Docker está instalado e rodando.**

2. **No diretório raiz do projeto, crie um arquivo `Dockerfile` com o seguinte conteúdo:**

```dockerfile
# Use imagem oficial do .NET 9 SDK para build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

# Copia csproj e restaura dependências
COPY *.sln .
COPY WebAPI/WebAPI.csproj ./WebAPI/
RUN dotnet restore

# Copia todo o código e publica
COPY . .
RUN dotnet publish WebAPI/WebAPI.csproj -c Release -o out

# Imagem runtime para rodar a aplicação
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app/out .

# Exponha a porta padrão
EXPOSE 80

# Comando para rodar a aplicação
ENTRYPOINT ["dotnet", "WebAPI.dll"]

```
3. **Construa a imagem Docker:**

```bash
docker build -t simulador-rotas-voo .

```
4. **Execute o container:**

```bash
docker run -d -p 5000:80 --name simulador-rotas-voo simulador-rotas-voo

```
5. **Acesse a API via navegador:**

http://localhost:5000/swagger

---

### Comandos úteis para o avaliador

- ***Parar o container:***

```bash
docker stop simulador-rotas-voo
```

```bash
docker run -d -p 5000:80 --name simulador-rotas-voo simulador-rotas-voo

```
- ***Remover o container***

```bash
docker rm simulador-rotas-voo

```
- ***Ver logs do container***

```bash
docker logs simulador-rotas-voo

```

### Dicas

- Se a porta 5000 já estiver em uso, o avaliador pode mudar o mapeamento da porta no comando `docker run`, por exemplo:  
  `-p 8080:80`

- Certifique-se que o Docker está rodando e que o usuário tem permissão para executar comandos Docker.

## Requisitos e criação do script para deploy local

Para facilitar o deploy local da aplicação usando Docker, é recomendado criar um script que automatize os comandos necessários para construir a imagem, parar e remover containers antigos, e executar o container atualizado.

---

### Requisitos para rodar o script de deploy local

- **Docker instalado e em execução:**  
  O Docker deve estar instalado na máquina e o serviço deve estar ativo para que os comandos funcionem corretamente.

- **Permissões adequadas:**  
  O usuário que executa o script deve ter permissão para rodar comandos Docker (geralmente, ser membro do grupo `docker` no Linux ou ter privilégios administrativos no Windows).

- **Arquivos do projeto na raiz:**  
  O script deve estar localizado na raiz do projeto, onde também está o `Dockerfile` e o arquivo de solução `.sln`.

---

### Criação do script para deploy local no Windows (`deploy.bat`)

1. Na raiz do projeto, crie um arquivo chamado `deploy.bat`.

2. Insira o seguinte conteúdo no arquivo:

```bat
@echo off
docker build -t simulador-rotas-voo .
docker stop simulador-rotas-voo
docker rm simulador-rotas-voo
docker run -d -p 5000:80 --name simulador-rotas-voo simulador-rotas-voo
echo Aplicação rodando em http://localhost:5000/swagger
pause

```
### Explicação dos comandos do script

- `docker build -t simulador-rotas-voo .`  
  Constrói a imagem Docker com a tag `simulador-rotas-voo` a partir do `Dockerfile` localizado na pasta atual.

- `docker stop simulador-rotas-voo`  
  Para o container em execução com o nome `simulador-rotas-voo`, caso ele exista.

- `docker rm simulador-rotas-voo`  
  Remove o container parado para evitar conflitos ao criar um novo container com o mesmo nome.

- `docker run -d -p 5000:80 --name simulador-rotas-voo simulador-rotas-voo`  
  Executa o container em modo destacado (`-d`), mapeando a porta 80 do container para a porta 5000 da máquina local, e nomeia o container como `simulador-rotas-voo`.

- `echo Aplicação rodando em http://localhost:5000/swagger`  
  Exibe uma mensagem informando onde a aplicação pode ser acessada.

- `pause`  
  Mantém a janela do prompt aberta para que o usuário possa visualizar a mensagem antes de fechar.