namespace API_TRANSPORTISTE.Configuration
{
    /// <summary>
    /// Clase que define los API Keys fijos y sus empresas asociadas
    /// Tú defines los tokens aquí manualmente
    /// </summary>
    public class ApiKeyConfig
    {
        /// <summary>
        /// Diccionario: Token -> IdEmpresa
        /// Cada token da acceso solo a una empresa
        /// </summary>
        public static readonly Dictionary<string, int> TokensValidos = new()
        {
            // Formato: { "TOKEN_AQUI", IdEmpresa }
            // Ejemplo: Token para empresa 1
            { "bearer_reset_soft_secreto_abc123def456", 1 },

            // Token para empresa 2
            { "8fAqwpFt6h9JTlDERFWDiegamES7cSpbxVRvBvfLiqP5M7sM0IzJyFjv6Q2jEkmD", 129 }, //EMP DE TRANSP EXPRESO LOS HUSARES SRLTDA

            // Token para empresa 3
            { "dj30dgkUwuX9Pyimh2q23LSFP9amiOQiTzad90zpOfes4hwf5dfWtFtUteSJc24c", 1441}, // EMPRESA DE TRANSPORTES TURISMO LOS HUSARES S.A.C.


            { "Ga0ARAz3tSo5S8EvjFD2fk6yMDkUobyUJQ1rjLkxUJVKipI5G2k7wYHO457tKUpH", 2505}, // EMPRESA DE TRANSPORTES TURISMO LOS HUSARES S.A.C.

        };

        /// <summary>
        /// Obtiene el IdEmpresa de un token
        /// Retorna -1 si el token no es válido
        /// </summary>
        public static int ObtenerIdEmpresa(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
                return -1;

            if (TokensValidos.TryGetValue(token, out var idEmpresa))
                return idEmpresa;

            return -1;
        }

        /// <summary>
        /// Verifica si un token es válido
        /// </summary>
        public static bool EsTokenValido(string token)
        {
            return !string.IsNullOrWhiteSpace(token) && TokensValidos.ContainsKey(token);
        }
    }
}
