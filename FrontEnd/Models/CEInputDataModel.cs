using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Shared.Entities
{
    public class CEInputDataModel
    {
        public int IdTipoEdificio { get; set; }
        public int PosFila { get; set; }
        public int PosColumna { get; set; }
    }
}
