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
        public Dictionary<string,EstadoData> EstadoUnidades { get; set; } = new Dictionary<string, EstadoData>();
        [DataMember]
        public Dictionary<string, EstadoData> EstadoTecnologias { get; set; } = new Dictionary<string, EstadoData>();
        [DataMember]
        public Dictionary<string, EstadoRecurso> EstadoRecursos { get; set; } = new Dictionary<string, EstadoRecurso>();
        [DataMember]
        public DateTime UltimaActualizacion { get; set; } = DateTime.Now;
    }
}
