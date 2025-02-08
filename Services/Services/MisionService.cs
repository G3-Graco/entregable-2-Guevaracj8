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
    public class MisionService : IMisionService
    {
         private readonly IUnitOfWork _unitOfWork;
        public MisionService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        

        public async Task<Mision> Create(Mision newMision)
        {
            MisionValidators validator = new();


            var validationResult = await validator.ValidateAsync(newMision);
            if (validationResult.IsValid)
            {

                await _unitOfWork.MisionRepository.AddAsync(newMision);
                await _unitOfWork.CommitAsync();
            }
            else
            {
                
                throw new ArgumentException(validationResult.Errors[0].ErrorMessage.ToString());
            }

            return newMision;
        }

        public async Task Delete(int MisionId)
        {
            Mision Mision = await _unitOfWork.MisionRepository.GetByIdAsync(MisionId);
            if(Mision != null)
            {
                _unitOfWork.MisionRepository.Remove(Mision);
                await _unitOfWork.CommitAsync();
            }
            else
            {
                throw new Exception("La Mision no existe");
            }
        }

        public async Task<IEnumerable<Mision>> GetAll()
        {
            return await _unitOfWork.MisionRepository.GetAllAsync();
        }

        public async Task<Mision> GetById(int id)
        {
            return await _unitOfWork.MisionRepository.GetByIdAsync(id);
        }

        public async Task<Mision> Update(int MisionToBeUpdatedId, Mision newMisionValues)
        {        
            MisionValidators MisionValidator = new();
            var validationResult = await MisionValidator.ValidateAsync(newMisionValues);
            if (!validationResult.IsValid)
                throw new ArgumentException(validationResult.Errors.ToString());

            Mision MisionToBeUpdated = await _unitOfWork.MisionRepository.GetByIdAsync(MisionToBeUpdatedId);

            if (MisionToBeUpdated == null)
                throw new ArgumentException("Invalid Mision ID while updating");

            MisionToBeUpdated.nombre = newMisionValues.nombre;
            MisionToBeUpdated.estado = newMisionValues.estado;
            MisionToBeUpdated.recompensas = newMisionValues.recompensas;
            MisionToBeUpdated.objetivos = newMisionValues.objetivos;

            await _unitOfWork.CommitAsync();

            return await _unitOfWork.MisionRepository.GetByIdAsync(MisionToBeUpdatedId);
        }
    }
}