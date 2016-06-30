using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer
{



    public class ConfigBatalla
    {
        public int TiempoGracia { get; set; } = 0;
        public int TiempoSegundos { get; set; } = 300;
        public double FraccionRecursos { get; set; } = 0.25;
        public int MaxAtacantes { get; set; } = 2;
        public int MaxDefensores { get; set; } = 2;
        public int SegundosAtaque { get; set; } = 30;
    }
}
