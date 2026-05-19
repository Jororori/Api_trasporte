using CapaEntidades;

namespace CapaDatos.Interfaces
{
    
    public interface ITransportistaRepository
    {
        Task<List<Transportista>> ObtenerTodosAsync();
        Task<Transportista?> ObtenerPorIdAsync(int id);
        Task<List<Ciudades>> ObtenerCuidadesPor(int id);
        Task<List<Rutas>> ObtenerRutasPor(int id);
        Task<List<Buses>> ObtenerBusesPor(int id);
        Task<List<Programaciones>> ObtenerProgramacionPor(int Id,DateTime Fecha, int IdOrigen, int IdDestino);
        Task<List<TipoAsiento>> ObtenerTiposAsiento();
        Task<List<DetalleProgramacion>> ObtenerAsientosPor(int id);
        Task<string> BloquearAsientoPor(int idDetalleProgramacion);
        Task<bool> LimpiarBloqueoAsientos();
        Task<bool> LiberarAsientoPorToken(string token);
        Task<int> CrearReserva(int TipoDocumento, string NroDocumento, string Pasajero, DateTime? FechaNacimiento, int Edad, string Sexo, string Ruc, string RazonSocial, string Direccion, int TipoDocVenta, DateTime? FechaEmision, int IdAgenciaOrigen, int IdAgenciaDestino, string FormaDePago, string MedioPago, string Tarjeta, DateTime? FechaVencimiento, double Adelanto, string Observaciones, int IdUsuario, int Estado, int IdDocumento, int IdDetalleProgramacion, string precio, string PrecioLetra, string PrecioReprog , string HoraSalida , string Menor , int Embarque , string Telefono);

    }
}
