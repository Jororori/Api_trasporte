namespace CapaEntidades
{
    public class LoginRequest
    {
        public string NombreUsuario { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public class LoginResponse
    {
        public bool Exitoso { get; set; }
        public string Mensaje { get; set; } = string.Empty;
        public string? Token { get; set; }
        public UsuarioDto? Usuario { get; set; }
    }

    public class UsuarioDto
    {
        public int Id { get; set; }
        public string NombreUsuario { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public int? IdTransportista { get; set; }
    }
}
