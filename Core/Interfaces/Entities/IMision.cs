using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;

namespace Core.Interfaces.Entities
{
    public interface IMision
    {
        public int id { get; set; }
        public string nombre { get; set; }
        public List<string> objetivos { get; set; }
        public List<Objeto> recompensas { get; set; }
        public int progreso { get; set;}
        public char estado { get; set; }
    }
}