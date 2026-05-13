@echo off
REM Script para ejecutar la API (Windows)

echo.
echo ╔════════════════════════════════════════════════════════════════╗
echo ║         API TRANSPORTISTA - Script de Ejecución               ║
echo ╚════════════════════════════════════════════════════════════════╝
echo.

echo 1. Compilando solucion...
dotnet build

if %ERRORLEVEL% neq 0 (
    echo.
    echo ❌ Error en la compilacion
    exit /b 1
)

echo.
echo ✅ Compilacion exitosa
echo.

echo 2. Ejecutando API en puerto 5001...
dotnet run --project API_TRANSPORTISTE

echo.
echo ╔════════════════════════════════════════════════════════════════╗
echo ║  API ejecutandose en: https://localhost:5001                  ║
echo ║  Swagger UI: https://localhost:5001/swagger                   ║
echo ║  GET /api/transportista - Obtener todos                       ║
echo ║  GET /api/transportista/{id} - Obtener por ID                 ║
echo ╚════════════════════════════════════════════════════════════════╝
echo.

pause
