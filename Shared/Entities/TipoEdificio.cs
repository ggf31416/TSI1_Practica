using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Shared.Entities
{
    [DataContract]
    public class TipoEdificio : TipoEntidad
    {
        [DataMember]
        public List<int> UnidadesAsociadas { get; set; }
        [DataMember]
        public List<int> Tecnologias { get; set; }
        [DataMember]
        public List<RecursoAsociado> RecursosAsociados { get; set; }
    }
}
