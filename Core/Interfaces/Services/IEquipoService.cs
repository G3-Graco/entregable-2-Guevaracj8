using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Core.Responses;

namespace Core.Interfaces.Services
{
    public interface IEquipoService : IBaseService<Equipo>
    {
        /*Task<Equipo> Equipar(int idObjeto, int idEquipo);

        Task<Equipo> Desequipar(int personajeToBeUpdatedId, int ObjToRemoveID);*/
    }
}