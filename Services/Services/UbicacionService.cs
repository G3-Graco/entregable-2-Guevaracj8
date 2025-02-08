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
    public class UbicacionService : IUbicacionService
    {
         private readonly IUnitOfWork _unitOfWork;
        public UbicacionService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Ubicacion> Create(Ubicacion newUbicacion)
        {
            UbicacionValidators validator = new();


            var validationResult = await validator.ValidateAsync(newUbicacion);
            if (validationResult.IsValid)
            {
                await _unitOfWork.UbicacionRepository.AddAsync(newUbicacion);
                await _unitOfWork.CommitAsync();
            }
            else
            {
                
                throw new ArgumentException(validationResult.Errors[0].ErrorMessage.ToString());
            }

            return newUbicacion;
        }

        public async Task Delete(int UbicacionId)
        {
            Ubicacion Ubicacion = await _unitOfWork.UbicacionRepository.GetByIdAsync(UbicacionId);
            if(Ubicacion != null)
            {
                _unitOfWork.UbicacionRepository.Remove(Ubicacion);
                await _unitOfWork.CommitAsync();
            }
            else
            {
                throw new Exception("La Ubicacion no existe");
            }
        }

        public async Task<IEnumerable<Ubicacion>> GetAll()
        {
            return await _unitOfWork.UbicacionRepository.GetAllAsync();
        }

        public async Task<Ubicacion> GetById(int id)
        {
            return await _unitOfWork.UbicacionRepository.GetByIdAsync(id);
        }

        public async Task<Ubicacion> Update(int UbicacionToBeUpdatedId, Ubicacion newUbicacionValues)
        {
            UbicacionValidators UbicacionValidator = new();
            
            var validationResult = await UbicacionValidator.ValidateAsync(newUbicacionValues);
            if (!validationResult.IsValid)
                throw new ArgumentException(validationResult.Errors.ToString());

            Ubicacion UbicacionToBeUpdated = await _unitOfWork.UbicacionRepository.GetByIdAsync(UbicacionToBeUpdatedId);

            if (UbicacionToBeUpdated == null)
                throw new ArgumentException("Invalid Personaje ID while updating");

            UbicacionToBeUpdated.nombre = newUbicacionValues.nombre;
            UbicacionToBeUpdated.descripcion = newUbicacionValues.descripcion;
            UbicacionToBeUpdated.clima = newUbicacionValues.clima;

            await _unitOfWork.CommitAsync();

            return await _unitOfWork.UbicacionRepository.GetByIdAsync(UbicacionToBeUpdatedId);
        }
    }
}