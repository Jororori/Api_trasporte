#!/bin/bash
# Script para ejecutar la API (para sistemas Unix/Linux/Mac)

echo "╔════════════════════════════════════════════════════════════════╗"
echo "║         API TRANSPORTISTA - Script de Ejecución               ║"
echo "╚════════════════════════════════════════════════════════════════╝"
echo ""

echo "1. Compilando solución..."
dotnet build

if [ $? -ne 0 ]; then
    echo "❌ Error en la compilación"
    exit 1
fi

echo "✅ Compilación exitosa"
echo ""

echo "2. Ejecutando API en puerto 5001..."
dotnet run --project API_TRANSPORTISTE

echo ""
echo "╔════════════════════════════════════════════════════════════════╗"
echo "║  API ejecutándose en: https://localhost:5001                  ║"
echo "║  Swagger UI: https://localhost:5001/swagger                   ║"
echo "║  GET /api/transportista - Obtener todos                       ║"
echo "║  GET /api/transportista/{id} - Obtener por ID                 ║"
echo "╚════════════════════════════════════════════════════════════════╝"
