using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Entities
{
    [DataContract]
    public class ClienteJuego
    {
        [DataMember]
        public string clienteId { get; set; }
        [DataMember]
        public string token { get; set; }
        [DataMember]
        public int idJuego { get; set; }
        [DataMember]
        public string username { get; set; }
        [DataMember]
        public string nombre { get; set; }
        [DataMember]
        public string apellido { get; set; }
        [DataMember]
        public DateTime creacion { get; set; }
    }
}
