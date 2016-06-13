using DataAccessLayer.Entities;
using MongoDB.Driver;
using Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class DALConexion
    {
        const string connectionstring = "mongodb://40.84.2.155";
        private static IMongoClient client = new MongoClient(connectionstring);
        private string nombreJuego;
        private IMongoDatabase database;
        private IMongoCollection<JugadorConexion> collection;



        public void agregarConexion(string tenant,ConexionSignalr conn)
        {
            database = client.GetDatabase(tenant);
            collection = database.GetCollection<JugadorConexion>("jugConexion");
            var act = collection.AsQueryable().FirstOrDefault(x => x.IdJugador == conn.ConnectionID);
            act.ConexionesId.Add(conn.ConnectionID);
            collection.ReplaceOneAsync(x => x.IdJugador == conn.ConnectionID, act);

        }

        public List<String> GetConexionesGrupo(string tenant, List<string> listaJugadores)
        {
            database = client.GetDatabase(tenant);
            collection = database.GetCollection<JugadorConexion>("jugConexion");
            var listaJug = collection.AsQueryable().Where(x => listaJugadores.Contains(x.IdJugador));
            var listaConexiones = listaJug.SelectMany(c => c.ConexionesId);
            return listaConexiones.ToList(); ;
        }

        public List<String> GetConexiones(string tenant,string idJugador)
        {
            database = client.GetDatabase(tenant);
            collection = database.GetCollection<JugadorConexion>("jugConexion");
            var act = collection.AsQueryable().FirstOrDefault(x => x.IdJugador == idJugador).ConexionesId;
            return act;
        }

        public bool eliminarConexion(string tenant, ConexionSignalr conn)
        {
            database = client.GetDatabase(tenant);
            collection = database.GetCollection<JugadorConexion>("jugConexion");
            var act = collection.AsQueryable().FirstOrDefault(x => x.IdJugador == conn.ConnectionID);
            var elimino = act.ConexionesId.Remove(conn.ConnectionID);
            collection.ReplaceOneAsync(x => x.IdJugador == conn.ConnectionID, act);
            return elimino;
        }

    }
}
