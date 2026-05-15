using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;
using API_TRANSPORTISTE.Configuration;

namespace API_TRANSPORTISTE.Authentication
{
    /// <summary>
    /// Handler personalizado para autenticación por API Key
    /// Valida que cada solicitud incluya un API Key válido en el header Authorization
    /// </summary>
    public class ApiKeyAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private const string ApiKeyHeaderName = "Authorization";

        public ApiKeyAuthenticationHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder)
            : base(options, logger, encoder)
        {
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            // Buscar el header Authorization
            if (!Request.Headers.TryGetValue(ApiKeyHeaderName, out var authorizationHeaderValue))
            {
                return Task.FromResult(AuthenticateResult.Fail("Authorization header no encontrado"));
            }

            var authorizationHeader = authorizationHeaderValue.ToString();

            // Extraer el token del header (formato: Bearer <token>)
            if (!authorizationHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
            {
                return Task.FromResult(AuthenticateResult.Fail("Authorization header debe comenzar con 'Bearer'"));
            }

            var apiKey = authorizationHeader.Substring("Bearer ".Length).Trim();

            if (string.IsNullOrEmpty(apiKey))
            {
                return Task.FromResult(AuthenticateResult.Fail("API Key vacío"));
            }

            // ✅ SIMPLIFICADO: Solo validar si el token está en la lista de ApiKeyConfig
            if (!ApiKeyConfig.EsTokenValido(apiKey))
            {
                return Task.FromResult(AuthenticateResult.Fail("API Key inválido"));
            }

            // Obtener el IdEmpresa asignado a este token
            var idEmpresa = ApiKeyConfig.ObtenerIdEmpresa(apiKey);

            // Crear claims con la información del token
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, apiKey),
                new Claim("apiKey", apiKey),
                new Claim("idEmpresa", idEmpresa.ToString()),
                new Claim("autenticacion_tipo", "ApiKey_Fijo")
            };

            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);

            return Task.FromResult(AuthenticateResult.Success(ticket));
        }
    }
}
