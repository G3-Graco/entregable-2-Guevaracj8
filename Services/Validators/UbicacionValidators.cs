using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces.Entities;
using FluentValidation;

namespace Services.Validators
{
    public class UbicacionValidators : AbstractValidator<Ubicacion>
    {
        public UbicacionValidators()
        {
            RuleFor(x => x.nombre)
                .NotEmpty().WithMessage("El nombre de la ubicacion no puede estar vacío.")
                ;

            RuleFor(x => x.descripcion)
                .NotEmpty().WithMessage("La descripcion de la ubicacion no puede estar vacía.")
                ;

            RuleFor(x => x.clima)
                .NotEmpty().WithMessage("El clima de la ubicacion no puede estar vacío.")
                ;
        }
    }
}