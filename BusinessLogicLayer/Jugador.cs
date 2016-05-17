using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer
{
    class Jugador
    {
        public string Identificador { get; set; }
        public List<ConjuntoUnidades> Unidades { get; set; } = new List<ConjuntoUnidades>();
        public List<Edificio> Edificios { get; set; } = new List<Edificio>();
    }

    class ConjuntoUnidades
    {
        public int UnidadId { get; set; }
        public int Cantidad { get; set; }
    }
}
