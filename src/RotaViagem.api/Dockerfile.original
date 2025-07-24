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