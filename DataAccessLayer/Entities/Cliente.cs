using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.Attributes;


namespace DataAccessLayer.Entities
{
    public class Cliente
    {
        [BsonId]
        public int clienteId { get; set; }
        public string token { get; set; }

        public Cliente(int clienteId, string token)
        {
            this.clienteId = clienteId;
            this.token = token;
        }
    }
}
