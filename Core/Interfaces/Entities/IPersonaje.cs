using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;

namespace Core.Interfaces.Entities
{
    public interface IPersonaje
    {
        public int id { get; set; }
        public string nombre { get; set; }
        public int salud { get; set; }
        public int energia { get; set; }
        public int fuerza { get; set; }
        public int inteligencia { get; set; }
        public int agilidad { get; set; }
        public int nivel { get; set; }
        public List<Equipo> equipo { get; set; }
        public List<Habilidad> habilidades { get; set; }
        public string ubicacion {get;set;}
        public List<Mision> misiones{ get; set; }

    }
}