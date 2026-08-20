using CapaEntidades;
using System.Data;

namespace CapaServicio.Interfaces
{
    public interface ITransportistaService
    {
        Task<List<Transportista>> ObtenerTodos();
        Task<Transportista?> ObtenerPorId(int id);
        Task<List<Ciudades>> ObtenerCuidadesPor(int id);
        Task<List<Rutas>> ObtenerRutasPor(int id);
        Task<List<Buses>> ObtenerBusesPor(int id);
        Task<List<Programaciones>> ObtenerProgramacionPor(int Id, DateTime Fecha, int IdOrigen, int IdDestino);
        Task<List<Programaciones>> ObtenerProgramacionPorRuta(int Id, DateTime Fecha, int IdRuta);
        Task<List<TipoAsiento>> ObtenerTiposAsiento();
        Task<List<DetalleProgramacion>> ObtenerAsientosPor(int id);
        Task<string> BloquearAsientoPor(int idDetalleProgramacion, int? Tiempo);
        Task<bool> LimpiarBloqueoAsientos();
        Task<bool> LiberarAsientoPorToken(string token);
        Task<bool> LiberarAsientoPorId(int IdDetalle);
        Task<int> CrearReserva(int TipoDocumento, string NroDocumento, string Pasajero, DateTime? FechaNacimiento, int Edad , string Sexo, string Ruc , string RazonSocial , string Direccion, int TipoDocVenta, DateTime? FechaEmision, int IdAgenciaOrigen, int IdAgenciaDestino, string FormaDePago, string MedioPago, string Tarjeta, DateTime? FechaVencimiento, double Adelanto, string Observaciones, int IdUsuario, int Estado , int IdDocumento, int IdDetalleProgramacion, string precio, string PrecioLetra, string PrecioReprog , string HoraSalida , string Menor, int Embarque, string Telefono);
        Task<Login> VerUsuarioPor(int IdEmpresa);
        Task<string> ObtenerSeriePor(int TipoDocumento, int IdEmpresa, int IdEstablecimiento);
        Task<DetalleDocVenta> ObtenerProductoPor(int IdEmpresa);
        Task<DateTime> ExtenderReserva(string token, int tiempo);
        Task<DateTime> ExtenderReservaPorId( int IdDetalle, int tiempo);
        Task<(string, DocumentoElectronicoResponse)> VerEstadoReserva(int IdDetalleProgramacion);
        Task<bool> ReprogramarPasajePor(int IdDetalleProgramacion, string Tipo, int IdEmpresa, int IdUsuario);
        Task<int> ObtenerIdDocVentaPor(int IdDetalleProgramacion);
        Task<(int, int)> BuscarDocumentoEnviadoOSE(int IdDocVenta);
        Task<bool> AnularDocumento(int IdDocVenta, int IdUsuario);
        Task<bool> LimpiarPor(int IdDetalleProgramacion);
        Task<DocumentoVenta> ObtenerDatosDocVentaPor(int IdDocVenta);
        Task<int> VerIdDetalle(int IdProgramacion, int Asiento);
        Task<int> GuardarDetalles(int IdProgramacion, string Asientos);
        Task<List<(int, int)>> VerAsientosPorBoleto(int numeroboleto);
        Task<int> GuardarDataPosponer(string jsonData, int numeroBoleto);
        Task<decimal> ObtenerMontoAnteriorPor(int IdDetalleProgramacion);
        Task<decimal> ObtenerMontosPor(int IdProgramacion, int Asiento, int IdDestino);
        Task<(int, string)> ObetenerDatosReservaPorId(int IdReserva);
        Task<(string, string)> ObtenerPlacaHora(int IdProgramacion, int PuntoEmbarque);
    }
}
