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

namespace DataAccessLayer
{
    public class DALConstruccionMongo : IDALConstruccion
    {
        const string connectionstring = "mongodb://40.84.2.155";
        protected static IMongoClient _client = new MongoClient();
        protected static IMongoDatabase _database = _client.GetDatabase("frontoffice");
        //var collection = _database.GetCollection<Employee>("employees");
        public void prueba(String prueba)
        {

        }

    }
}
