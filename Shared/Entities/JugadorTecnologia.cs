using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Entities
{
    public class JugadorTecnologia
    {
        public string IdJugador { get; set; }
        public List<int> Tecnologias { get; set; } = new List<int>();
        public List<TecnologiaDesarrollo> EnDesarrollo = new List<TecnologiaDesarrollo>();
        public List<int> Desarrollables { get; set; } = new List<int>();
    }

    public class TecnologiaDesarrollo
    {
        public int IdTecnologia;
        public DateTime TiempoFinalizacion { get; set; }
    }
}
