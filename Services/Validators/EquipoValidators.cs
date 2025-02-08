using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces.Entities;
using FluentValidation;

namespace Services.Validators
{
    public class EquipoValidators : AbstractValidator<Equipo>
    {
        public EquipoValidators()
        {
            
            RuleFor(x => x.casco)
                //.Must((Equipo, casco) => casco != null && Equipo.casco == "casco")
                .NotEmpty().WithMessage("El caso no puede estar vacío.")
                ;

            RuleFor(x => x.arma1)
                //.Must((Equipo, arma1) => arma1 != null && Equipo.arma1 == "arma")
                .NotEmpty().WithMessage("El arma no puede estar vacía.")
                ;    

            RuleFor(x => x.arma2)
                //.Must((Equipo, arma2) => arma2 != null && Equipo.arma2 == "arma")
                .NotEmpty().WithMessage("El arma no puede estar vacía.")
                ; 
            
            RuleFor(x => x.grebas)
                //.Must((Equipo, grebas) => grebas != null && Equipo.grebas == "grebas")
                .NotEmpty().WithMessage("Las grebas no puede estar vacío.")
                ; 

            RuleFor(x => x.guanteletes)
                //.Must((Equipo, guantes) => guantes != null && Equipo.guanteletes == "guanteletes")
                .NotEmpty().WithMessage("Los guanteletes no puede estar vacío.")
                ; 

            RuleFor(x => x.armadura)
                //.Must((Equipo, arm) => arm != null && Equipo.armadura == "armadura")
                .NotEmpty().WithMessage("La armadura no puede estar vacía.")
                ; 
        }   
    }
}