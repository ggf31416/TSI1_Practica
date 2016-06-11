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
        public enum EstadoEnum {A,
            C,
            Inactivo
        };

        [DataMember]
        public EstadoEnum Estado { get; set; }

        [DataMember]
        public DateTime Fin { get; set; }

        [DataMember]
        public int Cantidad { get; set; }


        
        [DataMember]
        public long Faltante
        {
            get {
                return (long)((Fin - DateTime.Now).TotalMilliseconds);
            }
            protected set { }
        }
    }
}
