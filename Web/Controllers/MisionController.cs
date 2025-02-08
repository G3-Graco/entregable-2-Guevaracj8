using Core.Entities;
using Core.Interfaces;
using Core.Interfaces.Entities;
using Core.Interfaces.Services;
using Core.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Services;

namespace Web.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class MisionController : ControllerBase
    {
        private IMisionService _servicio;

        public MisionController(IMisionService misionService)
        {
            _servicio = misionService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Mision>>> Get()
        {

            var Mision = await _servicio.GetAll();

            return Ok(Mision);
        }


        // POST api/<EquipoController>
        [HttpPost]
        public async Task<ActionResult<Mision>> Post([FromBody] Mision mision)
        {
            try
            {
                var createdMision =
                    await _servicio.Create(mision);

                return Ok(createdMision);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]

        public async Task<ActionResult<IEnumerable<Mision>>> Delete(int id)
        {
            try
            {
                await _servicio.Delete(id);
                return Ok("Mision eliminada");

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Mision>> Update(int id, string algo, [FromBody] Mision mision)
        {
            try
            {
                await _servicio.Update(id, mision);
                return Ok("Mision Actualizada!!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}