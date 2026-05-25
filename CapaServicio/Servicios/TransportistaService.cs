using CapaEntidades;
using CapaDatos.Interfaces;
using CapaServicio.Interfaces;

namespace CapaServicio.Servicios
{
    public class TransportistaService : ITransportistaService
    {
        private readonly ITransportistaRepository _repository;

        public TransportistaService(ITransportistaRepository repository)
        {
            _repository = repository;

        }

        public async Task<List<Transportista>> ObtenerTodos()
        {
            try
            {
                var transportistas = await _repository.ObtenerTodosAsync();
                return transportistas;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error en servicio al obtener transportistas: {ex.Message}", ex);
            }
        }

        public async Task<Transportista?> ObtenerPorId(int id)
        {
            try
            {
                if (id <= 0)
                    throw new ArgumentException("El id debe ser mayor a 0");

                var transportista = await _repository.ObtenerPorIdAsync(id);
                return transportista;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error en servicio al obtener transportista: {ex.Message}", ex);
            }
        }

        public async Task<List<Ciudades>> ObtenerCuidadesPor(int id)
        {
            try
            {
                if (id <= 0)
                    throw new ArgumentException("El id debe ser mayor a 0");
                var ciudades = await _repository.ObtenerCuidadesPor(id);
                return ciudades;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error en servicio al obtener ciudades : {ex.Message}", ex);
            }
        }

        public async Task<List<Rutas>> ObtenerRutasPor(int id)
        {
            try
            {
                if (id <= 0)
                    throw new ArgumentException("El id debe ser mayor a 0");
                var rutas = await _repository.ObtenerRutasPor(id);
                return rutas;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error en servicio al obtener rutas : {ex.Message}", ex);
            }
        }

        public async Task<List<Buses>> ObtenerBusesPor(int id)
        {
            try
            {
                if (id <= 0)
                    throw new ArgumentException("El id debe ser mayor a 0");
                var buses = await _repository.ObtenerBusesPor(id);
                return buses;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error en servicio al obtener buses del transportista: {ex.Message}", ex);
            }
        }

        public async Task<List<Programaciones>> ObtenerProgramacionPor(int Id, DateTime Fecha, int IdOrigen, int IdDestino)
        {
            try
            {
                if (IdOrigen <= 0 || IdDestino <= 0)
                    throw new ArgumentException("Los id de origen y destino deben ser mayores a 0");
                var programaciones = await _repository.ObtenerProgramacionPor(Id, Fecha, IdOrigen, IdDestino);
                return programaciones;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error en servicio al obtener programaciones: {ex.Message}", ex);
            }
        }
        public async Task<List<TipoAsiento>> ObtenerTiposAsiento()
        {
            try
            {
                var tipos = await _repository.ObtenerTiposAsiento();
                return tipos;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error en servicio al obtener tipos de asiento: {ex.Message}", ex);
            }
        }

        public async Task<List<DetalleProgramacion>> ObtenerAsientosPor(int id)
        {
            try
            {
                if (id <= 0)
                    throw new ArgumentException("El id debe ser mayor a 0");
                var asientos = await _repository.ObtenerAsientosPor(id);
                return asientos;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error en servicio al obtener asientos: {ex.Message}", ex);
            }
        }

        public async Task<string> BloquearAsientoPor(int idDetalleProgramacion)
        {
            try
            {
                if (idDetalleProgramacion <= 0)
                    throw new ArgumentException("El id del detalle de programación debe ser mayor a 0");
                string resultado = await _repository.BloquearAsientoPor(idDetalleProgramacion);
                return resultado;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error en servicio al bloquear asiento: {ex.Message}", ex);
            }

        }

        public async Task<bool> LimpiarBloqueoAsientos()
        {
            try
            {
                var resultado = await _repository.LimpiarBloqueoAsientos();
                return resultado;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error en servicio al limpiar bloqueo de asientos: {ex.Message}", ex);
            }
        }

        public async Task<bool> LiberarAsientoPorToken(string token)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(token))
                    throw new ArgumentException("El token no puede estar vacío");
                var resultado = await _repository.LiberarAsientoPorToken(token);
                return resultado;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error en servicio al liberar asiento por token: {ex.Message}", ex);
            }
        }
        public async Task<int> CrearReserva(int TipoDocumento, string NroDocumento, string Pasajero, DateTime? FechaNacimiento, int Edad, string Sexo, string Ruc, string RazonSocial, string Direccion, int TipoDocVenta, DateTime? FechaEmision, int IdAgenciaOrigen, int IdAgenciaDestino, string FormaDePago, string MedioPago, string Tarjeta, DateTime? FechaVencimiento, double Adelanto, string Observaciones, int IdUsuario, int Estado, int IdDocumento, int IdDetalleProgramacion, string precio, string PrecioLetra, string PrecioReprog, string HoraSalida, string Menor, int Embarque, string Telefono)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(NroDocumento))
                    throw new ArgumentException("El número de documento no puede estar vacío");
                if (string.IsNullOrWhiteSpace(Pasajero))
                    throw new ArgumentException("El nombre del pasajero no puede estar vacío");
                if (IdDetalleProgramacion <= 0)
                    throw new ArgumentException("El id del detalle de programación debe ser mayor a 0");
                var resultado = await _repository.CrearReserva(TipoDocumento, NroDocumento, Pasajero, FechaNacimiento, Edad, Sexo, Ruc, RazonSocial, Direccion, TipoDocVenta, FechaEmision, IdAgenciaOrigen, IdAgenciaDestino, FormaDePago, MedioPago, Tarjeta, FechaVencimiento, Adelanto, Observaciones, IdUsuario, Estado, IdDocumento, IdDetalleProgramacion, precio, PrecioLetra, PrecioReprog, HoraSalida, Menor, Embarque, Telefono);
                return resultado;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error en servicio al crear reserva: {ex.Message}", ex);
            }
        }
      
    }
}
