using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Shared.Entities
{
    [DataContract]
    public class ValidarUnidad
    {
        [DataMember]
        public TipoUnidad TipoUnidad { get; set; }
        [DataMember]
        public Dictionary<int,int> Recursos { get; set; }
    }
}
