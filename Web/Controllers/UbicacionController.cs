using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Web.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UbicacionController : ControllerBase
    {
        private IUbicacionService _servicio;

        public UbicacionController(IUbicacionService ubicacionService)
        {
            _servicio = ubicacionService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Ubicacion>>> Get()
        {

            var Ubicacion = await _servicio.GetAll();

            return Ok(Ubicacion);
        }


        // POST api/<EquipoController>
        [HttpPost]
        public async Task<ActionResult<Ubicacion>> Post([FromBody] Ubicacion ubicacion)
        {
            try
            {
                var createdUbicacion =
                    await _servicio.Create(ubicacion);

                return Ok(createdUbicacion);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<IEnumerable<Ubicacion>>> Delete(int id)
        {

            try
            {
                await _servicio.Delete(id);
                return Ok("Ubicacion eliminada");

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Ubicacion>> Update(int id, string algo, [FromBody] Ubicacion ubicacion)
        {
            try
            {
                await _servicio.Update(id, ubicacion);
                return Ok("Ubicacion Actualizada!!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("Moverse")]
        public async Task<ActionResult<Ubicacion>> Moverse(int idPersonaje, int idUbicacion)
        {
            try
            {
                var createdUbicacion =
                    await _servicio.Moverse(idUbicacion, idPersonaje);

                return Ok(createdUbicacion);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
