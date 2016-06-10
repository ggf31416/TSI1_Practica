using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Shared.Entities
{
    [DataContract]
    public class Accion
    {
        [DataMember]
        public string Nombre { get; set; }
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public Nullable<int> IdJuego { get; set; }
        [DataMember]
        public Nullable<int> IdTecnologia { get; set; }
        [DataMember]
        public Tecnologia Tecnologia { get; set; }
        [DataMember]
        public string NombreAtributo { get; set; }
        [DataMember]
        public Nullable<int> ValorPor { get; set; }
        [DataMember]
        public Nullable<int> Valor { get; set; }
        [DataMember]
        public Nullable<int> IdEntidad { get; set; }
    }
}
