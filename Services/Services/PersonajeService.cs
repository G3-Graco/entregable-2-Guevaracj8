using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces;
using Core.Services;
using Core.Responses;
using Services.Validators;

namespace Services.Services
{
    public class PersonajeService : IPersonajeService
    {
        private readonly IUnitOfWork _unitOfWork;
        public PersonajeService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        

        public async Task<Personaje> Create(Personaje newPersonaje)
        {
            PersonajeValidators validator = new();


            var validationResult = await validator.ValidateAsync(newPersonaje);
            if (validationResult.IsValid)
            {
                //newPersonaje.tipo = await _unitOfWork.TipoPersonajeRepository.GetByIdAsync(newPersonaje.tipoId);

                //if(newPersonaje.tipo != null ) {

                await _unitOfWork.PersonajeRepository.AddAsync(newPersonaje);
                await _unitOfWork.CommitAsync();
                //}
                //else {
                //    throw new ArgumentException("El tipo de personaje no existe");
                //}
            }
            else
            {
                
                throw new ArgumentException(validationResult.Errors[0].ErrorMessage.ToString());
            }

            return newPersonaje;
        }

        public async Task Delete(int PersonajeId)
        {
            Personaje Personaje = await _unitOfWork.PersonajeRepository.GetByIdAsync(PersonajeId);
            if(Personaje != null)
            {
                _unitOfWork.PersonajeRepository.Remove(Personaje);
                await _unitOfWork.CommitAsync();
            }
            else
            {
                throw new Exception("El personaje no existe");
            }
        }

        public async Task<IEnumerable<Personaje>> GetAll()
        {
            return await _unitOfWork.PersonajeRepository.GetAllAsync();
        }

        public async Task<Personaje> GetById(int id)
        {
            return await _unitOfWork.PersonajeRepository.GetByIdAsync(id);
        }

        //private Personaje ValidarPersonaje(int id)
        //{
        //    Personaje PersonajeToBeUpdated = await _unitOfWork.PersonajeRepository.GetByIdAsync(PersonajeToBeUpdatedId);

        //    if (PersonajeToBeUpdated == null)
        //        throw new ArgumentException("Invalid Personaje ID while updating");
        //    else
        //        return PersonajeToBeUpdated;
        //}

        public async Task<Personaje> Update(int PersonajeToBeUpdatedId, Personaje newPersonajeValues)
        {
            PersonajeActualizarValidators PersonajeValidator = new();
            
            var validationResult = await PersonajeValidator.ValidateAsync(newPersonajeValues);
            if (!validationResult.IsValid)
                throw new ArgumentException(validationResult.Errors.ToString());

            Personaje PersonajeToBeUpdated = await _unitOfWork.PersonajeRepository.GetByIdAsync(PersonajeToBeUpdatedId);

            if (PersonajeToBeUpdated == null)
                throw new ArgumentException("Invalid Personaje ID while updating");

            //PersonajeToBeUpdated.tipoId = newPersonajeValues.tipoId;
            PersonajeToBeUpdated.nombre = newPersonajeValues.nombre;

            await _unitOfWork.CommitAsync();

            return await _unitOfWork.PersonajeRepository.GetByIdAsync(PersonajeToBeUpdatedId);
        }

        public async Task<Personaje> LevelUp(int PersonajeToBeUpdatedId)
        {
            PersonajeValidators PersonajeValidator = new();
            Personaje PersonajeToBeUpdated = await _unitOfWork.PersonajeRepository.GetByIdAsync(PersonajeToBeUpdatedId);
            if (PersonajeToBeUpdated == null)
                throw new ArgumentException("Invalid Personaje ID while updating");

            PersonajeToBeUpdated.experiencia = 0;
            PersonajeToBeUpdated.nivel += 1;
            PersonajeToBeUpdated.salud = PersonajeToBeUpdated.nivel * (new Random().Next(1,5) + 50);
            PersonajeToBeUpdated.energia = PersonajeToBeUpdated.nivel * 50;
            PersonajeToBeUpdated.inteligencia += new Random().Next(1,5);
            PersonajeToBeUpdated.agilidad += new Random().Next(1,5);
            PersonajeToBeUpdated.salud += new Random().Next(1,5);
            PersonajeToBeUpdated.inteligencia += new Random().Next(1,5);
            PersonajeToBeUpdated.fuerza += new Random().Next(1,5);
            
            var validationResult = await PersonajeValidator.ValidateAsync(PersonajeToBeUpdated);
            if (!validationResult.IsValid)
                throw new ArgumentException(validationResult.Errors.ToString());

            await _unitOfWork.CommitAsync();

            return await _unitOfWork.PersonajeRepository.GetByIdAsync(PersonajeToBeUpdatedId);
        }

        public async Task<AtaqueResponse> Atacar(int idEnemigo, int idPersonaje){

            EnemigoService _servicioEnemigo = new EnemigoService(_unitOfWork);

            AtaqueResponse response = new AtaqueResponse();
            // buscar info enemico 
            var enemigo = await _unitOfWork.EnemigoRepository.GetByIdAsync(idEnemigo);
            var personaje = await _unitOfWork.PersonajeRepository.GetByIdAsync(idPersonaje);

            if(Math.Abs(enemigo.nivelAmenaza - personaje.nivel) <= 5){
                int puntosDañoEnemigo = 0;
                int puntosDaño = (int)(
                                    (20 + personaje.fuerza) * 1.5)
                                    - (int)(10 + enemigo.defensa * 1.75);
                enemigo.salud -=  puntosDaño;

                if(enemigo.salud > 0) {
                    puntosDañoEnemigo = (int)(
                                     (20 + enemigo.fuerza) * 1.5)
                                      - (int)(10 + personaje.defensa * 1.75);
                    personaje.salud -= puntosDañoEnemigo;

                }else{
                    personaje.experiencia += (enemigo.nivelAmenaza * 2);

                    if(personaje.nivel * 10 < personaje.experiencia){
                        await LevelUp(personaje.id);
                    }
                }
                response.personaje = personaje;
                response.enemigo = enemigo;
                response.mensaje = $"{personaje.nombre} atacó e inflingio {puntosDaño} a {enemigo.nombre} y sufrio un contraataque de {puntosDañoEnemigo}";
                await _unitOfWork.CommitAsync();
            }
            else{
                response.mensaje = "No es posible atacar al enemigo ";
            }


            return response;
        }

        public async Task<Personaje> AprenderHabilidad(int personajeToBeUpdatedId, int idHabilidad)
        {
            Personaje PersonajeToBeUpdated = await _unitOfWork.PersonajeRepository.GetByIdAsync(personajeToBeUpdatedId);
            Habilidad habilidad = await _unitOfWork.HabilidadRepository.GetByIdAsync(idHabilidad);
            if (PersonajeToBeUpdated == null)
                throw new ArgumentException("Invalid Personaje ID while updating");


            if (PersonajeToBeUpdated.habilidades.Where(Hab => Hab.id == idHabilidad).ToList().Count > 0)
                throw new ArgumentException("No se puede aprender la misma habilidad dos veces");


            PersonajeToBeUpdated.habilidades.Add(habilidad); //

            await _unitOfWork.CommitAsync();
            return PersonajeToBeUpdated;

        }

        public async Task<Personaje> Moverse(int personajeToBeUpdatedId, int idUbicacion){

            Personaje PersonajeToBeUpdated = await _unitOfWork.PersonajeRepository.GetByIdAsync(personajeToBeUpdatedId);
            Ubicacion ubicacion = await _unitOfWork.UbicacionRepository.GetByIdAsync(idUbicacion);

            if (PersonajeToBeUpdated == null)
                throw new ArgumentException("Invalid Personaje ID while updating");

            if (ubicacion == null)
                throw new ArgumentException("Invalid Ubicacion ID while updating");

            if (PersonajeToBeUpdated.ubicacion == ubicacion.id.ToString())
                throw new ArgumentException("El personaje ya esta en esta ubicación.");

            PersonajeToBeUpdated.ubicacion = "";
            PersonajeToBeUpdated.ubicacion = ubicacion.id.ToString();

            await _unitOfWork.CommitAsync();

            return PersonajeToBeUpdated;
        }
        public async Task<Personaje> Equipar(int PersonajeToBeUpdatedId, int idObjeto)
        {
            Personaje PersonajeToBeUpdated = await _unitOfWork.PersonajeRepository.GetByIdAsync(PersonajeToBeUpdatedId);
            Objeto objeto = await _unitOfWork.ObjetoRepository.GetByIdAsync(idObjeto);

            if (PersonajeToBeUpdated == null)
                throw new ArgumentException("Equipo no encontrado.");
            
            if (!PersonajeToBeUpdated.equipo.Any())
                throw new ArgumentException("El personaje no tiene un equipo asociado.");

            if (PersonajeToBeUpdated.equipo.Any(obj => obj.casco != null))
            {
                throw new ArgumentException("Solo se puede equipar un objeto de tipo casco en el slot de casco.");
            }
            else if (PersonajeToBeUpdated.equipo.Any(obj => obj.arma1 != null))
            {
                throw new ArgumentException("Solo se puede equipar un objeto de tipo arma en el slot de arma (Principal?).");
            }
            else if (PersonajeToBeUpdated.equipo.Any(obj => obj.arma2 != null))
            {
                throw new ArgumentException("Solo se puede equipar un objeto de tipo arma en el slot de arma (Secundaria?).");
            }
            else if (PersonajeToBeUpdated.equipo.Any(obj => obj.grebas != null))
            {
                throw new ArgumentException("Solo se puede equipar un objeto de tipo grebas en el slot de grebas.");
            }
            else if (PersonajeToBeUpdated.equipo.Any(obj => obj.guanteletes != null))
            {
                throw new ArgumentException("Solo se puede equipar un objeto de tipo guanteletes en el slot de guanteletes.");
            }
            else if (PersonajeToBeUpdated.equipo.Any(obj => obj.armadura != null))
            {
                throw new ArgumentException("Solo se puede equipar un objeto de tipo armadura en el slot de armadura.");
            }


            if (PersonajeToBeUpdated.equipo.Any(obj => obj.casco == null) && objeto.tipo == "casco")
            {
                Equipo nuevoEquipo = new Equipo
                {
                    casco = objeto.id.ToString()
                };
                PersonajeToBeUpdated.equipo.Add(nuevoEquipo);
            }
            else if (PersonajeToBeUpdated.equipo.Any(obj => obj.arma1 == null) && objeto.tipo == "arma")
            {
                Equipo nuevoEquipo = new Equipo
                {
                    arma1 = objeto.id.ToString()
                };
                PersonajeToBeUpdated.equipo.Add(nuevoEquipo);
            }
            else if (PersonajeToBeUpdated.equipo.Any(obj => obj.arma2 == null) && objeto.tipo == "arma")
            {
                Equipo nuevoEquipo = new Equipo
                {
                    arma2 = objeto.id.ToString()
                };
                PersonajeToBeUpdated.equipo.Add(nuevoEquipo);
            }
            else if (PersonajeToBeUpdated.equipo.Any(obj => obj.grebas == null) && objeto.tipo == "grebas")
            {
                Equipo nuevoEquipo = new Equipo
                {
                    grebas = objeto.id.ToString()
                };
                PersonajeToBeUpdated.equipo.Add(nuevoEquipo);
            }
            else if (PersonajeToBeUpdated.equipo.Any(obj => obj.guanteletes == null) && objeto.tipo == "guanteletes")
            {
                Equipo nuevoEquipo = new Equipo
                {
                    guanteletes = objeto.id.ToString()
                };
                PersonajeToBeUpdated.equipo.Add(nuevoEquipo);
            }
            else if (PersonajeToBeUpdated.equipo.Any(obj => obj.armadura == null) && objeto.tipo == "armadura")
            {
                Equipo nuevoEquipo = new Equipo
                {
                    armadura = objeto.id.ToString()
                };
                PersonajeToBeUpdated.equipo.Add(nuevoEquipo);
            }

            await _unitOfWork.CommitAsync();

            return PersonajeToBeUpdated; 
        }

        public async Task<Personaje> Desequipar(int PersonajeToBeUpdatedId, int ObjToRemoveID)
        {
            Personaje PersonajeToBeUpdated = await _unitOfWork.PersonajeRepository.GetByIdAsync(PersonajeToBeUpdatedId);
            Objeto objetoToRemove = await _unitOfWork.ObjetoRepository.GetByIdAsync(ObjToRemoveID);

            if (PersonajeToBeUpdated == null)
                throw new ArgumentException("Invalid Equipo ID while updating");

            if (objetoToRemove == null)
                throw new ArgumentException("Objeto no encontrado para desequipar.");

            var equipoToRemove = PersonajeToBeUpdated.equipo.FirstOrDefault(obj => 
                obj.casco == objetoToRemove.id.ToString() ||
                obj.arma1 == objetoToRemove.id.ToString() ||
                obj.arma2 == objetoToRemove.id.ToString() ||
                obj.grebas == objetoToRemove.id.ToString() ||
                obj.guanteletes == objetoToRemove.id.ToString() ||
                obj.armadura == objetoToRemove.id.ToString()
                );

            if (equipoToRemove != null)
            {
                if (equipoToRemove.casco == objetoToRemove.id.ToString())
                {
                    equipoToRemove.casco = "";
                }
                if (equipoToRemove.arma1 == objetoToRemove.id.ToString())
                {
                    equipoToRemove.arma1 = "";
                }
                if (equipoToRemove.arma2 == objetoToRemove.id.ToString())
                {
                    equipoToRemove.arma2 = "";
                }
                if (equipoToRemove.grebas == objetoToRemove.id.ToString())
                {
                    equipoToRemove.grebas = "";
                }
                if (equipoToRemove.guanteletes == objetoToRemove.id.ToString())
                {
                    equipoToRemove.guanteletes = "";
                }
                if (equipoToRemove.armadura == objetoToRemove.id.ToString())
                {
                    equipoToRemove.armadura = "";
                }
            }
            else
            {
                throw new ArgumentException("El objeto no esta equipado en el equipo.");
            }         

            await _unitOfWork.CommitAsync();

            return PersonajeToBeUpdated;
        }

        public async Task<Personaje> AceptarMision(int PersonajeToBeUpdatedId, int idMision)
        {
            Personaje PersonajeToBeUpdated = await _unitOfWork.PersonajeRepository.GetByIdAsync(PersonajeToBeUpdatedId);
            Mision misionToAccept = await _unitOfWork.MisionRepository.GetByIdAsync(idMision);

            if (PersonajeToBeUpdated == null)
                throw new ArgumentException("Personaje no encontrado.");

            if (misionToAccept == null)
                throw new ArgumentException("Mision no encontrada.");

            if (PersonajeToBeUpdated.misiones.Any(m => m.id == idMision))
                throw new ArgumentException("El personaje ya ha aceptado esta mision.");

            misionToAccept.estado = 'A'; 

            PersonajeToBeUpdated.misiones.Add(misionToAccept);

            await _unitOfWork.CommitAsync();

            return PersonajeToBeUpdated;
        }

        public async Task<Personaje> ProgresoMision(int PersonajeToBeUpdatedId, int idMision)
        {
            Personaje PersonajeToBeUpdated = await _unitOfWork.PersonajeRepository.GetByIdAsync(PersonajeToBeUpdatedId);
            Mision misionToUpdate = await _unitOfWork.MisionRepository.GetByIdAsync(idMision);

            if (PersonajeToBeUpdated == null)
                throw new ArgumentException("Personaje no encontrado.");

            if (misionToUpdate == null)
                throw new ArgumentException("Mision no encontrada.");

            var misionEnPersonaje = PersonajeToBeUpdated.misiones.FirstOrDefault(m => m.id == idMision);
            if (misionEnPersonaje == null)
                throw new ArgumentException("El personaje no ha aceptado esta mision.");

            if (misionToUpdate.estado != 'A')
                throw new ArgumentException("La mision debe ser aceptada para poder actualizar el progreso.");
            //duda no se si deberia hacer una clase para objetivos asi guardarlos en la lista
            //y poder cambiar el status del objetivo asi iria aumentando el porecntaje segun el status
           int objetivosCompletados = misionToUpdate.objetivos.Count();

            int progreso = (int)((double)objetivosCompletados / misionToUpdate.objetivos.Count * 100);

            misionToUpdate.progreso = progreso;

            await _unitOfWork.CommitAsync();

            return PersonajeToBeUpdated;
        }

        public async Task<Personaje> CompletarMision(int PersonajeToBeUpdatedId, int idMision)
        {
            Personaje PersonajeToBeUpdated = await _unitOfWork.PersonajeRepository.GetByIdAsync(PersonajeToBeUpdatedId);
            Mision misionToUpdate = await _unitOfWork.MisionRepository.GetByIdAsync(idMision);

            if (PersonajeToBeUpdated == null)
                throw new ArgumentException("Personaje no encontrado.");

            if (misionToUpdate == null)
                throw new ArgumentException("Mision no encontrada.");

            var misionEnPersonaje = PersonajeToBeUpdated.misiones.FirstOrDefault(m => m.id == idMision);
            if (misionEnPersonaje == null)
                throw new ArgumentException("El personaje no ha aceptado esta misión.");

            /*if (misionToUpdate.objetivos.Any(obj => obj.estado != 'C'))
                throw new ArgumentException("La mision no esta terminada.");*/

            misionToUpdate.estado = 'C'; 

            foreach (var recompensa in misionToUpdate.recompensas)
            {
                if (recompensa.tipo == "casco" && PersonajeToBeUpdated.equipo.All(e => e.casco == null))
                {
                    Equipo nuevoEquipo = new Equipo
                    {
                        casco = recompensa.id.ToString()  
                    };
                }
                else if (recompensa.tipo == "arma" && PersonajeToBeUpdated.equipo.All(e => e.arma1 == null))
                {
                    Equipo nuevoEquipo = new Equipo
                    {
                        arma1 = recompensa.id.ToString()  
                    };
                }
                else if (recompensa.tipo == "arma" && PersonajeToBeUpdated.equipo.All(e => e.arma2 == null))
                {
                    Equipo nuevoEquipo = new Equipo
                    {
                        //Habria que validar o cambiar las entidades de la clase a arma principal y arma secundaria
                        arma2 = recompensa.id.ToString()  
                    };
                }
                else if (recompensa.tipo == "grebas" && PersonajeToBeUpdated.equipo.All(e => e.grebas == null))
                {
                    Equipo nuevoEquipo = new Equipo
                    {
                        grebas = recompensa.id.ToString()  
                    };
                }
                else if (recompensa.tipo == "guanteletes" && PersonajeToBeUpdated.equipo.All(e => e.guanteletes == null))
                {
                    Equipo nuevoEquipo = new Equipo
                    {
                        guanteletes = recompensa.id.ToString()  
                    };
                }
                else if (recompensa.tipo == "armadura" && PersonajeToBeUpdated.equipo.All(e => e.armadura == null))
                {
                    Equipo nuevoEquipo = new Equipo
                    {
                        armadura = recompensa.id.ToString()  
                    };
                }
                
            }
            await _unitOfWork.CommitAsync();

            return PersonajeToBeUpdated;
        }
    }
}
