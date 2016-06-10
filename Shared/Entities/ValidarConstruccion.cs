using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Shared.Entities
{
    [DataContract]
    public class ValidarConstruccion
    {
        [DataMember]
        public TipoEdificio TipoEdificio { get; set; }
        [DataMember]
        public Tablero Tablero { get; set; }
        [DataMember]
        public Dictionary<int,int> Recursos { get; set; }
    }
}
