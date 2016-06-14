using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.Attributes;

namespace DataAccessLayer.Entities
{
    public class Clan
    {
        [BsonId]
        public string Nombre { get; set; }
        public string AdministradorId { get; set; }
        public List<string> IdJugadores { get; set; }
    }
}
