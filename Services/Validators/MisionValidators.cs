using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using FluentValidation;

namespace Services.Validators
{
    public class MisionValidators : AbstractValidator<Mision>
    {
        public MisionValidators(){

            RuleFor(x => x.nombre)
                .NotEmpty().WithMessage("El nombre no puede estar vacío.")
                ;

            RuleFor(x => x.objetivos)
                .NotEmpty().WithMessage("Los objetivos no pueden estar vacíos.")
                ;  

            RuleFor(x => x.nombre)
                .NotEmpty().WithMessage("El nombre no puede estar vacío.")
                ;  

            RuleFor(x => x.recompensas)
                .NotEmpty().WithMessage("Una recompensa debe ser agregada.")
                ;

            RuleFor(x => x.progreso)
                .LessThanOrEqualTo(100)
                ;

        }
    }
}