using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Entities
{
    [DataContract]
    public class TecnologiaDependencia
    {
        [DataMember]
        public int IdTecnologia { get; set; }
        [DataMember]
        public int IdTecnologiaDepende { get; set; }
        [DataMember]
        public int IdRaza { get; set; }
    }
}
