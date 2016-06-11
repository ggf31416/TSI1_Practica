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
    [DataContract]
    public class Juego
    {
        public Juego() { }
        public Juego(JuegoIndependiente juegoIndependiente)
        {
            this.Id = juegoIndependiente.Id;
            this.IdJugador = juegoIndependiente.IdJugador;
            this.Nombre = juegoIndependiente.Nombre;
            this.Imagen = juegoIndependiente.Imagen;
            this.Estado = juegoIndependiente.Estado;
            this.Url = juegoIndependiente.Url;
            this.IdDisenador = juegoIndependiente.IdDisenador;
            this.Acciones = juegoIndependiente.Acciones;
            this.Razas = juegoIndependiente.Razas;
            this.Tecnologias = juegoIndependiente.Tecnologias;
            this.TipoEdificios = juegoIndependiente.TipoEdificios;
            this.TipoUnidades = juegoIndependiente.TipoUnidades;
            this.TipoRecurso = juegoIndependiente.TipoRecurso;
            this.Tablero = juegoIndependiente.Tablero;
        }

        [DataMember]
        public int Id { get; set; }
        [DataMember]
        [BsonId]
        public string IdJugador { get; set; }
        [DataMember]
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