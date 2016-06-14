using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.Attributes;

namespace DataAccessLayer.Entities
{
    public class ClienteJuego
    {
        [BsonId]
        public string id { get; set; }
        public string username { get; set; }
        public string nombre { get; set; }
        public string apellido { get; set; }
        public string token { get; set; }
        public DateTime creacion { get; set; }
        public bool atacable { get; set; }
        public string clan { get; set; }
    }
}
