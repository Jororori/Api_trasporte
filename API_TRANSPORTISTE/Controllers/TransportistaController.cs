using API_TRANSPORTISTE.Configuration;
using API_TRANSPORTISTE.Services;
using API_TRANSPORTISTE.Utilities;
using AspNetCoreGeneratedDocument;
using CapaEntidades;
using CapaServicio.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Security.Claims;
using System.Text.Json.Nodes;
using static System.Runtime.InteropServices.JavaScript.JSType;

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

                return Ok(new { status = 200, success = true, datos = data });
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

                var transportista = await _service.ObtenerRutasPor(idEmpresa);


                return Ok(new { status = 200, success = true, datos = transportista });
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

                return Ok(new { status = 200, success = true, datos = transportista });
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

        [HttpGet("programacionesV2")]
        public async Task<IActionResult> Programaciones(DateTime Fecha, int IdRuta)
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

                var transportista = await _service.ObtenerProgramacionPorRuta(idEmpresa, Fecha, IdRuta);

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


                var resultado = await _service.BloquearAsientoPor(IdDetalleProgramacion, null);
                if (resultado == "")
                    return BadRequest(new { success = false, mensaje = "No se Obtuvo Token Valido, Este asiento se esta usando por alguien mas" });
                return Ok(new { success = true, mensaje = "Token Generado Correctamente", Token = resultado , IdDetalle = IdDetalleProgramacion });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, error = ex.Message });
            }
        }

        [HttpPost("Asientos/BloqueoAsientoV2")]
        public async Task<IActionResult> BloquearAsientosPorId (int IdDetalleProgramacion, int? Tiempo)
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


                var resultado = await _service.BloquearAsientoPor(IdDetalleProgramacion, Tiempo);
                if (resultado == "")
                    return BadRequest(new { success = false, mensaje = "Este asiento se esta usando por alguien mas" });
                return Ok(new { success = true, mensaje = "Asiento Bloqueado Correctamente",  IdDetalle = IdDetalleProgramacion , FechaExpiracion = DateTime.Now.AddMinutes(Tiempo.HasValue ? Tiempo.Value : 15) });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, error = ex.Message });
            }
        }

        [HttpPost("Reservas/Extender")]
        public async Task<IActionResult> ExtenderReserva(string Token, int Tiempo)
        {
            try
            {
                var authHeader = HttpContext.Request.Headers["Authorization"].ToString();
                if (string.IsNullOrWhiteSpace(authHeader))
                    return Unauthorized(new { mensaje = "Token no enviado" });
                var tokenVeri = authHeader.Replace("Bearer ", "").Trim();
                var idEmpresa = ApiKeyConfig.ObtenerIdEmpresa(tokenVeri);
                if (idEmpresa == -1)
                    return Unauthorized();


                var resultado = await _service.ExtenderReserva(Token, Tiempo);

                return Ok(new { success = true, mensaje = "Tiempo de reserva extendido correctamente", FechaExpira = resultado });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, error = ex.Message });
            }
        }


        [HttpPost("Reservas/ExtenderV2")]
        public async Task<IActionResult> ExtenderReservaPorId(int IdDetalle, int Tiempo)
        {
            try
            {
                var authHeader = HttpContext.Request.Headers["Authorization"].ToString();
                if (string.IsNullOrWhiteSpace(authHeader))
                    return Unauthorized(new { mensaje = "Token no enviado" });
                var tokenVeri = authHeader.Replace("Bearer ", "").Trim();
                var idEmpresa = ApiKeyConfig.ObtenerIdEmpresa(tokenVeri);
                if (idEmpresa == -1)
                    return Unauthorized();


                var resultado = await _service.ExtenderReservaPorId(IdDetalle, Tiempo);

                return Ok(new { success = true, mensaje = "Tiempo de reserva extendido correctamente", FechaExpira = resultado });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, error = ex.Message });
            }
        }

        [HttpGet("Reservas/Estado")]
        public async Task<IActionResult> VerEstadoReserva(int IdDetalleProgramacion)
        {
            try
            {
                var authHeader = HttpContext.Request.Headers["Authorization"].ToString();
                if (string.IsNullOrWhiteSpace(authHeader))
                    return Unauthorized(new { mensaje = "Token no enviado" });
                var tokenVeri = authHeader.Replace("Bearer ", "").Trim();
                var idEmpresa = ApiKeyConfig.ObtenerIdEmpresa(tokenVeri);
                if (idEmpresa == -1)
                    return Unauthorized();

                var resultado = await _service.VerEstadoReserva(IdDetalleProgramacion);
                return Ok(new { success = true, Estado = resultado.Item1, Boleto = resultado.Item2 });
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

        [HttpDelete("Asientos/Liberar/{IdDetalle}")]
        public async Task<IActionResult> LiberarAsientoPorId(int IdDetalle)
        {
            try
            {
                if (IdDetalle < 0)
                {
                    return Unauthorized(new { mesaje = "Id no existente" });
                }

                var authHeader = HttpContext.Request.Headers["Authorization"].ToString();
                if (string.IsNullOrWhiteSpace(authHeader))
                    return Unauthorized(new { mensaje = "Token no enviado" });
                var tokenEmpresa = authHeader.Replace("Bearer ", "").Trim();
                var idEmpresa = ApiKeyConfig.ObtenerIdEmpresa(tokenEmpresa);
                if (idEmpresa == -1)
                    return Unauthorized();

                var resultado = await _service.LiberarAsientoPorId(IdDetalle);

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
                int TipoDocumento = 1;
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

                if (!string.IsNullOrEmpty(Venta.Ruc))
                {
                    TipoDocumentoVenta = 1;
                    TipoDocumento = 6;
                }
                else
                {

                    Venta.Ruc = "";
                    Venta.RazonSocial = "";
                    Venta.Direccion = "";
                }
                Venta.PrecioLetra = ConvertidorPrecioALetras.ConvertirPrecioALetras(Convert.ToString(Venta.Precio));

                int Crear = await _service.CrearReserva(Venta.TipoDocumento, Venta.NroDocumento, Venta.Pasajero, Venta.FechaNacimiento, Venta.Edad, Venta.Sexo, Venta.Ruc, Venta.RazonSocial, Venta.Direccion, TipoDocumentoVenta, FechaEmision, Venta.IdAgenciaOrigen, Venta.IdAgenciaDestino, "Contado", Venta.MedioDePago, Venta.Tarjeta, FechaVencimiento, 0.00, Venta.Observacion, 0, 2, 0, Venta.IdDetalleProgramacion, Convert.ToString(Venta.Precio), Venta.PrecioLetra, "0.000", Venta.HoraSalida, NuevaDataMenor, 0, Venta.Telefono);
                var Factura = new GenerarDocumenoVenta.ResultadoFactura();
                if (Crear != 0)
                {
                    var UsuarioLog = await _service.VerUsuarioPor(idEmpresa);

                    string Serie = await _service.ObtenerSeriePor(TipoDocumentoVenta, idEmpresa, Venta.IdAgenciaOrigen);

                    var Detalles = await _service.ObtenerProductoPor(idEmpresa);

                    if (string.IsNullOrEmpty(Venta.Ruc))
                    {
                        Venta.Ruc = Venta.NroDocumento;
                        Venta.RazonSocial = Venta.Pasajero;
                    }

                    Factura = GenerarDocumenoVenta.GenerarFactBol(Detalles, Venta.IncluidoIGV, UsuarioLog.Dni, UsuarioLog.Ruc, UsuarioLog.Pass, Serie, TipoDocumento, Venta.NroDocumento, Venta.Pasajero, Venta.FechaNacimiento, Venta.Edad, Venta.Sexo, Venta.Ruc, Venta.RazonSocial, Venta.Direccion, TipoDocumentoVenta, FechaEmision, Venta.IdAgenciaOrigen, Venta.IdAgenciaDestino, "Contado", Venta.MedioDePago, Venta.Tarjeta, FechaVencimiento, 0.00, Venta.Observacion, 0, 2, 0, Venta.IdDetalleProgramacion, Convert.ToString(Venta.Precio), Venta.PrecioLetra, "0.000", Venta.HoraSalida, NuevaDataMenor, 0, Venta.Telefono, Venta.PlacaBus, Venta.MedioDePago, Venta.IdAgenciaOrigen);
                }

                return Ok(new { success = true, mensaje = "Reserva creada correctamente", Codigo = Crear, Pdffact = Factura });

            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, error = ex.Message });
            }
        }

        [HttpPost("Reservas/Anular")]
        public async Task<IActionResult> AnularReserva(string numeroBoleto)
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

                string Tipo = "Anular";

                var Asientos = await _service.VerAsientosPorBoleto(int.Parse(numeroBoleto));

                foreach (var asiento in Asientos)
                {
                    int IdDetalleProgramacion = await _service.VerIdDetalle(asiento.Item2, asiento.Item1);

                    var RegistroReprog = await _service.ReprogramarPasajePor(IdDetalleProgramacion, Tipo.Trim(), idEmpresa, 0);

                    int IdDocVenta = await _service.ObtenerIdDocVentaPor(IdDetalleProgramacion);

                    var (IdUsuario, EstadoOSE) = await _service.BuscarDocumentoEnviadoOSE(IdDocVenta);

                    if (EstadoOSE != 0)
                    {
                        var Datos = await _service.ObtenerDatosDocVentaPor(IdDocVenta);
                        string nc = GenerarDocumenoVenta.GenerarNC(Datos);
                    }
                    else
                    {
                        _service.AnularDocumento(IdDocVenta, IdUsuario);
                    }

                    var success2 = _service.LimpiarPor(IdDetalleProgramacion);
                }
                

                return Ok(new { success = true, mensaje = "Reserva anulada correctamente" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, error = ex.Message });
            }
        }

        [HttpPost("Reservas/Posponer")]
        public async Task<IActionResult> PosponerReserva(PosponerTicket data)
        {
            try
            {
                RespuestaReserva respuesta = new RespuestaReserva();

                var authHeader = HttpContext.Request.Headers["Authorization"].ToString();
                if (string.IsNullOrWhiteSpace(authHeader))
                    return Unauthorized(new { mensaje = "Token no enviado" });
                var tokenEmpresa = authHeader.Replace("Bearer ", "").Trim();
                var idEmpresa = ApiKeyConfig.ObtenerIdEmpresa(tokenEmpresa);
                if (idEmpresa == -1)
                    return Unauthorized();
                string Tipo = "Posponer";

                var Asientos = await _service.VerAsientosPorBoleto(int.Parse(data.NumeroBoleto));
                int index = 0;

                foreach (var asiento in Asientos)
                {
                    int IdDetalleProgramacion = await _service.VerIdDetalle(asiento.Item2, asiento.Item1);
                    var RegistroReprog = await _service.ReprogramarPasajePor(IdDetalleProgramacion, Tipo.Trim(), idEmpresa, 0);

                    decimal MontoAnterior = await _service.ObtenerMontoAnteriorPor(IdDetalleProgramacion);
                    int IdDocVenta = await _service.ObtenerIdDocVentaPor(IdDetalleProgramacion);

                    data.Pasajeros[index].MontoAnteriorP = MontoAnterior;
                    data.Pasajeros[index].IdDocVenta = IdDocVenta;
                    data.Pasajeros[index].IdDetalleProgAnterior = IdDetalleProgramacion;

                    index++;
                }


                foreach(var x in data.Pasajeros)
                {
                    var MontoActual = await _service.ObtenerMontosPor(data.IdProgramacion , x.Asiento, data.IdDestino);

                    decimal MontoExtra = MontoActual - x.MontoAnteriorP;
                    if (MontoExtra < 0)
                    {
                        MontoExtra = 0;
                    }

                    int IdDetalleProgramacion = await _service.VerIdDetalle(data.IdProgramacion, x.Asiento);

                    var Bloqueo = await _service.BloquearAsientoPor(IdDetalleProgramacion, null);

                    respuesta.DatosReserva ??= new List<Data>();

                    respuesta.DatosReserva.Add(new Data
                    {
                        Asiento = x.Asiento,
                        NuevoMonto = MontoActual,
                        MontoExtra = MontoExtra
                    });
                }

                string JsonData = System.Text.Json.JsonSerializer.Serialize(data);

                int GuardarData = await _service.GuardarDataPosponer(JsonData, int.Parse(data.NumeroBoleto));

                respuesta.IdReseva = GuardarData.ToString("D8");

                return Ok(new { success = true, mensaje = "Reserva pospuesta correctamente", Respuesta = respuesta });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, error = ex.Message });
            }
        }

        [HttpPost("Reservas/ReprogramarTicket")]
        public async Task<IActionResult> ReprogramarReserva(int IdReserva )
        {
            var authHeader = HttpContext.Request.Headers["Authorization"].ToString();
            if (string.IsNullOrWhiteSpace(authHeader))
                return Unauthorized(new { mensaje = "Token no enviado" });
            var tokenEmpresa = authHeader.Replace("Bearer ", "").Trim();
            var idEmpresa = ApiKeyConfig.ObtenerIdEmpresa(tokenEmpresa);
            if (idEmpresa == -1)
                return Unauthorized();

            int TipoDocumentoVenta = 3;
            int TipoDocumento = 1;
            DateTime FechaEmision = DateTime.Now;
            DateTime? FechaVencimiento = null;

            PosponerTicket PosTikect = new PosponerTicket();
            var documentosElectronicos = new List<DocumentoElecReserva>();

            var DatosReserva = await _service.ObetenerDatosReservaPorId(IdReserva);
            if(string.IsNullOrEmpty(DatosReserva.Item2))
            {
                return BadRequest(new { success = false, error = "No se encontraron datos para la reserva especificada" });
            } else
            {
                 PosTikect = System.Text.Json.JsonSerializer.Deserialize<PosponerTicket>(DatosReserva.Item2);
            }
            

            foreach (var venta in PosTikect.Pasajeros)
            {
                var MontoActual = await _service.ObtenerMontosPor(PosTikect.IdProgramacion, venta.Asiento, PosTikect.IdDestino);

                decimal MontoExtra = MontoActual - venta.MontoAnteriorP;
                if (MontoExtra < 0)
                {
                    MontoExtra = 0;
                }

                int IdDetalleProgramacion = await _service.VerIdDetalle(PosTikect.IdProgramacion, venta.Asiento);

                var adicionales = await _service.ObtenerPlacaHora(PosTikect.IdProgramacion, PosTikect.PuntoEmbarque);

                if (string.IsNullOrWhiteSpace(PosTikect.Telefono))
                    PosTikect.Telefono = "-";


                if (!string.IsNullOrEmpty(venta.Ruc))
                {
                    TipoDocumentoVenta = 1;
                    TipoDocumento = 6;
                }
                else
                {

                    venta.Ruc = "";
                    venta.RazonSocial = "";
                    venta.Direccion = "";
                }
                string PrecioLetra = "";
                string Observacion = null;
                Decimal Precio = 0.00m;
                if (MontoExtra > 0)
                {
                    PrecioLetra = ConvertidorPrecioALetras.ConvertirPrecioALetras(Convert.ToString(MontoExtra));
                    Observacion = "Monto Extra por Reprogramacion: " + venta.MontoAnteriorP.ToString("F2") +"de Factura N° " + venta.SerieBoleto;
                    Precio = MontoExtra;
                }
                else
                {
                    PrecioLetra = ConvertidorPrecioALetras.ConvertirPrecioALetras(Convert.ToString(venta.MontoAnteriorP));
                    Observacion = " Pasaje Reprogramado";
                    Precio = venta.MontoAnteriorP;
                }
                

                int Crear = await _service.CrearReserva(venta.TipoDocumento, venta.NroDocumento, venta.Pasajero, venta.FechaNacimiento, 0, venta.Sexo, venta.Ruc, venta.RazonSocial, venta.Direccion, TipoDocumentoVenta, FechaEmision, PosTikect.IdOrigen, PosTikect.IdDestino, "Contado", PosTikect.MedioDePago, PosTikect.Tarjeta, FechaVencimiento, 0.00, Observacion, 0, 2, 0, IdDetalleProgramacion, Convert.ToString(Precio), PrecioLetra, "0.000", adicionales.Item2, "", PosTikect.PuntoEmbarque, PosTikect.Telefono);

                var Factura = new GenerarDocumenoVenta.ResultadoFactura();
                if (Crear != 0)
                {
                    var UsuarioLog = await _service.VerUsuarioPor(idEmpresa);

                    string Serie = await _service.ObtenerSeriePor(TipoDocumentoVenta, idEmpresa, PosTikect.IdOrigen);

                    var Detalles = await _service.ObtenerProductoPor(idEmpresa);

                    if (string.IsNullOrEmpty(venta.Ruc))
                    {
                        venta.Ruc = venta.NroDocumento;
                        venta.RazonSocial = venta.Pasajero;
                    }
                    if(MontoExtra > 0)
                    {
                        Factura = GenerarDocumenoVenta.GenerarFactBol(Detalles, 1, UsuarioLog.Dni, UsuarioLog.Ruc, UsuarioLog.Pass, Serie, TipoDocumento, venta.NroDocumento, venta.Pasajero, venta.FechaNacimiento,0, venta.Sexo, venta.Ruc, venta.RazonSocial, venta.Direccion, TipoDocumentoVenta, FechaEmision, PosTikect.PuntoEmbarque , PosTikect.IdDestino, "Contado", PosTikect.MedioDePago, PosTikect.Tarjeta, FechaVencimiento, 0.00, Observacion, 0, 2, 0, IdDetalleProgramacion, Convert.ToString(Precio), PrecioLetra, "0.000", adicionales.Item2, "", 0, PosTikect.Telefono, adicionales.Item1, PosTikect.MedioDePago, PosTikect.IdOrigen);
                    }
                    else
                    {
                        Factura = GenerarDocumenoVenta.RegenerarPdf(venta.IdDocVenta, TipoDocumento, IdDetalleProgramacion);
                    }

                    var limpiar = await _service.LimpiarPor(venta.IdDetalleProgAnterior);
                    documentosElectronicos.Add(new DocumentoElecReserva
                    {
                        Asiento = venta.Asiento,
                        Numero = Factura.NumeroDocumento,
                        Pdf = Factura.PdfBytes,
                        NuevoMonto = MontoActual,
                        MontoExtra = MontoExtra
                    });


                }
            }

            var asientos = string.Join(",", documentosElectronicos.Select(x => x.Asiento));

            int Guardar = await _service.GuardarDetalles(PosTikect.IdProgramacion, asientos);

            string NroBoleto = Guardar.ToString("D8");

            return Ok(new { success = true, mensaje = "Reserva reprogramada correctamente", NumeroBoleto = NroBoleto, documentosElectronicos = documentosElectronicos });
        }


        [HttpPost("ReservasLista")]
        public async Task<IActionResult> CrearReservaMasiva(Boleto boleto)
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
                int TipoDocumento = 1;
                string NuevaDataMenor = "";

                DateTime FechaEmision = DateTime.Now;
                DateTime? FechaVencimiento = null;
                var documentosElectronicos = new List<DocumentoElectronicoResponse>();

                foreach (var Venta in boleto.venta)
                {
                    var adicionales = await _service.ObtenerPlacaHora(boleto.IdProgramacion, Venta.PuntoEmbarque);

                    int IdDetalleProgramacion = await _service.VerIdDetalle(boleto.IdProgramacion, Venta.Asiento);

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

                    if (!string.IsNullOrEmpty(Venta.Ruc))
                    {
                        TipoDocumentoVenta = 1;
                        TipoDocumento = 6;
                    }
                    else
                    {

                        Venta.Ruc = "";
                        Venta.RazonSocial = "";
                        Venta.Direccion = "";
                    }
                    Venta.PrecioLetra = ConvertidorPrecioALetras.ConvertirPrecioALetras(Convert.ToString(Venta.Precio));

                    int Crear = await _service.CrearReserva(Venta.TipoDocumento, Venta.NroDocumento, Venta.Pasajero, Venta.FechaNacimiento, Venta.Edad, Venta.Sexo, Venta.Ruc, Venta.RazonSocial, Venta.Direccion, TipoDocumentoVenta, FechaEmision, Venta.IdAgenciaOrigen, Venta.IdAgenciaDestino, "Contado", Venta.MedioDePago, Venta.Tarjeta, FechaVencimiento, 0.00, Venta.Observacion, 0, 2, 0, IdDetalleProgramacion, Convert.ToString(Venta.Precio), Venta.PrecioLetra, "0.000", adicionales.Item2, NuevaDataMenor, Venta.PuntoEmbarque, Venta.Telefono);
                    
                    var Factura = new GenerarDocumenoVenta.ResultadoFactura();
                    if (Crear != 0)
                    {
                        var UsuarioLog = await _service.VerUsuarioPor(idEmpresa);

                        string Serie = await _service.ObtenerSeriePor(TipoDocumentoVenta, idEmpresa, Venta.IdAgenciaOrigen);

                        var Detalles = await _service.ObtenerProductoPor(idEmpresa);

                        if (string.IsNullOrEmpty(Venta.Ruc))
                        {
                            Venta.Ruc = Venta.NroDocumento;
                            Venta.RazonSocial = Venta.Pasajero;
                        }

                        Factura = GenerarDocumenoVenta.GenerarFactBol(Detalles, Venta.IncluidoIGV, UsuarioLog.Dni, UsuarioLog.Ruc, UsuarioLog.Pass, Serie, TipoDocumento, Venta.NroDocumento, Venta.Pasajero, Venta.FechaNacimiento, Venta.Edad, Venta.Sexo, Venta.Ruc, Venta.RazonSocial, Venta.Direccion, TipoDocumentoVenta, FechaEmision, Venta.IdAgenciaOrigen, Venta.IdAgenciaDestino, "Contado", Venta.MedioDePago, Venta.Tarjeta, FechaVencimiento, 0.00, Venta.Observacion, 0, 2, 0, IdDetalleProgramacion, Convert.ToString(Venta.Precio), Venta.PrecioLetra, "0.000", adicionales.Item2, NuevaDataMenor, 0, Venta.Telefono, adicionales.Item1, Venta.MedioDePago, Venta.IdAgenciaOrigen);

                        documentosElectronicos.Add(new DocumentoElectronicoResponse
                        {
                            Asiento = Venta.Asiento,
                            Numero = Factura.NumeroDocumento,
                            Pdf = Factura.PdfBytes
                        });
                    }
                }
                var asientos = string.Join(",",documentosElectronicos.Select(x => x.Asiento));

                int Guardar = await _service.GuardarDetalles(boleto.IdProgramacion, asientos);

                string NroBoleto = Guardar.ToString("D8");

                return Ok(new { success = true, mensaje = "Reserva creada correctamente", NumeroBoleto = NroBoleto, documentosElectronicos = documentosElectronicos });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, error = ex.Message });
            }
        }

    }
}
