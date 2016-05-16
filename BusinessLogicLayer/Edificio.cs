using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer
{
   public class Edificio
    {
        public int posX { get; set; }
        public int posY { get; set; }
        public int id { get; set; }
        public int tipo_id { get; set; }
        public int sizeX { get; set; } = 4;
        public int sizeY { get; set; } = 4;
    }
}
