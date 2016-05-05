using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.Attributes;


namespace DataAccessLayer.Entities
{
    class Cliente
    {
        [BsonId]
        string clienteId { get; set; }
        string login { get; set; }

    }
}
