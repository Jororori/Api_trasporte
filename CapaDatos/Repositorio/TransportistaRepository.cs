using CapaDatos.Interfaces;
using CapaEntidades;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text.Json;
namespace CapaDatos.Repositorio
{
    public class TransportistaRepository : ITransportistaRepository
    {

        private readonly IConfiguration _configuration;

        public TransportistaRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private SqlConnection GetConnection(string name)
        {
            return new SqlConnection(
                _configuration.GetConnectionString(name)
            );
        }


        public async Task<List<Transportista>> ObtenerTodosAsync()
        {
            var transportistas = new List<Transportista>();

            try
            {
                using (SqlConnection connection = new SqlConnection("_connectionString"))
                {
                    await connection.OpenAsync();

                    using (SqlCommand command = new SqlCommand("SELECT Id, Nombre, Cedula, Telefono, Email, Activo, FechaRegistro FROM Transportistas", connection))
                    {
                        command.CommandType = CommandType.Text;

                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                transportistas.Add(new Transportista
                                {
                                    Id = (int)reader["Id"],
                                    Nombre = reader["Nombre"].ToString() ?? string.Empty,
                                    Cedula = reader["Cedula"].ToString() ?? string.Empty,
                                    Telefono = reader["Telefono"].ToString() ?? string.Empty,
                                    Email = reader["Email"].ToString() ?? string.Empty,
                                    Activo = (bool)reader["Activo"],
                                    FechaRegistro = (DateTime)reader["FechaRegistro"]
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener transportistas: {ex.Message}", ex);
            }

            return transportistas;
        }

        public async Task<Transportista?> ObtenerPorIdAsync(int id)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection("_connectionString"))
                {
                    await connection.OpenAsync();

                    using (SqlCommand command = new SqlCommand("SELECT Id, Nombre, Cedula, Telefono, Email, Activo, FechaRegistro FROM Transportistas WHERE Id = @Id", connection))
                    {
                        command.CommandType = CommandType.Text;
                        command.Parameters.AddWithValue("@Id", id);

                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                return new Transportista
                                {
                                    Id = (int)reader["Id"],
                                    Nombre = reader["Nombre"].ToString() ?? string.Empty,
                                    Cedula = reader["Cedula"].ToString() ?? string.Empty,
                                    Telefono = reader["Telefono"].ToString() ?? string.Empty,
                                    Email = reader["Email"].ToString() ?? string.Empty,
                                    Activo = (bool)reader["Activo"],
                                    FechaRegistro = (DateTime)reader["FechaRegistro"]
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener transportista con id {id}: {ex.Message}", ex);
            }

            return null;
        }
        public async Task<List<Ciudades>> ObtenerCuidadesPor(int id)
        {
            var ciudades = new List<Ciudades>();

            try
            {
                using (SqlConnection connection = GetConnection("EmpresarialConnection"))
                {
                    await connection.OpenAsync();

                    using (SqlCommand command = new SqlCommand("SP_ListarEstablecimientos", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@IdEmpresa", id);

                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                ciudades.Add(new Ciudades
                                {
                                    IdCiudad = Convert.ToInt32(reader[0]),
                                    NombreCiudad = reader[2]?.ToString() ?? "",
                                    Codigo = reader[1]?.ToString() ?? "",
                                    IdUbigeo = Convert.ToInt16(reader[5])
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"id = {id}: {ex.Message}", ex);
            }

            return ciudades;
        }

        public async Task<List<Rutas>> ObtenerRutasPor(int id)
        {
            var rutas = new List<Rutas>();
            try
            {
                using (SqlConnection connection = GetConnection("TransportistaConnection"))
                {
                    await connection.OpenAsync();
                    using (SqlCommand command = new SqlCommand("SP_ListarRutas", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@IdEmpresa", id);
                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                rutas.Add(new Rutas
                                {
                                    IdRuta = Convert.ToInt32(reader[0]),
                                    Origen = reader[2]?.ToString() ?? "",
                                    Destino = reader[3]?.ToString() ?? "",
                                    Distancia = Convert.ToInt32(reader[4]),
                                    IdOrigen = Convert.ToInt32(reader[7]),
                                    IdDestino = Convert.ToInt32(reader[8]),

                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener rutas con id {id}: {ex.Message}", ex);
            }
            return rutas;
        }


        public async Task<List<Buses>> ObtenerBusesPor(int id)
        {
            var buses = new List<Buses>();
            try
            {
                using (SqlConnection connection = GetConnection("TransportistaConnection"))
                {
                    await connection.OpenAsync();
                    using (SqlCommand command = new SqlCommand("SP_ListarBuses", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@IdEmpresa", id);
                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                var bus = new Buses
                                {
                                    IdBus = Convert.ToInt32(reader[0]),
                                    Placa = reader[4]?.ToString() ?? "",
                                    Modelo = reader[5]?.ToString() ?? "",
                                    Capacidad = Convert.ToInt32(reader[2]),
                                    Asiento = new List<Asientos>() // importante inicializar
                                };

                                bus.Asiento = await ObtenerAsientosPorBus(bus.IdBus);

                                buses.Add(bus);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener buses con id {id}: {ex.Message}", ex);
            }
            return buses;
        }

        public async Task<List<Asientos>> ObtenerAsientosPorBus(int idBus)
        {
            var asientos = new List<Asientos>();

            using (SqlConnection connection = GetConnection("TransportistaConnection"))
            {
                await connection.OpenAsync();

                using (SqlCommand command = new SqlCommand("SP_BuscarAsientos", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@IdOmnibus", idBus);

                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            asientos.Add(new Asientos
                            {
                                IdAsiento = Convert.ToInt32(reader[4]),
                                Numero = reader[0]?.ToString(),
                                NumeroFila = Convert.ToInt32(reader[1]),
                                NumeroColumna = Convert.ToInt32(reader[2]),
                                NumeroPiso = Convert.ToInt32(reader[3]),
                            });
                        }
                    }
                }
            }

            return asientos;
        }


        public async Task<List<Programaciones>> ObtenerProgramacionPor(int Id, DateTime Fecha, int IdOrigen, int IdDestino)
        {
            var programaciones = new List<Programaciones>();
            try
            {
                using (SqlConnection connection = GetConnection("TransportistaConnection"))
                {
                    await connection.OpenAsync();
                    using (SqlCommand command = new SqlCommand("SP_FiltrarSalidasV2", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@FechaProgramacion", Fecha);
                        command.Parameters.AddWithValue("@IdEmpresa", Id);
                        command.Parameters.AddWithValue("@IdOrigen", IdOrigen);
                        command.Parameters.AddWithValue("@IdDestino", IdDestino);
                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                var jsonPuntos = reader[14]?.ToString();
                                string HoraLlegadaStr = Convert.ToString(reader[16]);
                                string HoraSalidaStr = Convert.ToString(reader[5]);
                                programaciones.Add(new Programaciones
                                {
                                    IdProgramacion = Convert.ToInt32(reader[0]),
                                    FechaProgramacion = Convert.ToDateTime(reader[4]),
                                    IdOrigen = Convert.ToInt32(reader[17]),
                                    Origen = reader[1]?.ToString() ?? "",
                                    IdDestino = Convert.ToInt32(reader[18]),
                                    Destino = reader[2]?.ToString() ?? "",
                                    MarcaBus = reader[11]?.ToString() ?? "",
                                    ModeloBus = reader[10]?.ToString() ?? "",
                                    placaBus = reader[3]?.ToString() ?? "",
                                    IdConductor = Convert.ToInt32(reader[12]),
                                    Conductor = reader[13]?.ToString() ?? "",
                                    PrecioPiso1 = Convert.ToDecimal(reader[6]),
                                    PrecioPiso2 = Convert.ToDecimal(reader[7]),
                                    Estado = Convert.ToInt32(reader[9]),
                                    FechaLlegada = Convert.ToDateTime(reader[15]),
                                    HoraSalida = DateTime.Parse(HoraSalidaStr).TimeOfDay,
                                    HoraLlegada = DateTime.Parse(HoraLlegadaStr).TimeOfDay,
                                    PuntosIntermedios = string.IsNullOrEmpty(jsonPuntos) ? new List<PuntoIntemedio>() : JsonSerializer.Deserialize<List<PuntoIntemedio>>(jsonPuntos),
                                    PuntoEmbarque = await ObtenerZonasEmbarquePor(Convert.ToInt32(reader[0])), 
                                    DireccionOrigen = reader[19]?.ToString() ?? "",
                                    DireccionDestino = reader[20]?.ToString() ?? "",
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener programaciones: {ex.Message}", ex);
            }
            return programaciones;
        }

        public async Task<List<Programaciones>> ObtenerProgramacionPorRuta(int Id, DateTime Fecha, int IdRuta)
        {
            var programaciones = new List<Programaciones>();
            try
            {
                using (SqlConnection connection = GetConnection("TransportistaConnection"))
                {
                    await connection.OpenAsync();
                    using (SqlCommand command = new SqlCommand("SP_FiltrarSalidasV3", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@FechaProgramacion", Fecha);
                        command.Parameters.AddWithValue("@IdEmpresa", Id);
                        command.Parameters.AddWithValue("@IdRuta", IdRuta);
                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                var jsonPuntos = reader[14]?.ToString();
                                string HoraLlegadaStr = Convert.ToString(reader[16]);
                                string HoraSalidaStr = Convert.ToString(reader[5]);
                                programaciones.Add(new Programaciones
                                {
                                    IdProgramacion = Convert.ToInt32(reader[0]),
                                    FechaProgramacion = Convert.ToDateTime(reader[4]),
                                    IdOrigen = Convert.ToInt32(reader[17]),
                                    Origen = reader[1]?.ToString() ?? "",
                                    IdDestino = Convert.ToInt32(reader[18]),
                                    Destino = reader[2]?.ToString() ?? "",
                                    MarcaBus = reader[11]?.ToString() ?? "",
                                    ModeloBus = reader[10]?.ToString() ?? "",
                                    placaBus = reader[3]?.ToString() ?? "",
                                    IdConductor = Convert.ToInt32(reader[12]),
                                    Conductor = reader[13]?.ToString() ?? "",
                                    PrecioPiso1 = Convert.ToDecimal(reader[6]),
                                    PrecioPiso2 = Convert.ToDecimal(reader[7]),
                                    Estado = Convert.ToInt32(reader[9]),
                                    FechaLlegada = Convert.ToDateTime(reader[15]),
                                    HoraSalida = DateTime.Parse(HoraSalidaStr).TimeOfDay,
                                    HoraLlegada = DateTime.Parse(HoraLlegadaStr).TimeOfDay,
                                    PuntosIntermedios = string.IsNullOrEmpty(jsonPuntos) ? new List<PuntoIntemedio>() : JsonSerializer.Deserialize<List<PuntoIntemedio>>(jsonPuntos),
                                    PuntoEmbarque = await ObtenerZonasEmbarquePor(Convert.ToInt32(reader[0])),
                                    DireccionOrigen = reader[19]?.ToString() ?? "",
                                    DireccionDestino = reader[20]?.ToString() ?? "",
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener programaciones: {ex.Message}", ex);
            }
            return programaciones;
        }

        public async Task<List<ZonasEmbarque>> ObtenerZonasEmbarquePor(int idProgramacion)
        {
            var zonasEmbarque = new List<ZonasEmbarque>();
            try
            {
                using (SqlConnection connection = GetConnection("TransportistaConnection"))
                {
                    await connection.OpenAsync();
                    using (SqlCommand command = new SqlCommand("SP_ListarZonasEmbarque", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@IdProgramacion", idProgramacion);
                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                string HoraSalidaStr = Convert.ToString(reader[2]);

                                zonasEmbarque.Add(new ZonasEmbarque
                                {
                                    ID = Convert.ToInt32(reader[0]),
                                    NombrePuntoIntermedio = reader[1]?.ToString() ?? "",
                                    HoraSalida = DateTime.Parse(HoraSalidaStr).TimeOfDay,
                                    Direccion = reader[3]?.ToString() ?? "",
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener zonas de embarque para la programación con id {idProgramacion}: {ex.Message}", ex);
            }
            return zonasEmbarque;
        }

        public async Task<List<TipoAsiento>> ObtenerTiposAsiento()
        {
            var tiposAsiento = new List<TipoAsiento>();
            try
            {
                using (SqlConnection connection = GetConnection("TransportistaConnection"))
                {
                    await connection.OpenAsync();
                    using (SqlCommand command = new SqlCommand("SELECT IdTipoAsiento, TIpoAsientpo FROM TipoAsiento", connection))
                    {
                        command.CommandType = CommandType.Text;
                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                tiposAsiento.Add(new TipoAsiento
                                {
                                    IdTipoAsiento = Convert.ToInt32(reader[0]),
                                    TiposAsiento = reader[1]?.ToString() ?? ""

                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener tipos de asiento: {ex.Message}", ex);
            }
            return tiposAsiento;
        }

        public async Task<List<DetalleProgramacion>> ObtenerAsientosPor(int id)
        {
            var asientos = new List<DetalleProgramacion>();
            using (SqlConnection connection = GetConnection("TransportistaConnection"))
            {
                await connection.OpenAsync();
                using (SqlCommand command = new SqlCommand("SP_CargarPlantillaV2", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@IdProgramacion", id);
                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {

                        while (await reader.ReadAsync())
                        {
                            var jsonPuntos = reader[10]?.ToString();

                            asientos.Add(new DetalleProgramacion
                            {
                                IdDetalleProgramacion = Convert.ToInt32(reader[5]),
                                ValorAsiento = reader[0]?.ToString().Trim(),
                                NumeroFila = Convert.ToInt32(reader[1]),
                                NumeroColumna = Convert.ToInt32(reader[2]),
                                NumeroPiso = Convert.ToInt32(reader[3]),
                                Estado = Convert.ToInt32(reader[4]),
                            });
                        }
                    }
                }
            }
            return asientos;
        }

        public async Task<string> BloquearAsientoPor(int idDetalleProgramacion, int? Tiempo)
        {
            try
            {
                using SqlConnection connection = GetConnection("TransportistaConnection");
                await connection.OpenAsync();

                using SqlCommand command = new SqlCommand("SP_BloquearDetalleSalida", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@IdDetalleProgramacion", SqlDbType.Int).Value = idDetalleProgramacion;
                command.Parameters.Add("@Tiempo", SqlDbType.Int).Value = Tiempo ?? (object)DBNull.Value;

                // ExecuteScalarAsync obtiene el objeto devuelto por el SELECT
                object result = await command.ExecuteScalarAsync();

                // Convertimos a string de forma segura; si es nulo, devuelve un string vacío
                return result?.ToString() ?? string.Empty;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al bloquear asiento con idDetalleProgramacion {idDetalleProgramacion}: {ex.Message}", ex);
            }
        }

        public async Task<DateTime> ExtenderReserva(string token, int tiempo)
        {
            DateTime FechaExpira = DateTime.MinValue;
            try
            {
                using (SqlConnection connection = GetConnection("TransportistaConnection"))
                {
                    await connection.OpenAsync();
                    using (SqlCommand command = new SqlCommand("SP_ExtenderReserva", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@Token", token);
                        command.Parameters.AddWithValue("@Tiempo", tiempo);

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                FechaExpira = reader.GetDateTime(0);
                            }
                        }
                    }

                }

                return FechaExpira;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al extender reserva con token {token}: {ex.Message}", ex);
            }
        }


        public async Task<DateTime> ExtenderReservaPorId(int IdDetalle, int tiempo)
        {
            DateTime FechaExpira = DateTime.MinValue;
            try
            {
                using (SqlConnection connection = GetConnection("TransportistaConnection"))
                {
                    await connection.OpenAsync();
                    using (SqlCommand command = new SqlCommand("SP_ExtenderReservaV2", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@IdDetalle", IdDetalle);
                        command.Parameters.AddWithValue("@Tiempo", tiempo);

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                FechaExpira = reader.GetDateTime(0);
                            }
                        }
                    }

                }

                return FechaExpira;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al extender reserva con id {IdDetalle}: {ex.Message}", ex);
            }
        }

        public async Task<(string, DocumentoElectronicoResponse)> VerEstadoReserva(int IdDetalleProgramacion)
        {
            string estado = string.Empty;
            DocumentoElectronicoResponse DocElectronico = null;
            try
            {
                using (SqlConnection connection = GetConnection("TransportistaConnection"))
                {
                    await connection.OpenAsync();
                    using (SqlCommand command = new SqlCommand("SP_VerEstadoReserva", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@IdDetalleProgramacion", IdDetalleProgramacion);
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                estado = reader.GetString(0);
                                if (estado == "Finalizado")
                                {
                                    DocElectronico = new DocumentoElectronicoResponse
                                    {
                                        Asiento = reader.GetInt32(1),
                                        Numero = reader.GetString(2),
                                        Pdf = ObtenerPdfBytes(reader.GetString(3))
                                    };
                                }
                                else
                                {
                                    
                                }
                                
                            }
                        }
                    }
                }
                return (estado, DocElectronico);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al verificar estado de reserva con IdDetalleProgramacion {IdDetalleProgramacion}: {ex.Message}", ex);
            }
        }

        private byte[] ObtenerPdfBytes(string ruta)
        {
            try
            {
                string rutaPDF = Path.Combine(
                    @"E:\Site Web\Factura-2.com\site\wwwroot\DocsFilesXML",
                    ruta + ".pdf"
                );

                return File.ReadAllBytes(rutaPDF);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al leer PDF: {ex.Message}", ex);
            }
        }
        public async Task<bool> LimpiarBloqueoAsientos()
        {
            try
            {
                using (SqlConnection connection = GetConnection("TransportistaConnection"))
                {
                    await connection.OpenAsync();
                    using (SqlCommand command = new SqlCommand("SP_LiberarDetalleTokenAut", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        int rowsAffected = await command.ExecuteNonQueryAsync();
                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al limpiar bloqueo de asientos: {ex.Message}", ex);
            }

        }

        public async Task<bool> LiberarAsientoPorToken(string token)
        {
            try
            {
                using (SqlConnection connection = GetConnection("TransportistaConnection"))
                {
                    await connection.OpenAsync();
                    using (SqlCommand command = new SqlCommand("SP_LiberarDetalleConToken", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add("@Token", SqlDbType.VarChar).Value = token;
                        int rowsAffected = await command.ExecuteNonQueryAsync();
                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al liberar asiento con token {token}: {ex.Message}", ex);
            }
        }

        public async Task<bool> LiberarAsientoPorId(int IdDetalle)
        {
            try
            {
                using (SqlConnection connection = GetConnection("TransportistaConnection"))
                {
                    await connection.OpenAsync();
                    using (SqlCommand command = new SqlCommand("SP_LiberarDetalleConId", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add("@IdDetalle", SqlDbType.Int).Value = IdDetalle;
                        int rowsAffected = await command.ExecuteNonQueryAsync();
                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al liberar asiento con id {IdDetalle}: {ex.Message}", ex);
            }
        }

        public async Task<int> CrearReserva(int TipoDocumento, string NroDocumento, string Pasajero, DateTime? FechaNacimiento, int Edad, string Sexo, string Ruc, string RazonSocial, string Direccion, int TipoDocVenta, DateTime? FechaEmision, int IdAgenciaOrigen, int IdAgenciaDestino, string FormaDePago, string MedioPago, string Tarjeta, DateTime? FechaVencimiento, double Adelanto, string Observaciones, int IdUsuario, int Estado, int IdDocumento, int IdDetalleProgramacion, string precio, string PrecioLetra, string PrecioReprog, string HoraSalida, string Menor, int Embarque, string Telefono)
        {
            int codigo = 0;
            try
            {
                using (SqlConnection connection = GetConnection("TransportistaConnection"))
                {
                    await connection.OpenAsync();
                    using (SqlCommand command = new SqlCommand("SP_CrearDetalleSalidaV2", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@TipoDoc", TipoDocumento);
                        command.Parameters.AddWithValue("@NroDoc", NroDocumento);
                        command.Parameters.AddWithValue("@Pasajero", Pasajero);
                        var param = command.Parameters.Add("@FechaNacimiento", SqlDbType.Date); param.Value = (object?)FechaNacimiento?.Date ?? DBNull.Value;
                        command.Parameters.AddWithValue("@Edad", Edad);
                        command.Parameters.AddWithValue("@Sexo", Sexo);
                        command.Parameters.AddWithValue("@Ruc", Ruc);
                        command.Parameters.AddWithValue("@Empresa", RazonSocial);
                        command.Parameters.AddWithValue("@Direccion", Direccion);
                        command.Parameters.AddWithValue("@TipoDocVenta", TipoDocVenta);
                        command.Parameters.AddWithValue("@FechaEmision", FechaEmision);
                        command.Parameters.AddWithValue("@AgOrigen", IdAgenciaOrigen);
                        command.Parameters.AddWithValue("@AgDestino", IdAgenciaDestino);
                        command.Parameters.AddWithValue("@Forma", FormaDePago);
                        command.Parameters.AddWithValue("@FormaPago", MedioPago);
                        command.Parameters.AddWithValue("@Tarjeta", Tarjeta);
                        var param2 = command.Parameters.Add("@FechaVenc", SqlDbType.Date); param.Value = (object?)FechaNacimiento?.Date ?? DBNull.Value;
                        command.Parameters.AddWithValue("@Adelanto", Adelanto);
                        command.Parameters.AddWithValue("@Observacion", Observaciones);
                        command.Parameters.AddWithValue("@IdUsuario", IdUsuario);
                        command.Parameters.AddWithValue("@Estado", Estado);
                        command.Parameters.AddWithValue("@IdDocumento", IdDocumento);
                        command.Parameters.AddWithValue("@IdDetalleSalida", IdDetalleProgramacion);
                        command.Parameters.AddWithValue("@Precio", precio);
                        command.Parameters.AddWithValue("@PrecioLetra", PrecioLetra);
                        command.Parameters.AddWithValue("@PrecioReprog", PrecioReprog);
                        command.Parameters.AddWithValue("@HoraSalida", HoraSalida);
                        command.Parameters.AddWithValue("@Menor", Menor);
                        command.Parameters.AddWithValue("@Embarque", Embarque);
                        command.Parameters.AddWithValue("@Telefono", Telefono);
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                codigo = reader.GetInt32(0);
                            }
                        }
                    }

                }

                return codigo;

            }
            catch (Exception ex)
            {
                throw new Exception($"Error al crear reserva: {ex.Message}", ex);
            }
        }

        public async Task<Login> VerUsuarioPor(int IdEmpresa)
        {
            Login login = new Login();
            try
            {
                using (SqlConnection connection = GetConnection("EmpresarialConnection"))
                {
                    await connection.OpenAsync();
                    using (SqlCommand command = new SqlCommand("SP_ObtenerPassPorId", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@IdEmpresa", IdEmpresa);
                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                login.Ruc = reader["Ruc"]?.ToString() ?? "";
                                login.Dni = reader["Dni"]?.ToString() ?? "";
                                login.Pass = reader["Pass"]?.ToString() ?? "";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener usuario para empresa con id {IdEmpresa}: {ex.Message}", ex);
            }
            return login;
        }
        public async Task<string> ObtenerSeriePor(int TipoDocumento, int IdEmpresa, int IdEstablecimiento)
        {
            string serie = string.Empty;
            try
            {
                using (SqlConnection connection = GetConnection("EmpresarialConnection"))
                {
                    await connection.OpenAsync();
                    using (SqlCommand command = new SqlCommand("SP_ObtenerSeriePorTipoDocV2", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@TipoDocumento", TipoDocumento);
                        command.Parameters.AddWithValue("@IdEmpresa", IdEmpresa);
                        command.Parameters.AddWithValue("@IdEstablecimiento", IdEstablecimiento);
                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                serie = reader["Serie"]?.ToString() ?? "";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener serie para tipo de documento {TipoDocumento}, empresa {IdEmpresa} y establecimiento {IdEstablecimiento}: {ex.Message}", ex);
            }
            return serie;
        }

        public async Task<DetalleDocVenta> ObtenerProductoPor(int IdEmpresa)
        {
            DetalleDocVenta detalle = new DetalleDocVenta();
            try
            {
                using (SqlConnection connection = GetConnection("FacturacionConnection"))
                {
                    await connection.OpenAsync();
                    using (SqlCommand command = new SqlCommand("SP_BuscarProductoPresentBus", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@IdEmpresa", IdEmpresa);
                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                detalle.IdProducto = Convert.ToInt32(reader["IdPresentacion"]);
                                detalle.Codigo = reader["Codigo"]?.ToString() ?? "";
                                detalle.Descripcion = reader["DescripcionProd"]?.ToString() ?? "";
                                detalle.Medida = reader["Medida"]?.ToString() ?? "";
                                detalle.Precio = String.Format("{0:0.00}", Convert.ToDecimal(reader["PrecioVenta"])).Replace(',', '.');
                                detalle.Descuento = String.Format("{0:0.00}", Convert.ToDecimal(reader["Descuento"])).Replace(',', '.');
                                detalle.Moneda = reader["Moneda"]?.ToString() ?? "";
                                detalle.Equivalencia = String.Format("{0:0.00}", Convert.ToDecimal(reader["Equivalencia"])).Replace(',', '.');
                                detalle.NombreCompleto = (reader["Categoria"]?.ToString() ?? "") + ":" + (reader["Codigo"]?.ToString() ?? "") + "-" + (reader["DescripcionProd"]?.ToString() ?? "") + "-" + (reader["DescripcionMedida"]?.ToString() ?? reader["Medida"]?.ToString() ?? "") + " - " + ((reader["Moneda"]?.ToString() == "PEN") ? "S/" : "$") + " " + string.Format("{0:0.00}", Convert.ToDecimal(reader["PrecioVenta"]));
                                detalle.TipoProducto = reader["TipoProd"]?.ToString() ?? "";
                                detalle.Exonerado = Convert.ToInt32(reader["Exonerado"]);
                                detalle.LogoProd = reader["LogoProd"]?.ToString() ?? "";
                                detalle.Peso = String.Format("{0:0.00}", Convert.ToDecimal(reader["Peso"])).Replace(',', '.');
                                detalle.IdMedida = Convert.ToInt32(reader["IdMedida"]);
                                detalle.NroBolsas = Convert.ToInt32(reader["NroBolsas"]);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener producto para empresa con id {IdEmpresa}: {ex.Message}", ex);
            }
            return detalle;
        }

        public async Task<bool> ReprogramarPasajePor(int IdDetalleProgramacion, string Tipo, int IdEmpresa, int IdUsuario)
        {
            try
            {
                using (SqlConnection connection = GetConnection("TransportistaConnection"))
                {
                    await connection.OpenAsync();
                    using (SqlCommand command = new SqlCommand("SP_ReprogramarPasaje", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@IdDetalleSalida", IdDetalleProgramacion);
                        command.Parameters.AddWithValue("@TipoLiberar", Tipo);
                        command.Parameters.AddWithValue("@IdEmpresa", IdEmpresa);
                        command.Parameters.AddWithValue("@IdUsuarioLiberar", IdUsuario);
                        int rowsAffected = await command.ExecuteNonQueryAsync();
                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al reprogramar pasaje con IdDetalleProgramacion {IdDetalleProgramacion}: {ex.Message}", ex);
            }
        }

        public async Task<int> ObtenerIdDocVentaPor(int IdDetalleProgramacion)
        {
            int IdDocVenta = 0;
            try
            {
                using (SqlConnection connection = GetConnection("TransportistaConnection"))
                {
                    await connection.OpenAsync();
                    using (SqlCommand command = new SqlCommand("SP_ObtenerIdDocVentaPorDetalleProgramacion", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@IdDetalleSalida", IdDetalleProgramacion);
                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                IdDocVenta = Convert.ToInt32(reader[0]);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener IdDocVenta para IdDetalleProgramacion {IdDetalleProgramacion}: {ex.Message}", ex);
            }
            return IdDocVenta;
        }

        public async Task<(int, int)> BuscarDocumentoEnviadoOSE(int IdDocVenta)
        {
            int IdUsuario = 0;
            int EstadoOSE = 0;
            try
            {
                using (SqlConnection connection = GetConnection("FacturacionConnection"))
                {
                    await connection.OpenAsync();
                    using (SqlCommand command = new SqlCommand("SP_BuscarDocumentoEnviadoOSE", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@IdDocumento", IdDocVenta);
                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                IdUsuario = Convert.ToInt32(reader[7]);
                                EstadoOSE = Convert.ToInt32(reader[3]);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al buscar documento enviado a OSE para IdDocVenta {IdDocVenta}: {ex.Message}", ex);
            }
            return (IdUsuario, EstadoOSE);
        }

        public async Task<bool> AnularDocumento(int IdDocVenta, int IdUsuario)
        {
            try
            {
                using (SqlConnection connection = GetConnection("FacturacionConnection"))
                {
                    await connection.OpenAsync();
                    using (SqlCommand command = new SqlCommand("SP_AnularDocumento", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@IdDocumento", IdDocVenta);
                        command.Parameters.AddWithValue("@IdUsuario", IdUsuario);
                        int rowsAffected = await command.ExecuteNonQueryAsync();
                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al anular documento con IdDocVenta {IdDocVenta}: {ex.Message}", ex);

            }
        }

        public async Task<bool> LimpiarPor(int IdDetalleProgramacion)
        {
            try
            {
                using (SqlConnection connection = GetConnection("TransportistaConnection"))
                {
                    await connection.OpenAsync();
                    using (SqlCommand command = new SqlCommand("SP_LimpiarDetalleSalida", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@IdDetalleSalida", IdDetalleProgramacion);
                        int rowsAffected = await command.ExecuteNonQueryAsync();
                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al limpiar por IdDetalleProgramacion {IdDetalleProgramacion}: {ex.Message}", ex);
            }
        }

        public async Task<DocumentoVenta> ObtenerDatosDocVentaPor(int IdDocVenta)
        {
            DocumentoVenta documento = new DocumentoVenta();
            try
            {
                using (SqlConnection connection = GetConnection("FacturacionConnection"))
                {
                    await connection.OpenAsync();
                    using (SqlCommand command = new SqlCommand("SP_obtenerDocVentaApi", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@IdDocumentoVenta", IdDocVenta);
                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                documento.Ruc_emisor = reader[0].ToString();
                                documento.Dni_usuario = reader[1].ToString();
                                documento.Pass = reader[2].ToString();
                                documento.Tipo = reader[3].ToString();
                                documento.Serie = reader[4].ToString();
                                documento.Ruc = reader[5].ToString();
                                documento.Tipo_de_documento = reader[6].ToString();
                                documento.Razon_social = reader[7].ToString();
                                documento.Direccion = reader[8].ToString();
                                documento.Email = reader[9].ToString();
                                documento.Telefono = reader[10].ToString();
                                documento.Fecha_de_emision = reader[11].ToString();
                                documento.Fecha_de_vencimiento = reader[12].ToString();
                                documento.Moneda = reader[13].ToString();
                                documento.Total = Convert.ToString(reader[14]);
                                documento.Observaciones = reader[15].ToString();
                                documento.Documento_que_se_modifica_tipo = reader[16].ToString();
                                documento.Documento_que_se_modifica_serie = reader[17].ToString();
                                documento.Concepto_de_nota__de_credito = reader[18].ToString();
                                documento.Motivo_de_nota__de_credito = reader[19].ToString();
                                documento.IncluirIgv = Convert.ToInt32(reader[20]);
                                documento.Placa = reader[21].ToString();
                                documento.Bus = Convert.ToInt32(reader[22]);
                                documento.Documento_que_se_modifica_numero = reader[23].ToString();
                                documento.Invoice_lines = VerDetalleDocPor(IdDocVenta);
                                documento.IdEstablecimiento = Convert.ToInt32(reader[24]);

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener datos de DocVenta con IdDocVenta {IdDocVenta}: {ex.Message}", ex);
            }
            return documento;
        }

        private List<Detalle> VerDetalleDocPor(int IdDocVenta)
        {
            List<Detalle> detalles = new List<Detalle>();
            try
            {
                using (SqlConnection connection = GetConnection("FacturacionConnection"))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand("SP_ObtenerDetalleDocVenta", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@IdDocVenta", IdDocVenta);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Detalle detalle = new Detalle
                                {
                                    Unit_code = reader[0].ToString(),
                                    Cantidad = reader[1].ToString(),
                                    Tipo_de_igv = reader[2].ToString(),
                                    Precio_unitario = reader[3].ToString(),
                                    Descripcion = reader[4].ToString(),
                                    IdProducto = Convert.ToInt32(reader[5])
                                };
                                detalles.Add(detalle);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener detalles de DocVenta con IdDocVenta {IdDocVenta}: {ex.Message}", ex);
            }
            return detalles;
        }

        public async Task<int> VerIdDetalle(int IdProgramacion, int Asiento)
        {
            int IdDetalleProgramacion = 0;
            try
            {
                using (SqlConnection connection = GetConnection("TransportistaConnection"))
                {
                    await connection.OpenAsync();
                    using (SqlCommand command = new SqlCommand("SP_VerIdDetallePorAsiento", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@IdProgramacion", IdProgramacion);
                        command.Parameters.AddWithValue("@Asiento", Asiento);
                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                IdDetalleProgramacion = Convert.ToInt32(reader[0]);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener IdDetalleProgramacion para IdProgramacion {IdProgramacion} y Asiento {Asiento}: {ex.Message}", ex);
            }
            return IdDetalleProgramacion;
        }

        public async Task<int> GuardarDetalles(int IdProgramacion, string Asientos)
        {
            int NroBoleto = 0;
            try
            {
                using (SqlConnection connection = GetConnection("TransportistaConnection"))
                {
                    await connection.OpenAsync();
                    using (SqlCommand command = new SqlCommand("SP_GuardarDetallesWeb", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@IdProgramacion", Convert.ToString(IdProgramacion));
                        command.Parameters.AddWithValue("@Asientos", Asientos);
                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                NroBoleto = Convert.ToInt32(reader[0]);
                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al limpiar por IdProgramacion {IdProgramacion} y Asientos {Asientos}: {ex.Message}", ex);
            }
            return NroBoleto;
        }

        public async Task<List<(int, int)>> VerAsientosPorBoleto(int numeroboleto)
        {
            List<(int, int)> asientos = new List<(int, int)>();
            try
            {
                using (SqlConnection connection = GetConnection("TransportistaConnection"))
                {
                    await connection.OpenAsync();
                    using (SqlCommand command = new SqlCommand("SP_VerAsientosPorBoleto", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@NroBoleto", numeroboleto);
                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                asientos.Add((Convert.ToInt32(reader[0]), Convert.ToInt32(reader[1])));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener asientos por boleto {numeroboleto}: {ex.Message}", ex);
            }
            return asientos;
        }

        public async Task<int> GuardarDataPosponer(string jsonData, int numeroBoleto)
        {
            int Id = 0;
            try
            {
                using (SqlConnection connection = GetConnection("TransportistaConnection"))
                {
                    await connection.OpenAsync();
                    using (SqlCommand command = new SqlCommand("SP_CrearReservaTicket", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@JsonData", jsonData);
                        command.Parameters.AddWithValue("@NumeroBoleto", numeroBoleto);
                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                Id = Convert.ToInt32(reader[0]);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al guardar data posponer para boleto {numeroBoleto}: {ex.Message}", ex);
            }
            return Id;
        }

        public async Task<decimal> ObtenerMontoAnteriorPor(int IdDetalleProgramacion)
        {
            decimal monto = 0;
            try
            {
                using (SqlConnection connection = GetConnection("TransportistaConnection"))
                {
                    await connection.OpenAsync();
                    using (SqlCommand command = new SqlCommand("SP_ObtenerMontoAnteriorPor", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@IdDetalleProgramacion", IdDetalleProgramacion);
                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                monto = Convert.ToDecimal(reader[0]);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener monto anterior para detalle {IdDetalleProgramacion}: {ex.Message}", ex);
            }
            return monto;
        }

        public async Task<decimal> ObtenerMontosPor(int IdProgramacion, int Asiento, int IdDestino)
        {
            decimal monto = 0;
            try
            {
                using (SqlConnection connection = GetConnection("TransportistaConnection"))
                {
                    await connection.OpenAsync();
                    using (SqlCommand command = new SqlCommand("SP_VerPrecioActual", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@IdProgramacion", IdProgramacion);
                        command.Parameters.AddWithValue("@Asiento", Asiento);
                        command.Parameters.AddWithValue("@IdDestino", IdDestino);
                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                monto = Convert.ToDecimal(reader[0]);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener montos para programación {IdProgramacion}: {ex.Message}", ex);
            }
            return monto;
        }

        public async Task<(int, string)> ObetenerDatosReservaPorId(int IdReserva)
        {
            int IdDetalleProgramacion = 0;
            string Reserva = string.Empty;
            try
            {
                using (SqlConnection connection = GetConnection("TransportistaConnection"))
                {
                    await connection.OpenAsync();
                    using (SqlCommand command = new SqlCommand("SP_ObtenerDatosReservaPorId", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@IdReserva", IdReserva);
                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                IdDetalleProgramacion = Convert.ToInt32(reader[0]);
                                Reserva = reader[1].ToString() ?? string.Empty;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener datos de reserva por IdReserva {IdReserva}: {ex.Message}", ex);
            }
            return (IdDetalleProgramacion, Reserva);
        }

        public async Task<(string, string)> ObtenerPlacaHora(int IdProgramacion, int PuntoEmbarque)
        {
            string placa = string.Empty;
            string hora = string.Empty;
            try
            {
                using (SqlConnection connection = GetConnection("TransportistaConnection"))
                {
                    await connection.OpenAsync();
                    using (SqlCommand command = new SqlCommand("SP_ObtenerPlacaHora", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@IdProgramacion", IdProgramacion);
                        command.Parameters.AddWithValue("@IdPuntoEmbarque", PuntoEmbarque);
                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            

                            if (await reader.ReadAsync())
                            {
                                placa = reader[0].ToString() ?? string.Empty;
                                hora = reader[1].ToString() ?? string.Empty;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener placa y hora para programación {IdProgramacion}: {ex.Message}", ex);
            }
            return (placa, hora);
        }
    }
}
