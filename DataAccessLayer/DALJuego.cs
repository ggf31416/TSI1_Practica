using System;
using Shared.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Core;
using MongoDB.Driver.Linq;
using MongoDB.Bson.Serialization;
using DataAccessLayer.Entities;

namespace DataAccessLayer
{
    public class DALJuego : IDALJuego
    {
        const string connectionstring = "mongodb://40.84.2.155";
        private static IMongoClient _client = new MongoClient(connectionstring);

        public Juego GetJuego(Int32 idJuego)
        {
            IMongoDatabase _database = _client.GetDatabase(idJuego.ToString());
            IMongoCollection<Juego> collection = _database.GetCollection<Juego>("juego");

            var query = from juego in collection.AsQueryable<Juego>()
                        where juego.Id == idJuego
                        select juego;

            if (query.Count() > 0) {
                return query.First();
            }
            else
            {
                return null;
            }
        }
    }
}
