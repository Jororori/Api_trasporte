using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace API_TRANSPORTISTE.Authentication
{
    /// <summary>
    /// Middleware para manejar errores de autenticación y retornar respuestas JSON personalizadas
    /// </summary>
    public class AuthenticationErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public AuthenticationErrorHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Llamar al siguiente middleware
            await _next(context);

            // Si la respuesta es 401 Unauthorized, retornar JSON personalizado
            if (context.Response.StatusCode == StatusCodes.Status401Unauthorized)
            {
                context.Response.ContentType = "application/json";

                var response = new
                {
                    status = 401,
                    success = false,
                    message = "Credenciales de API inválidas.",
                    error_code = "AUTH_CREDENTIALS_INVALID"
                };

                await context.Response.WriteAsJsonAsync(response);
            }
            // Si la respuesta es 403 Forbidden
            else if (context.Response.StatusCode == StatusCodes.Status403Forbidden)
            {
                context.Response.ContentType = "application/json";

                var response = new
                {
                    status = 403,
                    success = false,
                    message = "No tienes permiso para acceder a este recurso.",
                    error_code = "AUTH_FORBIDDEN"
                };

                await context.Response.WriteAsJsonAsync(response);
            }
        }
    }
}
