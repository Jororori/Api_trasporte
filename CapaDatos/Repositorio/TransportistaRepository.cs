using CapaDatos.Interfaces;
using CapaEntidades;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

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
                throw new Exception($"Error al obtener ciudades del transportista con id {id}: {ex.Message}", ex);
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
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener rutas del transportista con id {id}: {ex.Message}", ex);
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
                throw new Exception($"Error al obtener buses del transportista con id {id}: {ex.Message}", ex);
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
                                programaciones.Add(new Programaciones
                                {
                                    IdProgramacion = Convert.ToInt32(reader[0]),
                                    FechaProgramacion = Convert.ToDateTime(reader[4]),
                                    Origen = reader[1]?.ToString() ?? "",
                                    Destino = reader[2]?.ToString() ?? "",
                                    MarcaBus = reader[11]?.ToString() ?? "",
                                    ModeloBus = reader[10]?.ToString() ?? "",
                                    placaBus = reader[3]?.ToString() ?? "",
                                    IdConductor = Convert.ToInt32(reader[12]),
                                    Conductor = reader[13]?.ToString() ?? "",
                                    PrecioPiso1 = Convert.ToDecimal(reader[6]),
                                    PrecioPiso2 = Convert.ToDecimal(reader[7]),
                                    Estado = Convert.ToInt32(reader[9])
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
                            asientos.Add(new DetalleProgramacion
                            {
                                IdDetalleProgramacion = Convert.ToInt32(reader[5]),
                                ValorAsiento = reader[0]?.ToString(),
                                NumeroFila = Convert.ToInt32(reader[1]),
                                NumeroColumna = Convert.ToInt32(reader[2]),
                                NumeroPiso = Convert.ToInt32(reader[3]),
                                Estado = Convert.ToInt32(reader[4]),
                                PrecioPiso1 = Convert.ToDecimal(reader[8]), 
                                PrecioPiso2 = Convert.ToDecimal(reader[9])
                            });
                        }
                    }
                }
            }
            return asientos;
        }
    }
}
