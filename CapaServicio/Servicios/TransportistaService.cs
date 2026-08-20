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

        public async Task<string> BloquearAsientoPor(int idDetalleProgramacion, int? Tiempo)
        {
            try
            {
                if (idDetalleProgramacion <= 0)
                    throw new ArgumentException("El id del detalle de programación debe ser mayor a 0");
                string resultado = await _repository.BloquearAsientoPor(idDetalleProgramacion, Tiempo);
                return resultado;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error en servicio al bloquear asiento: {ex.Message}", ex);
            }

        }

        public async Task<DateTime> ExtenderReserva(string token, int tiempo)
        {
            try
            {
                var resultado = await _repository.ExtenderReserva(token, tiempo);
                return resultado;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error en servicio al extender reserva: {ex.Message}", ex);
            }
        }

        public async Task<DateTime> ExtenderReservaPorId(int IdDetalle, int tiempo)
        {
            try
            {
                var resultado = await _repository.ExtenderReservaPorId(IdDetalle, tiempo);
                return resultado;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error en servicio al extender reserva: {ex.Message}", ex);
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

        public async Task<bool> LiberarAsientoPorId(int IdDetalle)
        {
            try
            {
                if (IdDetalle < 0)
                    throw new ArgumentException("El id del detalle no puede estar vacío");
                var resultado = await _repository.LiberarAsientoPorId(IdDetalle);
                return resultado;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error en servicio al liberar asiento por id: {ex.Message}", ex);
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

        public async Task<Login> VerUsuarioPor(int IdEmpresa)
        {
            try
            {
                if (IdEmpresa <= 0)
                    throw new ArgumentException("El id de empresa debe ser mayor a 0");
                var usuario = await _repository.VerUsuarioPor(IdEmpresa);
                return usuario;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error en servicio al verificar usuario: {ex.Message}", ex);
            }
        }

        public async Task<string> ObtenerSeriePor(int TipoDocumento, int IdEmpresa, int IdEstablecimiento)
        {
            try
            {
                if (IdEmpresa <= 0)
                    throw new ArgumentException("El id de empresa debe ser mayor a 0");
                string serie = await _repository.ObtenerSeriePor(TipoDocumento, IdEmpresa, IdEstablecimiento);
                return serie;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error en servicio al Obtener serie: {ex.Message}", ex);
            }
        }

        public async Task<DetalleDocVenta> ObtenerProductoPor(int IdEmpresa)
        {
            try
            {
                if (IdEmpresa <= 0)
                    throw new ArgumentException("El id de empresa debe ser mayor a 0");
                var producto = await _repository.ObtenerProductoPor(IdEmpresa);
                return producto;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error en servicio al Obtener producto: {ex.Message}", ex);
            }
        }

        public async Task<bool> ReprogramarPasajePor(int IdDetalleProgramacion, string Tipo, int IdEmpresa, int IdUsuario)
        {
            try
            {
                if (IdDetalleProgramacion <= 0)
                    throw new ArgumentException("El id del detalle de programación debe ser mayor a 0");
                if (string.IsNullOrWhiteSpace(Tipo))
                    throw new ArgumentException("El tipo no puede estar vacío");
                if (IdEmpresa <= 0)
                    throw new ArgumentException("El id de empresa debe ser mayor a 0");

                var resultado = await _repository.ReprogramarPasajePor(IdDetalleProgramacion, Tipo, IdEmpresa, IdUsuario);
                return resultado;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error en servicio al reprogramar pasaje: {ex.Message}", ex);
            }
        }

        public async Task<(string, DocumentoElectronicoResponse)> VerEstadoReserva(int IdDetalleProgramacion)
        {
            try
            {
                if (IdDetalleProgramacion <= 0)
                    throw new ArgumentException("El id del detalle de programación debe ser mayor a 0");
                var estado = await _repository.VerEstadoReserva(IdDetalleProgramacion);
                return estado;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error en servicio al verificar estado de reserva: {ex.Message}", ex);
            }
        }
        public async Task<int> ObtenerIdDocVentaPor(int IdDetalleProgramacion)
        {
            try
            {
                if (IdDetalleProgramacion <= 0)
                    throw new ArgumentException("El id del detalle de programación debe ser mayor a 0");
                var idDocVenta = await _repository.ObtenerIdDocVentaPor(IdDetalleProgramacion);
                return idDocVenta;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error en servicio al obtener id de documento de venta: {ex.Message}", ex);
            }
        }

        public async Task<(int, int)> BuscarDocumentoEnviadoOSE(int IdDocVenta)
        {
            try
            {
                if (IdDocVenta <= 0)
                    throw new ArgumentException("El id del documento de venta debe ser mayor a 0");
                var resultado = await _repository.BuscarDocumentoEnviadoOSE(IdDocVenta);
                return resultado;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error en servicio al buscar documento enviado a OSE: {ex.Message}", ex);
            }
        }

        public async Task<bool> AnularDocumento(int IdDocVenta, int IdUsuario)
        {
            try
            {
                if (IdDocVenta <= 0)
                    throw new ArgumentException("El id del documento de venta debe ser mayor a 0");
                var resultado = await _repository.AnularDocumento(IdDocVenta, IdUsuario);
                return resultado;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error en servicio al anular documento de venta: {ex.Message}", ex);
            }
        }

        public async Task<bool> LimpiarPor(int IdDetalleProgramacion)
        {
            try
            {
                if (IdDetalleProgramacion <= 0)
                    throw new ArgumentException("El id del detalle de programación debe ser mayor a 0");
                var resultado = await _repository.LimpiarPor(IdDetalleProgramacion);
                return resultado;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error en servicio al limpiar reserva: {ex.Message}", ex);
            }
        }

        public async Task<DocumentoVenta> ObtenerDatosDocVentaPor(int IdDocVenta)
        {
            try
            {
                if (IdDocVenta <= 0)
                    throw new ArgumentException("El id del documento de venta debe ser mayor a 0");
                var resultado = await _repository.ObtenerDatosDocVentaPor(IdDocVenta);
                return resultado;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error en servicio al obtener datos de documento de venta: {ex.Message}", ex);
            }
        }

        public async Task<int> VerIdDetalle(int IdProgramacion, int Asiento)
        {
            try
            {
                if (IdProgramacion <= 0)
                    throw new ArgumentException("El id de programación debe ser mayor a 0");
                if (Asiento <= 0)
                    throw new ArgumentException("El número de asiento debe ser mayor a 0");
                var resultado = await _repository.VerIdDetalle(IdProgramacion, Asiento);
                return resultado;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error en servicio al obtener id de detalle: {ex.Message}", ex);
            }
        }

        public async Task<int> GuardarDetalles(int IdProgramacion, string Asientos)
        {
            try
            {
                if (IdProgramacion <= 0)
                    throw new ArgumentException("El id de programación debe ser mayor a 0");
                if (string.IsNullOrWhiteSpace(Asientos))
                    throw new ArgumentException("La lista de asientos no puede estar vacía");
                var resultado = await _repository.GuardarDetalles(IdProgramacion, Asientos);
                return resultado;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error en servicio al guardar detalles: {ex.Message}", ex);
            }
        }

        public async Task<List<(int, int)>> VerAsientosPorBoleto(int numeroboleto)
        {
            try
            {
                if (numeroboleto <= 0)
                    throw new ArgumentException("El número de boleto debe ser mayor a 0");
                var resultado = await _repository.VerAsientosPorBoleto(numeroboleto);
                return resultado;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error en servicio al obtener asientos por boleto: {ex.Message}", ex);
            }
        }

        public async Task<int> GuardarDataPosponer(string jsonData, int numeroBoleto)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(jsonData))
                    throw new ArgumentException("El jsonData no puede estar vacío");
                if (numeroBoleto <= 0)
                    throw new ArgumentException("El número de boleto debe ser mayor a 0");
                var resultado = await _repository.GuardarDataPosponer(jsonData, numeroBoleto);
                return resultado;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error en servicio al guardar data posponer: {ex.Message}", ex);
            }
        }

        public async Task<decimal> ObtenerMontoAnteriorPor(int IdDetalleProgramacion)
        {
            try
            {
                if (IdDetalleProgramacion <= 0)
                    throw new ArgumentException("El id del detalle de programación debe ser mayor a 0");
                var resultado = await _repository.ObtenerMontoAnteriorPor(IdDetalleProgramacion);
                return resultado;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error en servicio al obtener monto anterior: {ex.Message}", ex);
            }
        }

        public async Task<decimal> ObtenerMontosPor(int IdProgramacion, int Asiento, int IdDestino)
        {
            try
            {
                if (IdProgramacion <= 0)
                    throw new ArgumentException("El id de programación debe ser mayor a 0");
                if (Asiento <= 0)
                    throw new ArgumentException("El número de asiento debe ser mayor a 0");
                if (IdDestino <= 0)
                    throw new ArgumentException("El id del destino debe ser mayor a 0");
                var resultado = await _repository.ObtenerMontosPor(IdProgramacion, Asiento, IdDestino);
                return resultado;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error en servicio al obtener montos: {ex.Message}", ex);
            }
        }

        public async Task<(int, string)> ObetenerDatosReservaPorId(int IdReserva)
        {
            try
            {
                if (IdReserva <= 0)
                    throw new ArgumentException("El id de reserva debe ser mayor a 0");
                var resultado = await _repository.ObetenerDatosReservaPorId(IdReserva);
                return resultado;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error en servicio al obtener datos de reserva por id: {ex.Message}", ex);
            }
        }

        public async Task<(string, string)> ObtenerPlacaHora(int IdProgramacion, int PuntoEmbarque)
        {
            try
            {
                if (IdProgramacion <= 0)
                    throw new ArgumentException("El id de programación debe ser mayor a 0");
                if (PuntoEmbarque <= 0)
                    throw new ArgumentException("El id del punto de embarque debe ser mayor a 0");
                var resultado = await _repository.ObtenerPlacaHora(IdProgramacion, PuntoEmbarque);
                return resultado;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error en servicio al obtener placa y hora: {ex.Message}", ex);
            }
        }

        public async Task<List<Programaciones>> ObtenerProgramacionPorRuta(int Id, DateTime Fecha, int IdRuta)
        {
            try
            {
                if (IdRuta <= 0)
                    throw new ArgumentException("El id de ruta debe ser mayor a 0");
                var programaciones = await _repository.ObtenerProgramacionPorRuta(Id, Fecha, IdRuta);
                return programaciones;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error en servicio al obtener programaciones por ruta: {ex.Message}", ex);
            }
        }
    }
}
