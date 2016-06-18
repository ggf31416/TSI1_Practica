using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer
{
    public class Unidad : Entidad
    {
        
        public float velocidad { get; set; } = 10;
        public bool puedeDispararEnMovimiento { get; set; } = false;

    }
}
