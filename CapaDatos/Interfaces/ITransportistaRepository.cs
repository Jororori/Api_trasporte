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
    }
}
