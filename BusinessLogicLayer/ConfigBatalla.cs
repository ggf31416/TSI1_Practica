using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer
{



    public class ConfigBatalla
    {
        public int TiempoDeploy { get; set; } = 30;
        public int TiempoBatalla { get; set; } = 180;
        public int MilisTurno { get; set; } = 500;
        public double FraccionRecursos { get; set; } = 0.25;
        public int MaxAtacantes { get; set; } = 2;
        public int MaxDefensores { get; set; } = 2;
        public int SegundosAtaque { get; set; } = 30;
    }
}
