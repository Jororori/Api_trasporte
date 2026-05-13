using CapaServicio.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API_TRANSPORTISTE.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransportistaController : ControllerBase
    {
        private readonly ITransportistaService _service;

        public TransportistaController(ITransportistaService service)
        {
            _service = service;
        }

       

        [HttpGet("{id}/ciudades")]
        public async Task<IActionResult> ciudades(int id)
        {
            try
            {
                var transportista = await _service.ObtenerCuidadesPor(id);

                if (transportista == null || transportista.Count == 0)
                    return NotFound(new { mensaje = $"No hay ciudades para el transportista con ID {id}" });

                return Ok(new { exito = true, datos = transportista });
            }
            catch (Exception ex)
            {
                return BadRequest(new { exito = false, error = ex.Message });
            }
        }

        [HttpGet("{id}/rutas")]
        public async Task<IActionResult> rutas(int id)
        {
            try
            {
                var transportista = await _service.ObtenerRutasPor(id);
                if (transportista == null || transportista.Count == 0)
                    return NotFound(new { mensaje = $"No hay rutas para el transportista con ID {id}" });
                return Ok(new { exito = true, datos = transportista });
            }
            catch (Exception ex)
            {
                return BadRequest(new { exito = false, error = ex.Message });
            }
        }

        [HttpGet("{id}/buses")]
        public async Task<IActionResult> buses(int id)
        {
            try
            {
                var transportista = await _service.ObtenerBusesPor(id);
                if (transportista == null || transportista.Count == 0)
                    return NotFound(new { mensaje = $"No hay buses para el transportista con ID {id}" });
                return Ok(new { exito = true, datos = transportista });
            }
            catch (Exception ex)
            {
                return BadRequest(new { exito = false, error = ex.Message });
            }
        } /* unjfkv*/

        [HttpGet("{id}/programaciones")]
        public async Task<IActionResult> programaciones(int id, DateTime Fecha , int IdOrigen , int IdDestino)
        {
            try
            {
                var transportista = await _service.ObtenerProgramacionPor(id,Fecha , IdOrigen , IdDestino);
                if (transportista == null || transportista.Count == 0)
                    return NotFound(new { mensaje = $"No hay rutas disponibles" });
                return Ok(new { exito = true, datos = transportista });
            }
            catch (Exception ex)
            {
                return BadRequest(new { exito = false, error = ex.Message });
            }
            
        }
    }
}
