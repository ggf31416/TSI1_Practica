using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Entities
{
    class JugadorTecnologiasMongo
    {
        public int IdJugador { get; set; }
        public List<int> Tecnologias { get; set; } = new List<int>();
        public List<TecnologiaDesarrollo> EnDesarrollo = new List<TecnologiaDesarrollo>();
        public List<int> Desarrollables { get; set; } = new List<int>();
    }

    class TecnologiaDesarrollo
    {
        public int IdTecnologia;
        public DateTime TiempoFinalizacion { get; set; }
        public bool Terminada { get; set; }
    }
}
