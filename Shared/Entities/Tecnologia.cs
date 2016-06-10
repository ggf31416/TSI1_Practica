using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Shared.Entities
{
    [DataContract]
    public class Tecnologia
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Nombre { get; set; }
        [DataMember]
        public string Imagen { get; set; }
        [DataMember]
        public int Tiempo { get; set; }
        [DataMember]
        public int IdEdificio { get; set; }
        [DataMember]
        public int IdJuego { get; set; }
        [DataMember]
        public List<TecnologiaRecursoCosto> TecnologiaRecursoCostos { get; set; }
        [DataMember]
        public List<TecnologiaDependencia> TecnologiaDependencias { get; set; }
        [DataMember]
        public List<Accion> AccionesAsociadas { get; set; }
    }
}
