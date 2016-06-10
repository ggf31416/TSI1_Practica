using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer
{
   public class Edificio : Entidad
    {

        public int tipo_id { get; set; }
        public int sizeX { get; set; } = 4;
        public int sizeY { get; set; } = 4;

        public Dictionary<Shared.Entities.Recurso, double> produccion = new Dictionary<Shared.Entities.Recurso, double>();

        public Dictionary<Shared.Entities.Recurso, double> costo = new Dictionary<Shared.Entities.Recurso, double>();



    }
}
