using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Shared.Entities
{
    [DataContract]
    [KnownType(typeof(TipoEdificio))]
    [KnownType(typeof(TipoUnidad))]
    public abstract class TipoEntidad
    {
        [DataMember]
        public string Nombre { get; set; }
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public Nullable<int> Vida { get; set; }
        [DataMember]
        public Nullable<int> Defensa { get; set; }
        [DataMember]
        public string Imagen { get; set; }
        [DataMember]
        public Nullable<int> Ataque { get; set; }
        [DataMember]
        public Nullable<int> TiempoConstruccion { get; set; }
        [DataMember]
        public int IdJuego { get; set; }
        [DataMember]
        public List<Costo> Costos { get; set; }
    }
}
