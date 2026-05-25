 using API_TRANSPORTISTE.Configuration;
using API_TRANSPORTISTE.Utilities;
using CapaEntidades;
using CapaServicio.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text.Json.Nodes;

namespace API_TRANSPORTISTE.Controllers
{
    [ApiController]
    [Route("v1/auth/[controller]")]
    [Authorize]
    public class TransportistaController : ControllerBase
    {
        private readonly ITransportistaService _service;

        public TransportistaController(ITransportistaService service)
        {
            _service = service;
        }

        /// <summary>
        /// Obtiene el IdEmpresa del token actual
        /// </summary>
        private int ObtenerIdEmpresaDelToken()
        {
            var claim = User.FindFirst("idEmpresa");
            if (claim != null && int.TryParse(claim.Value, out var idEmpresa))
            {
                return idEmpresa;
            }
            return -1;
        }



        [HttpGet("ciudades")]
        public async Task<IActionResult> Ciudades()
        {
            try
            {
                var authHeader = HttpContext.Request.Headers["Authorization"].ToString();

                if (string.IsNullOrWhiteSpace(authHeader))
                    return Unauthorized(new { mensaje = "Token no enviado" });

                var token = authHeader.Replace("Bearer ", "").Trim();

                var idEmpresa = ApiKeyConfig.ObtenerIdEmpresa(token);

                if (idEmpresa == -1)
                    return Unauthorized(new { mensaje = "Token inválido" });

                var data = await _service.ObtenerCuidadesPor(idEmpresa);

                return Ok(new { status = 200 , success = true, datos = data });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, error = ex.Message });
            }
        }
            
        [HttpGet("rutas")]
        public async Task<IActionResult> Rutas()
        {
            try
            {
                var authHeader = HttpContext.Request.Headers["Authorization"].ToString();

                if (string.IsNullOrWhiteSpace(authHeader))
                    return Unauthorized(new { mensaje = "Token no enviado" });

                var token = authHeader.Replace("Bearer ", "").Trim();

                var idEmpresa = ApiKeyConfig.ObtenerIdEmpresa(token);
                if (idEmpresa == -1)
                    return Unauthorized();

                var transportista = await _service.ObtenerRutasPor(0);


                return Ok(new {status = 200,  success = true, datos = transportista });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, error = ex.Message });
            }
        }

        [HttpGet("buses")]
        public async Task<IActionResult> Buses()
        {
            try
            {
                var authHeader = HttpContext.Request.Headers["Authorization"].ToString();

                if (string.IsNullOrWhiteSpace(authHeader))
                    return Unauthorized(new { mensaje = "Token no enviado" });

                var token = authHeader.Replace("Bearer ", "").Trim();

                var idEmpresa = ApiKeyConfig.ObtenerIdEmpresa(token);
                if (idEmpresa == -1)
                    return Unauthorized();

                var transportista = await _service.ObtenerBusesPor(idEmpresa);

                return Ok(new { status = 200,  success = true, datos = transportista });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, error = ex.Message });
            }
        }

        [HttpGet("programaciones")]
        public async Task<IActionResult> Programaciones(DateTime Fecha, int IdOrigen, int IdDestino)
        {
            try
            {
                var authHeader = HttpContext.Request.Headers["Authorization"].ToString();

                if (string.IsNullOrWhiteSpace(authHeader))
                    return Unauthorized(new { mensaje = "Token no enviado" });

                var token = authHeader.Replace("Bearer ", "").Trim();

                var idEmpresa = ApiKeyConfig.ObtenerIdEmpresa(token);
                if (idEmpresa == -1)
                    return Unauthorized();

                var transportista = await _service.ObtenerProgramacionPor(idEmpresa, Fecha, IdOrigen, IdDestino);

                return Ok(new { success = true, datos = transportista });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, error = ex.Message });
            }
        }



        [HttpGet("TipoAsiento")]
        public async Task<IActionResult> TipoAsiento()
        {
            try
            {
                var listaAsientos = new List<TipoAsiento>
                {
                    new TipoAsiento
                    {
                        IdTipoAsiento = 1,
                        TiposAsiento = "140",
                        Precio = 0.00m
                    },
                    new TipoAsiento
                    {
                        IdTipoAsiento = 2,
                        TiposAsiento = "160",
                        Precio = 0.00m
                    }
                };//la lista de los tipos de haciendo es parte de la logica de datos, no deberia estar aca. Arregla eso. Hay que mantener el orden de de estructuras.

                return Ok(new { success = true, datos = listaAsientos });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, error = ex.Message });
            }
        }

        [HttpGet("programaciones/{id}/asientos")]
        public async Task<IActionResult> Asientos(int id)
        {
            try
            {
                var transportista = await _service.ObtenerAsientosPor(id);
                if (transportista == null || transportista.Count == 0)
                    return NotFound(new { mensaje = $"No hay asientos disponibles" });
                return Ok(new { success = true, datos = transportista });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, error = ex.Message });
            }
        }

        [HttpPost("Asientos/BloqueoAsiento")]
        public async Task<IActionResult> BloquearAsientos(int IdDetalleProgramacion)
        {
            try
            {
                var authHeader = HttpContext.Request.Headers["Authorization"].ToString();
                if (string.IsNullOrWhiteSpace(authHeader))
                    return Unauthorized(new { mensaje = "Token no enviado" });
                var token = authHeader.Replace("Bearer ", "").Trim();
                var idEmpresa = ApiKeyConfig.ObtenerIdEmpresa(token);
                if (idEmpresa == -1)
                    return Unauthorized();


                var resultado = await _service.BloquearAsientoPor(IdDetalleProgramacion);
                if (resultado == "" )
                    return BadRequest(new { success = false,  mensaje = "No se Obtuvo Token Valido, Este asiento se esta usando por alguien mas"  });
                return Ok(new { success = true, mensaje = "Token Generado Correctamente", Token = resultado });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, error = ex.Message });
            }
        }

        [HttpDelete("Asientos/Bloquear/{token}")]
        public async Task<IActionResult> LiberarAsientoPorToken(string token)
        {
            try
            {
                if (token == null)
                {
                    return Unauthorized(new { mesaje = "token no existente" });
                }

                var authHeader = HttpContext.Request.Headers["Authorization"].ToString();
                if (string.IsNullOrWhiteSpace(authHeader))
                    return Unauthorized(new { mensaje = "Token no enviado" });
                var tokenEmpresa = authHeader.Replace("Bearer ", "").Trim();
                var idEmpresa = ApiKeyConfig.ObtenerIdEmpresa(tokenEmpresa);
                if (idEmpresa == -1)
                    return Unauthorized();

                var resultado = await _service.LiberarAsientoPorToken(token);

                return Ok(new { success = true, mensaje = "Asiento liberado correctamente" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, error = ex.Message });

            }
        }

        [HttpPost("Reservas")]
        public async Task<IActionResult> CrearReserva(DetalleReserva Venta)
        {
            try
            {
                var authHeader = HttpContext.Request.Headers["Authorization"].ToString();
                if (string.IsNullOrWhiteSpace(authHeader))
                    return Unauthorized(new { mensaje = "Token no enviado" });
                var tokenEmpresa = authHeader.Replace("Bearer ", "").Trim();
                var idEmpresa = ApiKeyConfig.ObtenerIdEmpresa(tokenEmpresa);
                if (idEmpresa == -1)
                    return Unauthorized();

                int TipoDocumentoVenta = 3;
                string NuevaDataMenor = "";

                DateTime FechaEmision = DateTime.Now;
                DateTime? FechaVencimiento = null;

                if (string.IsNullOrWhiteSpace(Venta.Menor))
                    Venta.Menor = "";

                if (string.IsNullOrWhiteSpace(Venta.Telefono))
                    Venta.Telefono = "-";


                if (string.IsNullOrWhiteSpace(Venta.Tarjeta))
                    Venta.Tarjeta = "";
                if (string.IsNullOrEmpty(Venta.Observacion))
                    Venta.Observacion = "";

                if (Venta.Menor != "")
                {
                    JsonNode datosMenor = JsonNode.Parse(Venta.Menor);
                    datosMenor["IdClienteNatular"] = 0;

                    NuevaDataMenor = datosMenor.ToJsonString();
                }
                
                if (Venta.Ruc.Length == 11)
                {
                    TipoDocumentoVenta = 1;
                }

                Venta.PrecioLetra = ConvertidorPrecioALetras.ConvertirPrecioALetras(Convert.ToString(Venta.Precio));

                var Crear = await _service.CrearReserva(Venta.TipoDocumento, Venta.NroDocumento, Venta.Pasajero, Venta.FechaNacimiento, Venta.Edad, Venta.Sexo, Venta.Ruc, Venta.RazonSocial, Venta.Direccion, TipoDocumentoVenta, FechaEmision, Venta.IdAgenciaOrigen, Venta.IdAgenciaDestino, "Contado", Venta.MedioDePago, Venta.Tarjeta, FechaVencimiento, 0.00, Venta.Observacion, 0, 2, 0, Venta.IdDetalleProgramacion, Convert.ToString(Venta.Precio), Venta.PrecioLetra, "0.000", Venta.HoraSalida, NuevaDataMenor, 0, Venta.Telefono);

                return Ok(new { success = true, mensaje = "Reserva creada correctamente", Codigo = Crear });

            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, error = ex.Message });
            }
        }
    }
}
