# 🔐 Guía de Autenticación Bearer Token

## 📋 ¿Cómo funciona?

Tu API ahora requiere un **Bearer Token** (JWT) para acceder a los endpoints protegidos. El flujo es:

1. **Cliente hace login** → POST `/api/auth/login` con credenciales
2. **Servidor genera token JWT** → Válido por 24 horas
3. **Cliente incluye token en headers** → `Authorization: Bearer <token>`
4. **Servidor valida y procesa la solicitud**

---

## 🚀 PASO 1: Obtener el Token (Login)

### Endpoint
```
POST /api/auth/login
Content-Type: application/json
```

### Request Body
```json
{
  "nombreUsuario": "admin",
  "password": "admin123"
}
```

### Response (Exitoso)
```json
{
  "exitoso": true,
  "mensaje": "Login exitoso",
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "usuario": {
    "id": 1,
    "nombreUsuario": "admin",
    "email": "admin@transporte.com",
    "idTransportista": null
  }
}
```

### Response (Error)
```json
{
  "exitoso": false,
  "mensaje": "Usuario o contraseña incorrectos"
}
```

---

## 🔑 PASO 2: Usar el Token en Solicitudes

Ahora incluye el token en el header `Authorization` de cada solicitud:

### Ejemplo: Obtener ciudades
```
GET /api/transportista/1/ciudades
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

### Con cURL
```bash
curl -X GET "https://localhost:5001/api/transportista/1/ciudades" \
  -H "Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
```

### Con Postman
1. Ve a la pestaña **Authorization**
2. Selecciona **Type: Bearer Token**
3. Pega el token en el campo **Token**
4. Envía la solicitud

---

## 📝 Usuarios de Prueba (Hardcodeados por ahora)

```
Usuario: admin
Password: admin123
ID: 1
Acceso: Admin (sin transportista específico)

Usuario: transportista1
Password: pass123
ID: 2
Acceso: Transportista con ID 1
```

---

## ⚙️ Configuración JWT (appsettings.json)

```json
{
  "Jwt": {
    "SecretKey": "TuClaveSecretaMuyLargaDeAlMenos32CaracteresParaSeguridadMaxima!",
    "Issuer": "TuAppTransporte",
    "Audience": "TransporteUsuarios",
    "ExpirationHours": "24"
  }
}
```

### Parámetros:
- **SecretKey**: Clave para firmar tokens (IMPORTANTE: usa una segura en producción)
- **Issuer**: Quién emite el token
- **Audience**: A quién va dirigido
- **ExpirationHours**: Tiempo de validez en horas

---

## 🔄 PRÓXIMOS PASOS (Recomendaciones)

### 1. ✅ Mover usuarios a Base de Datos
En `AuthController.cs`, reemplaza el hardcoding:

```csharp
// En lugar de:
var usuariosValidos = new List<...> { ... };

// Consulta la BD:
var usuario = await _usuarioRepository.ObtenerPorNombreAsync(loginRequest.NombreUsuario);
if (usuario != null && _authService.VerificarPassword(loginRequest.Password, usuario.PasswordHash))
{
    // Generar token
}
```

### 2. 🔒 Usar contraseñas hasheadas
```csharp
// Al registrar un usuario:
usuario.PasswordHash = _authService.HashearPassword(password);
await _repository.GuardarAsync(usuario);
```

### 3. 📋 Proteger endpoints específicos
```csharp
// Solo usuarios con rol "Admin":
[Authorize(Roles = "Admin")]
public async Task<IActionResult> EliminarTransportista(int id) { ... }

// Solo el transportista autenticado puede acceder a sus datos:
[Authorize]
[HttpGet("{id}")]
public async Task<IActionResult> ObtenerMisDatos(int id)
{
    var usuarioId = int.Parse(User.FindFirst("sub").Value);
    if (usuarioId != id) return Forbid();
    // ...
}
```

### 4. 🛡️ Mejorar appsettings para producción
```json
{
  "Jwt": {
    "SecretKey": "${JWT_SECRET_KEY}",  // Variable de entorno
    "ExpirationHours": "8"             // Menos tiempo en producción
  }
}
```

---

## 🧪 Pruebas con Postman

### Colección de ejemplo:

**1. Login**
```
POST http://localhost:5001/api/auth/login
{
  "nombreUsuario": "admin",
  "password": "admin123"
}
```
Guarda el token de la respuesta en una variable: `{{token}}`

**2. Obtener datos protegidos**
```
GET http://localhost:5001/api/transportista/1/ciudades
Authorization: Bearer {{token}}
```

---

## ❌ Errores Comunes

| Error | Causa | Solución |
|-------|-------|----------|
| 401 Unauthorized | Token no proporcionado o inválido | Verifica el header Authorization |
| 403 Forbidden | Token válido pero sin permisos | Verifica los roles/claims del token |
| 500 Internal Server Error | JWT:SecretKey no configurada | Revisa appsettings.json |

---

## 🔐 Seguridad (Importante)

✅ **Hacer:**
- Usar HTTPS en producción
- Guardar SecretKey en variables de entorno
- Usar contraseñas hasheadas (SHA256 o bcrypt)
- Implementar rate limiting en `/api/auth/login`
- Renovar tokens periódicamente

❌ **NO hacer:**
- Poner SecretKey en el código fuente
- Enviar contraseñas en texto plano
- Usar tokens con expiración muy larga
- Almacenar tokens en localStorage (vulnerabilidad XSS)

---

## 📚 Referencias

- Microsoft JWT Documentation: https://docs.microsoft.com/en-us/dotnet/api/system.identitymodel.tokens.jwt
- JWT.io: https://jwt.io/
- Bearer Token RFC 6750: https://tools.ietf.org/html/rfc6750
