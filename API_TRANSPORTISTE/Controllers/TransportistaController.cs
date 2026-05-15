using API_TRANSPORTISTE.Configuration;
using CapaEntidades;
using CapaServicio.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API_TRANSPORTISTE.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
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
        public async Task<IActionResult> ciudades()
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

                return Ok(new { exito = true, datos = data });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { exito = false, error = ex.Message });
            }
        }

        [HttpGet("rutas")]
        public async Task<IActionResult> rutas()
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

               
                return Ok(new { exito = true, datos = transportista });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { exito = false, error = ex.Message });
            }
        }

        [HttpGet("buses")]
        public async Task<IActionResult> buses()
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
              
                return Ok(new { exito = true, datos = transportista });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { exito = false, error = ex.Message });
            }
        }

        [HttpGet("programaciones")]
        public async Task<IActionResult> programaciones(DateTime Fecha, int IdOrigen, int IdDestino)
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
               
                return Ok(new { exito = true, datos = transportista });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { exito = false, error = ex.Message });
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

                return Ok(new { exito = true, datos = listaAsientos });
            }
            catch (Exception ex)
            {
                return BadRequest(new { exito = false, error = ex.Message });
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
                return Ok(new { exito = true, datos = transportista });
            }
            catch (Exception ex)
            {
                return BadRequest(new { exito = false, error = ex.Message });
            }
        }
    }
}
