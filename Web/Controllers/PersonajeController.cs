using Core.Entities;
using Core.Interfaces;
using Core.Interfaces.Entities;
using Core.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Services;

namespace Web.Crontrollers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonajeController : ControllerBase
    {
       
        private IPersonajeService _servicio;
        public PersonajeController(IPersonajeService personajeService) 
        {
            _servicio = personajeService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Personaje>>> Get()
        {

            var Personajes = await _servicio.GetAll();

            return Ok(Personajes);
        }


        // POST api/<PersonajeController>
        [HttpPost]
        public async Task<ActionResult<Personaje>> Post([FromBody] Personaje personaje)
        {
            try
            {
                var createdPersonaje =
                    await _servicio.Create(personaje);

                return Ok(createdPersonaje);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<IEnumerable<Personaje>>> Delete(int id)
        {

            try
            {
                await _servicio.Delete(id);
                return Ok("Personaje eliminado");

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }


        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Personaje>> Update(int id, string algo, [FromBody] Personaje personaje)
        {
            try
            {
                await _servicio.Update(id, personaje);
                return Ok("Personaje Actualizado!!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("Atacar")]
        public async Task<ActionResult<Habilidad>> Atacar(int idEnemigo, int idPersonaje)
        {
            try
            {
                var createdPersonaje =
                    await _servicio.Atacar(idEnemigo, idPersonaje);

                return Ok(createdPersonaje);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("AprenderHabilidad")]
        public async Task<ActionResult<Personaje>> AprenderHabilidad(int idPersonaje, int idHabilidad)
        {
            try
            {
                var createdPersonaje =
                    await _servicio.AprenderHabilidad(idPersonaje, idHabilidad);

                return Ok(createdPersonaje);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("Equipar")]
        public async Task<ActionResult<Objeto>> Equipar(int idObjeto, int idEquipo)
        {
            try
            {
                var createdPersonaje =
                    await _servicio.Equipar(idObjeto, idEquipo);

                return Ok(createdPersonaje);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("Desequipar")]
        public async Task<ActionResult<Equipo>> Desequipar(int id, int ObjToRemoveID)
        {
            try
            {
                var createdPersonaje =
                    await _servicio.Desequipar(id, ObjToRemoveID);

                return Ok(createdPersonaje);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("Moverse")]
        public async Task<ActionResult<Ubicacion>> Moverse(int id, int idUbicacion)
        {
            try
            {
                var createdPersonaje =
                    await _servicio.Desequipar(id, idUbicacion);

                return Ok(createdPersonaje);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("AceptarMision")]
        public async Task<ActionResult<Mision>> AceptarMision(int id, int idMision)
        {
            try
            {
                var createdPersonaje =
                    await _servicio.AceptarMision(id, idMision);

                return Ok(createdPersonaje);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("ProgresoMision")]
        public async Task<ActionResult<Mision>> ProgresoMision(int id, int idMision)
        {
            try
            {
                var createdPersonaje =
                    await _servicio.ProgresoMision(id, idMision);

                return Ok(createdPersonaje);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("CompletarMision")]
        public async Task<ActionResult<Mision>> CompletarMision(int id, int idMision)
        {
            try
            {
                var createdPersonaje =
                    await _servicio.CompletarMision(id, idMision);

                return Ok(createdPersonaje);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
