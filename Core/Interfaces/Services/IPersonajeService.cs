using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces.Services;
using Core.Responses;

namespace Core.Services
{
    public interface IPersonajeService : IBaseService<Personaje>
    {
        Task<Personaje> LevelUp(int personajeToBeUpdatedId);
        Task<AtaqueResponse> Atacar(int idEnemigo, int idPersonaje);
        Task<Personaje> AprenderHabilidad(int personajeToBeUpdatedId, int idHabilidad);
        Task<Personaje> Moverse(int idPersonaje, int idUbicacion);
        Task<Personaje> Equipar(int personajeToBeUpdatedId, int idObjeto);
        Task<Personaje> Desequipar(int personajeToBeUpdatedId, int idObjeto);
        Task<Personaje> AceptarMision(int idPersonaje, int idMision);
        Task<Personaje> ProgresoMision(int idPersonaje, int idMision);
        Task<Personaje> CompletarMision(int idPersonaje, int idMision);
    }
}