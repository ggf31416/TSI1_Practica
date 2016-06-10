using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Entities
{
    [DataContract]
    public class TecnologiaRecursoCosto
    {
        [DataMember]
        public int IdTecnologia { get; set; }
        [DataMember]
        public int IdRecurso { get; set; }
        [DataMember]
        public int Costo { get; set; }
    }
}
