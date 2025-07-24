# Etapa de build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copia os arquivos de projeto para restaurar dependências
COPY ["RotaViagem.api/RotaViagem.api.csproj", "RotaViagem.api/"]
COPY ["RotaViagem.application/RotaViagem.application.csproj", "RotaViagem.application/"]
COPY ["RotaViagem.domain/RotaViagem.domain.csproj", "RotaViagem.domain/"]

# Restaura as dependências do projeto API
RUN dotnet restore "RotaViagem.api/RotaViagem.api.csproj"

# Copia todo o código
COPY . .

WORKDIR "/src/RotaViagem.api"

# Publica a aplicação em modo Release, sem app host para container leve
RUN dotnet publish "RotaViagem.api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Etapa final (runtime)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app

# Copia os arquivos publicados da etapa build
COPY --from=build /app/publish .

# Expõe a porta 80 para o container
EXPOSE 80

# Comando para iniciar a aplicação
ENTRYPOINT ["dotnet", "RotaViagem.api.dll"]

