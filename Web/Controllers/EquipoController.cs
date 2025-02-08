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
    public class EquipoController : ControllerBase
    {
        private IEquipoService _servicio;

        public EquipoController(IEquipoService equipoService)
        {
            _servicio = equipoService;
        }

         [HttpGet]
        public async Task<ActionResult<IEnumerable<Equipo>>> Get()
        {

            var Equipo = await _servicio.GetAll();

            return Ok(Equipo);
        }


        // POST api/<EquipoController>
        [HttpPost]
        public async Task<ActionResult<Equipo>> Post([FromBody] Equipo equipo)
        {
            try
            {
                var createdEquipo =
                    await _servicio.Create(equipo);

                return Ok(createdEquipo);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<IEnumerable<Equipo>>> Delete(int id)
        {

            try
            {
                await _servicio.Delete(id);
                return Ok("Equipo eliminado");

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Equipo>> Update(int id, string algo, [FromBody] Equipo equipo)
        {
            try
            {
                await _servicio.Update(id, equipo);
                return Ok("Equipo Actualizado!!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /*[HttpPost("Equipar")]
        public async Task<ActionResult<Objeto>> Equipar(int idObjeto, int idEquipo)
        {
            try
            {
                var createdEquipo =
                    await _servicio.Equipar(idObjeto, idEquipo);

                return Ok(createdEquipo);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }*/

        /*[HttpPost("Desequipar")]
        public async Task<ActionResult<Equipo>> Desequipar(int id, int ObjToRemoveID)
        {
            try
            {
                var createdEquipo =
                    await _servicio.Desequipar(id, ObjToRemoveID);

                return Ok(createdEquipo);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }*/
    }
}