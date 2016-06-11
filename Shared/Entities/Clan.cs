using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Shared.Entities
{
    [DataContract]
    public class Clan
    {
        [DataMember]
        public string AdministradorId { get; set; }
        [DataMember]
        public string Nombre { get; set; }
        [DataMember]
        public List<string> IdJugadores { get; set; }
    }
}
