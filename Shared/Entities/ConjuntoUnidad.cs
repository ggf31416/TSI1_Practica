using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Entities
{
    public class CantidadRecurso
    {
        public double porSegundo { get; set; }
        public double acumulado { get; set; }
    }

    public class ConjuntoUnidades
    {
        public int UnidadId { get; set; }
        public int Cantidad { get; set; }
    }
}
