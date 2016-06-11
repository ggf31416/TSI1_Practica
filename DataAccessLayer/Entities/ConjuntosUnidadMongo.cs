using MongoDB.Bson.Serialization.Attributes;
using Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Entities
{
    class ConjuntosUnidadMongo
    {
        [BsonId]
        public string IdJugador;
        public List<ConjuntoUnidades> Unidades { get; set; } = new List< ConjuntoUnidades>();
    }
}
