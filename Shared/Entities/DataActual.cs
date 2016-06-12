using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Shared.Entities
{
    [DataContract]
    public class DataActual
    {
        [DataMember]
        public Dictionary<int,EstadoData> EstadoUnidades { get; set; }
        [DataMember]
        public Dictionary<int, EstadoData> EstadoTecnologias { get; set; }
        [DataMember]
        public Dictionary<string, EstadoRecurso> EstadoRecursos { get; set; }
        [DataMember]
        public DateTime UltimaActualizacion { get; set; }
    }
}
