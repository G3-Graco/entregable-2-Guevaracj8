using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Interfaces.Entities;

namespace Core.Entities
{
    public class Mision : IMision
    {
        public int id { get; set; }
        public string nombre { get; set; } = string.Empty;
        public List<string> objetivos { get; set; } = new List<string>();
        public List<Objeto> recompensas { get; set; } = new List<Objeto>();
        public char estado { get; set; }
        public int progreso { get; set;}
    }
}