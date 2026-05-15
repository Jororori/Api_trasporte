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
            { "UygJULtqrzputY5yyODpSVGrKluO0wb7BpqoFsLeYbPyZYmG2SXDSBOGkGmSGCm2", 129 },

            // Token para empresa 3
            { "UygJULtqrzputY5yyODpSVGrKluO0wb7BpqoFsLeYbPyZYmG2SXDSBOGkGmSGCm2", 3 },
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
