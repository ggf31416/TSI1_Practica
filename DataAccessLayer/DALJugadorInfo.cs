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
using DataAccessLayer.Exceptions;

namespace DataAccessLayer
{
    class DALJugadorInfo
    {

        const string connectionstring = "mongodb://40.84.2.155";
        private static IMongoClient client = new MongoClient(connectionstring);
        private IMongoDatabase database;
        private IMongoCollection<TableroConstruccion> collection;
        private int idJuego;

        public DALJugadorInfo(int idJuego)
        {
            this.idJuego = idJuego;
            database = client.GetDatabase(idJuego.ToString());
            collection = database.GetCollection<TableroConstruccion>("construccion");
        }
        public void inicializarJugadorInfo(int idUsuario)
        {
            JugadorInfo info = new JugadorInfo();
            info.IdUsuario = idUsuario;
            
        }
    }
}
