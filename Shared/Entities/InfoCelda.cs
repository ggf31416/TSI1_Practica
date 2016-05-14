using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Shared.Entities
{
    [DataContract]
    public class InfoCelda
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public Nullable<int> PosX { get; set; }
        [DataMember]
        public Nullable<int> PosY { get; set; }
    }
}
