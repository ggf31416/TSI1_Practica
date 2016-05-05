using System;
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
    public class DALConstruccionMongo : IDALConstruccion
    {
        const string connectionstring = "mongodb://40.84.2.155";
        protected static IMongoClient _client = new MongoClient(connectionstring);
        protected static IMongoDatabase _database = _client.GetDatabase("frontoffice");
        protected static IMongoCollection<Prueba> collection = _database.GetCollection<Prueba>("construccion");

        public void AddPrueba(Prueba prueba)
        {
            collection.InsertOne(prueba);
        }

        public void DeletePrueba(String nombre)
        {
            collection.DeleteOne(prueba => prueba.Nombre == nombre);
        }

        public void UpdatePrueba(Prueba pruebaUpdate)
        {
            collection.ReplaceOne(prueba => prueba.Nombre == pruebaUpdate.Nombre, pruebaUpdate);
        }


        public List<Prueba> GetAllPrueba()
        {
            var collection = _database.GetCollection<BsonDocument>("construccion");
            List<Prueba> result = new List<Prueba>();
            List<BsonDocument> mongoResult = collection.AsQueryable<BsonDocument>().ToList();
            foreach (var prueba in mongoResult)
            {
                try
                {
                    result.Add(BsonSerializer.Deserialize<Prueba>(prueba));
                }
                catch (Exception ex)
                {
                    //ignore
                }
            }
            return result;
        }

        public Prueba GetPrueba(String nombre)
        {
            var prueba = from p in collection.AsQueryable<Prueba>()
                                     where p.Nombre == nombre
                                     select p;
            return prueba.First();
        }
    }
}
