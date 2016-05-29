using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Shared.Entities
{
    [DataContract]
    public class RecursoAsociado
    {
        [DataMember]
        public int IdEdificio { get; set; }
        [DataMember]
        public int IdRecurso { get; set; }
        [DataMember]
        public int Valor { get; set; }
    }
}
