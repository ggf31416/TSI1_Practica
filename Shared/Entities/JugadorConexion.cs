using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Entities
{

    public class ConexionSignalr
    {
        public string ConnectionID { get; set; }
        string IdJugador { get; set; }
        //public string UserAgent { get; set; }
        //public bool Connected { get; set; }
    }
    public class JugadorConexion
    {
        [BsonId]
        public string IdJugador { get; set; }
        public List<string> ConexionesId { get; set; } = new List<string>();
    }
}
