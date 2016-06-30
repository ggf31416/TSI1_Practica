using DataAccessLayer.Entities;
using MongoDB.Driver;
using Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace DataAccessLayer
{
    public class DALConexion
    {
        const string connectionstring = "mongodb://40.84.2.155";
        private static IMongoClient client = new MongoClient(connectionstring);
        //private string nombreJuego;
        private IMongoDatabase database;
        private IMongoCollection<JugadorConexion> collection;
        private string tenant = "__Conexiones__SignalR__";


        public void agregarConexion( ConexionSignalr conn)
        {

            var sw = Stopwatch.StartNew();
            database = client.GetDatabase(tenant);
            collection = database.GetCollection<JugadorConexion>("jugConexion");
            JugadorConexion jugCon = collection.AsQueryable().FirstOrDefault(x => x.IdJugador == conn.IdJugador);
            if (jugCon == null)
            {
                jugCon = new JugadorConexion() { IdJugador = conn.IdJugador };
            }
            Console.WriteLine("cantConn: " + jugCon.ConexionesId.Count  +" jug " + conn.IdJugador + " new " + conn.ConnectionID);
            if (!jugCon.ConexionesId.Contains(conn.ConnectionID))
            {
                if (jugCon.ConexionesId.Count >= 5)
                {
                    jugCon.ConexionesId.RemoveAt(0);
                }
                jugCon.ConexionesId.Add(conn.ConnectionID);
            }
            else
            {
                Console.WriteLine("Ya estaba!");
            }
            collection.ReplaceOne (x => x.IdJugador == conn.IdJugador, jugCon,new UpdateOptions() { IsUpsert = true });
            Console.WriteLine("Agregar Conexion "+ sw.ElapsedMilliseconds);
        }

        public List<String> GetConexionesGrupo( List<string> listaJugadores)
        {
            var sw = Stopwatch.StartNew();
            database = client.GetDatabase(tenant);
            collection = database.GetCollection<JugadorConexion>("jugConexion");
            var listaJug = collection.AsQueryable().Where(x => listaJugadores.Contains(x.IdJugador));
            var listaConexiones = listaJug.SelectMany(c => c.ConexionesId);
            var res =  listaConexiones.ToList(); 
            Console.WriteLine("GetConGrupos " + sw.ElapsedMilliseconds);
            return res;
        }

        public List<String> GetConexiones( string idJugador)
        {
            database = client.GetDatabase(tenant);
            collection = database.GetCollection<JugadorConexion>("jugConexion");
            var act = collection.AsQueryable().FirstOrDefault(x => x.IdJugador == idJugador).ConexionesId;
            return act;
        }

        public bool eliminarConexion(  ConexionSignalr conn)
        {
            var sw = Stopwatch.StartNew();
            try
            {
                database = client.GetDatabase(tenant);
                collection = database.GetCollection<JugadorConexion>("jugConexion");
                var act = collection.AsQueryable().FirstOrDefault(x => x.IdJugador.Equals(conn.IdJugador));
                bool elimino = act == null ? false : act.ConexionesId.Remove(conn.ConnectionID);
                if (elimino)
                {
                    collection.ReplaceOne(x => x.IdJugador == conn.IdJugador, act, new UpdateOptions() { IsUpsert = false });
                }
                Console.WriteLine("Eliminar Conexion " + sw.ElapsedMilliseconds + " estaba: " + elimino + " jug " + conn.IdJugador + " connId " + conn.ConnectionID);
                return elimino;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }

        }

    }
}
