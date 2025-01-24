# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar apenas os arquivos de projeto primeiro para aproveitar o cache das dependências
COPY ["*.csproj", "./"]
RUN dotnet restore

# Copiar o resto do código fonte
COPY . .
RUN dotnet build -c Release -o /app/build
RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Criar um usuário não-root para segurança
RUN adduser -u 5678 --disabled-password --gecos "" appuser && chown -R appuser /app
USER appuser

# Copiar apenas os arquivos publicados do stage de build
COPY --from=build /app/publish .

# Configurar a porta e o ambiente
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "PersonalTasksProject.dll"]