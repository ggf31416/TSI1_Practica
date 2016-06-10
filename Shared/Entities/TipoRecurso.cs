using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Shared.Entities
{
    [DataContract]
    public class TipoRecurso
    {
        [DataMember]
        public string Nombre { get; set; }
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Imagen { get; set; }
        [DataMember]
        public int IdJuego { get; set; }
        [DataMember]
        public int Tope { get; set; }
        [DataMember]
        public bool EsFijo { get; set; }
    }
}
