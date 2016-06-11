using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace Shared.Entities
{
    public class JuegoIndependiente
    {
        [BsonId]
        public int Id { get; set; }
        public string IdJugador { get; set; }
        public string Nombre { get; set; }
        public string Imagen { get; set; }
        public int Estado { get; set; }
        public string Url { get; set; }
        public int IdDisenador { get; set; }
        public List<Accion> Acciones { get; set; }
        public List<Raza> Razas { get; set; }
        public List<Tecnologia> Tecnologias { get; set; }
        public List<TipoEntidad> TipoEntidad { get; set; }
        public List<TipoEdificio> TipoEdificios { get; set; }
        public List<TipoUnidad> TipoUnidades { get; set; }
        public List<TipoRecurso> TipoRecurso { get; set; }
        public Tablero Tablero { get; set; }
    }
}
