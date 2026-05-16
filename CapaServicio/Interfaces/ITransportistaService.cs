using CapaEntidades;

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

        Task<List<TipoAsiento>> ObtenerTiposAsiento();
        Task<List<DetalleProgramacion>> ObtenerAsientosPor(int id);
        Task<bool> BloquearAsientoPor(int idDetalleProgramacion);
    }
}
