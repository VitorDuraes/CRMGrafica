# Usar imagem oficial do .NET SDK para build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src

# Copiar tudo do frontend para dentro do container
COPY . ./

# Restaurar dependências e publicar
RUN dotnet publish CRMGraficaBlazor.csproj -c Release -o /app/publish

# Imagem runtime menor para rodar a aplicação Blazor Server
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Copiar os arquivos publicados da build
COPY --from=build /app/publish .

# Usar porta fornecida pela Render
ENV ASPNETCORE_URLS=http://+:$PORT

# Expõe a porta 5000 (ou ajuste conforme sua app)
EXPOSE 5000

# Comando para iniciar a aplicação
ENTRYPOINT ["dotnet", "CRMGraficaBlazor.dll"]
