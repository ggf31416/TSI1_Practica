using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Shared.Entities
{
    [DataContract]
    public class EstadoRecurso
    {
        [DataMember]
        public int Total { get; set; }
        [DataMember]
        public int Produccion { get; set; }
    }
}
