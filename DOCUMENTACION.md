# Estructura de Arquitectura - API Transportista

## 📋 Descripción

Esta es una estructura básica de una API REST construida con .NET 10 siguiendo patrones de arquitectura en capas (N-Tier Architecture). El ejemplo implementa un GET simple a la base de datos para obtener transportistas.

## 🏗️ Estructura del Proyecto

```
API_TRANSPORTISTE/
├── API_TRANSPORTISTE/          # Capa de Presentación (API Web)
│   ├── Controllers/
│   │   └── TransportistaController.cs
│   ├── Program.cs              # Configuración e inyección de dependencias
│   ├── appsettings.json        # Configuración y connection strings
│   └── API_TRANSPORTISTE.csproj
│
├── CapaServicio/               # Capa de Lógica de Negocio
│   ├── Interfaces/
│   │   └── ITransportistaService.cs
│   ├── Servicios/
│   │   └── TransportistaService.cs
│   └── CapaServicio.csproj
│
├── CapaDatos/                  # Capa de Acceso a Datos
│   ├── Interfaces/
│   │   └── ITransportistaRepository.cs
│   ├── Repositorio/
│   │   └── TransportistaRepository.cs
│   └── CapaDatos.csproj
│
├── CapaEntidades/              # Capa de Modelos/Entidades
│   ├── Transportista.cs
│   └── CapaEntidades.csproj
│
└── BD/                         # Scripts de Base de Datos
    └── ScriptTransportistas.sql
```

## 🔄 Flujo de Datos

```
Cliente HTTP
    ↓
[TransportistaController] - Recibe la solicitud GET
    ↓
[TransportistaService] - Procesa lógica de negocio
    ↓
[TransportistaRepository] - Accede a la base de datos
    ↓
SQL Server (BDTransporte)
    ↓ Retorna datos
[Transportista Entity] - Mapea los datos
    ↓
JSON Response al Cliente
```

## 📚 Descripción de Capas

### 1. **Capa de Presentación** (API_TRANSPORTISTE)
- **Responsabilidad**: Exponer endpoints HTTP
- **Componentes**:
  - `TransportistaController`: Maneja las solicitudes HTTP GET
  - `Program.cs`: Configuración de servicios e inyección de dependencias

### 2. **Capa de Lógica de Negocio** (CapaServicio)
- **Responsabilidad**: Validar y procesar la lógica de negocio
- **Componentes**:
  - `ITransportistaService`: Contrato/interfaz del servicio
  - `TransportistaService`: Implementación de la lógica

### 3. **Capa de Acceso a Datos** (CapaDatos)
- **Responsabilidad**: Acceder a la base de datos
- **Componentes**:
  - `ITransportistaRepository`: Contrato/interfaz del repositorio
  - `TransportistaRepository`: Implementación con SQL directo

### 4. **Capa de Entidades** (CapaEntidades)
- **Responsabilidad**: Definir modelos de datos
- **Componentes**:
  - `Transportista`: Entidad que representa un transportista

## 🔌 Endpoints Disponibles

### Obtener todos los transportistas
```http
GET /api/transportista
```

**Respuesta exitosa (200 OK):**
```json
{
  "exito": true,
  "datos": [
    {
      "id": 1,
      "nombre": "Juan Pérez",
      "cedula": "12345678",
      "telefono": "3001234567",
      "email": "juan.perez@example.com",
      "activo": true,
      "fechaRegistro": "2024-01-15T10:30:00"
    }
  ],
  "cantidad": 1
}
```

### Obtener transportista por ID
```http
GET /api/transportista/{id}
```

**Respuesta exitosa (200 OK):**
```json
{
  "exito": true,
  "datos": {
    "id": 1,
    "nombre": "Juan Pérez",
    "cedula": "12345678",
    "telefono": "3001234567",
    "email": "juan.perez@example.com",
    "activo": true,
    "fechaRegistro": "2024-01-15T10:30:00"
  }
}
```

**Respuesta no encontrado (404 Not Found):**
```json
{
  "exito": false,
  "mensaje": "Transportista con ID 999 no encontrado"
}
```

## ⚙️ Configuración

### appsettings.json
Actualizar la cadena de conexión según tu base de datos:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=TU_SERVIDOR;Database=BDTransporte;User Id=sa;Password=TU_PASSWORD;TrustServerCertificate=True;"
  }
}
```

### Crear la tabla en SQL Server
Ejecutar el script `BD/ScriptTransportistas.sql` en SQL Server Management Studio:

```sql
-- El script crea:
-- 1. Tabla Transportistas con campos necesarios
-- 2. Índices para optimizar búsquedas
-- 3. Inserta datos de prueba
```

## 🚀 Cómo ejecutar

1. **Restaurar base de datos**:
   - Ejecutar el script SQL en tu servidor

2. **Actualizar connection string**:
   - Modificar `appsettings.json` con tus datos de conexión

3. **Compilar la solución**:
   ```bash
   dotnet build
   ```

4. **Ejecutar la API**:
   ```bash
   dotnet run --project API_TRANSPORTISTE
   ```

5. **Probar los endpoints**:
   - Swagger: `https://localhost:5001/swagger`
   - Postman: Importar los endpoints GET

## 📦 Dependencias

- .NET 10
- System.Data.SqlClient 4.8.6
- Microsoft.AspNetCore.OpenApi 10.0.7

## 🔒 Principios SOLID Aplicados

✅ **S - Single Responsibility**: Cada clase tiene una única responsabilidad
✅ **O - Open/Closed**: Abierto para extensión, cerrado para modificación
✅ **L - Liskov Substitution**: Interfaces implementadas correctamente
✅ **I - Interface Segregation**: Interfaces específicas y no genéricas
✅ **D - Dependency Inversion**: Inversión de control mediante DI

## 🔄 Inyección de Dependencias

El `Program.cs` configura automáticamente todas las dependencias:

```csharp
// Registrar repositorios
builder.Services.AddScoped<ITransportistaRepository>(sp => 
    new TransportistaRepository(connectionString));

// Registrar servicios
builder.Services.AddScoped<ITransportistaService, TransportistaService>();
```

## 📝 Próximos Pasos

Para expandir esta estructura, puedes:

1. **Agregar más operaciones**: POST (Crear), PUT (Actualizar), DELETE (Eliminar)
2. **Implementar validaciones**: Usar FluentValidation
3. **Agregar autenticación**: JWT o Identity
4. **Implementar logging**: Serilog
5. **Agregar caché**: Redis
6. **Usar Entity Framework Core**: Para ORM
7. **Crear especificaciones**: Patrones query objects
8. **Agregar tests unitarios**: xUnit o NUnit

## 📧 Soporte

Para preguntas o mejoras, revisa los archivos fuente en cada capa.

---

**Creado con**: .NET 10 | Visual Studio 2026 | SQL Server
**Patrón**: Arquitectura en Capas (N-Tier)
