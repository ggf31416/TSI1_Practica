using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Shared.Entities
{
    [DataContract]
    public class CEInputData
    {
        [DataMember]
        public int IdTipoEdificio { get; set; }
        [DataMember]
        public int PosFila { get; set; }
        [DataMember]
        public int PosColumna { get; set; }
    }
}
