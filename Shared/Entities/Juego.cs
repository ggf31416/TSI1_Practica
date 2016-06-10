using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using MongoDB.Bson.Serialization.Attributes;

namespace Shared.Entities
{
    [DataContract]
    public class Juego
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string IdJugador { get; set; }
        [DataMember]
        [BsonId]
        public string Nombre { get; set; }
        [DataMember]
        public string Imagen { get; set; }
        [DataMember]
        public int Estado { get; set; }
        [DataMember]
        public string Url { get; set; }
        [DataMember]
        public int IdDisenador { get; set; }
        [DataMember]
        public List<Accion> Acciones { get; set; }
        [DataMember]
        public List<Raza> Razas { get; set; }
        [DataMember]
        public List<Tecnologia> Tecnologias { get; set; }
        [DataMember]
        public List<TipoEntidad> TipoEntidad { get; set; }
        [DataMember]
        public List<TipoEdificio> TipoEdificios { get; set; }
        [DataMember]
        public List<TipoUnidad> TipoUnidades { get; set; }
        [DataMember]
        public List<TipoRecurso> TipoRecurso { get; set; }
        [DataMember]
        public Tablero Tablero { get; set; }

        [DataMember]
        public DataActual DataJugador { get; set; }
    }
}
