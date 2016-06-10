using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Shared.Entities
{
    [DataContract]
    public class TableroCelda
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public Nullable<int> IdTipoEdificio { get; set; }
        [DataMember]
        public Nullable<int> PosFila { get; set; }
        [DataMember]
        public Nullable<int> PosColumna { get; set; }
        [DataMember]
        public Nullable<int> IdTablero { get; set; }

        [DataMember]
        public EstadoData Estado { get; set; }
    }
}
