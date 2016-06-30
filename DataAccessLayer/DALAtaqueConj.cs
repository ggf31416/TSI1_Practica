using MongoDB.Driver;
using Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataAccessLayer
{
    public class DALAtaqueConj
    {
        const string connectionstring = "mongodb://40.84.2.155";
        private static IMongoClient client = new MongoClient(connectionstring);

        private IMongoCollection<InfoAtaqueConj> getColeccion(string tenant)
        {
            IMongoDatabase database = client.GetDatabase(tenant);
            IMongoCollection<InfoAtaqueConj> collection = database.GetCollection<InfoAtaqueConj>("jugConexion");
            return collection;
        }

        public void agregarAtaqueConj(InfoAtaqueConj info)
        {
            var collection = getColeccion(info.Tenant);
            collection.InsertOne(info);
        }

        

        public InfoAtaqueConj obtenerAtaqueConj(string tenant, string idDefensor)
        {
            var collection = getColeccion(tenant);
            InfoAtaqueConj res = collection.AsQueryable().FirstOrDefault(info => info.Tenant == tenant && info.Defensor == idDefensor);
            return res;
        }

        // agrega atomicamente una contribucion
        public bool agregarContribucion(string tenant,string idDefensor,Contribucion contr)
        {
            var collection = getColeccion(tenant);

            var update = Builders<InfoAtaqueConj>.Update.Push(info => info.UnidadesContribuidas, contr);
            var res = collection.UpdateOne(info => info.Tenant == tenant && info.Defensor == idDefensor, update);
            return res.IsModifiedCountAvailable && res.ModifiedCount == 1;
        }
    }
}
