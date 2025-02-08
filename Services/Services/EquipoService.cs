using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces;
using Core.Services;
using Core.Responses;
using Services.Validators;
using Core.Interfaces.Services;

namespace Services.Services
{
    public class EquipoService : IEquipoService
    {
         private readonly IUnitOfWork _unitOfWork;
        public EquipoService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Equipo> Create(Equipo newEquipo)
        {
            EquipoValidators validator = new();


            var validationResult = await validator.ValidateAsync(newEquipo);
            if (validationResult.IsValid)
            {

                await _unitOfWork.EquipoRepository.AddAsync(newEquipo);
                await _unitOfWork.CommitAsync();
            }
            else
            {
                
                throw new ArgumentException(validationResult.Errors[0].ErrorMessage.ToString());
            }

            return newEquipo;
        }

        public async Task Delete(int EquipoId)
        {
            Equipo Equipo = await _unitOfWork.EquipoRepository.GetByIdAsync(EquipoId);
            if(Equipo != null)
            {
                _unitOfWork.EquipoRepository.Remove(Equipo);
                await _unitOfWork.CommitAsync();
            }
            else
            {
                throw new Exception("El Equipo no existe");
            }
        }

        public async Task<IEnumerable<Equipo>> GetAll()
        {
            return await _unitOfWork.EquipoRepository.GetAllAsync();
        }

        public async Task<Equipo> GetById(int id)
        {
            return await _unitOfWork.EquipoRepository.GetByIdAsync(id);
        }

        public async Task<Equipo> Update(int EquipoToBeUpdatedId, Equipo newEquipoValues)
        {
            EquipoValidators EquipoValidator = new();
            
            var validationResult = await EquipoValidator.ValidateAsync(newEquipoValues);
            if (!validationResult.IsValid)
                throw new ArgumentException(validationResult.Errors.ToString());

            Equipo EquipoToBeUpdated = await _unitOfWork.EquipoRepository.GetByIdAsync(EquipoToBeUpdatedId);

            if (EquipoToBeUpdated == null)
                throw new ArgumentException("Invalid Equipo ID while updating");

            //PersonajeToBeUpdated.tipoId = newPersonajeValues.tipoId;
            EquipoToBeUpdated.casco = newEquipoValues.casco;
            EquipoToBeUpdated.arma1 = newEquipoValues.arma1;
            EquipoToBeUpdated.arma2 = newEquipoValues.arma2;
            EquipoToBeUpdated.grebas = newEquipoValues.grebas;
            EquipoToBeUpdated.armadura = newEquipoValues.armadura;
            EquipoToBeUpdated.guanteletes = newEquipoValues.guanteletes;

            await _unitOfWork.CommitAsync();

            return await _unitOfWork.EquipoRepository.GetByIdAsync(EquipoToBeUpdatedId);
        }
    }
}