#!/bin/bash

# Esperar o banco estar pronto
echo "Waiting for database..."
sleep 10

# Executar migrations com caminho explícito do projeto
echo "Running migrations..."
dotnet ef database update --project /app/PersonalTasksProject.csproj --verbose

# Iniciar a aplicação
echo "Starting application..."
dotnet PersonalTasksProject.dll