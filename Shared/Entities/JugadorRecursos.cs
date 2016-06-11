using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Entities
{
    class JugadorRecursos
    {
        public string IdJugador { get; set; }
        public Dictionary<int, Shared.Entities.CantidadRecurso> Recursos { get; set; } = new Dictionary<int, CantidadRecurso>();
    }
}
