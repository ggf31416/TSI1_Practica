using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Entities
{

    class ConexionSignalr
    {
        public string ConnectionID { get; set; }
        //public string UserAgent { get; set; }
        //public bool Connected { get; set; }
    }
    class JugadorConexion
    {
        [BsonId]
        string IdJugador;
        List<string> conexionesId { get; set; } = new List<string>();
    }
}
