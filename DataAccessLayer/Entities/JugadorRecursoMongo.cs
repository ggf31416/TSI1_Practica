using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Entities
{
    class JugadorRecursoMongo
    {
        [BsonId]
        public string IdJugador;
        public List<Shared.Entities.CantidadRecurso> Recursos { get; set; }
    }
}
