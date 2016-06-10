using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Shared.Entities
{
    [DataContract]
    public class EUInputData
    {
        [DataMember]
        public int IdTipoUnidad { get; set; }
        [DataMember]
        public int Cantidad { get; set; }
    }
}
