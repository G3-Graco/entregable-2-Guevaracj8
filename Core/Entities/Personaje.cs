using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Interfaces.Entities;

namespace Core.Entities
{
    public class Personaje : IPersonaje
    {
        public int id { get; set; }
        public string nombre { get; set; } = string.Empty;
        public int salud { get; set; }
        public int energia { get; set; }
        public int fuerza { get; set; }
        public int inteligencia { get; set; }
        public int agilidad { get; set; }
        public int nivel { get; set; }
        public int defensa { get; set; }
        public List<Equipo> equipo { get; set; } = new List<Equipo>();
        public List<Habilidad> habilidades { get; set; } = new List<Habilidad>();
        public int experiencia { get; set; }
        public int? tipoId { get; set; }
        public virtual TipoPersonaje? tipo { get; set; } 
        public string ubicacion {get;set;}
        public List<Mision> misiones{ get; set; }
    }
}