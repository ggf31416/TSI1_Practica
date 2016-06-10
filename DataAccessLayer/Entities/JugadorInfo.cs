using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.Entities;
using MongoDB.Bson.Serialization.Attributes;


namespace DataAccessLayer.Entities
{
    class JugadorInfo
    {
        [BsonId]
        public int IdUsuario;
        public Dictionary<int, ConjuntoUnidades> Unidades { get; set; } = new Dictionary<int, ConjuntoUnidades>();
        //public List<Edificio> Edificios { get; set; } = new List<Edificio>();
        public List<TipoEdificio> TipoEdificios { get; set; } = new List<TipoEdificio>();
        public List<TipoUnidad> TipoUnidades { get; set; } = new List<TipoUnidad>();
        public Dictionary<int, CantidadRecurso> Recursos { get; set; } = new Dictionary<int, CantidadRecurso>(); // clave Recurso.ID
        public DateTime ultimaActualizacionRecursos;

        public void DesdeShared(Shared.Entities.JugadorShared shared)
        {
            Unidades = shared.Unidades;
            TipoEdificios = shared.TipoEdificios;
            TipoUnidades = shared.TipoUnidades;
            Recursos = shared.Recursos;
            ultimaActualizacionRecursos = shared.ultimaActualizacionRecursos;
        }

        public Shared.Entities.JugadorShared ToShared()
        {
            var shared = new Shared.Entities.JugadorShared();
            shared.Unidades = this.Unidades;
            shared.TipoEdificios = this.TipoEdificios;
            shared.TipoUnidades = this.TipoUnidades;
            shared.Recursos = this.Recursos;
            shared.ultimaActualizacionRecursos = this.ultimaActualizacionRecursos;
            return shared;
        }
    }
}
