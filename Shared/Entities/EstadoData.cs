using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Shared.Entities
{
    [DataContract]
    public class EstadoData
    {
        public enum EstadoEnum {A,C};

        [DataMember]
        public EstadoEnum Estado { get; set; }
        [DataMember]
        public int Tiempo { get; set; }
        [DataMember]
        public int Cantidad { get; set; }
    }
}
