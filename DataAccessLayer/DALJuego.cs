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

        public JuegoIndependiente GetJuego(string tenant)
        {
            IMongoDatabase _database = _client.GetDatabase(tenant.ToString());
            IMongoCollection<JuegoIndependiente> collection = _database.GetCollection<JuegoIndependiente>("juego");

            var query = from juego in collection.AsQueryable<JuegoIndependiente>()
                        where juego.Nombre == tenant
                        select juego;

            if (query.Count() > 0) {
                return query.First();
            }
            else
            {
                return null;
            }
        }

        public Juego GetJuegoUsuario(string tenant, string idUsuario)
        {
            IMongoDatabase _database = _client.GetDatabase(tenant.ToString());
            IMongoCollection<Juego> collection = _database.GetCollection<Juego>("juego_usuario");

            var query = from juego in collection.AsQueryable<Juego>()
                        where juego.IdJugador == idUsuario
                        select juego;

            if (query.Count() > 0)
            {
                return query.First();
            }
            else
            {
                return null;
            }
        }

        private Task<ReplaceOneResult> GuardarJuegoUsuario(Juego juego)
        {
            string tenant = juego.Nombre;
            IMongoDatabase _database = _client.GetDatabase(tenant);
            IMongoCollection<Juego> collection = _database.GetCollection<Juego>("juego_usuario");

            return collection.ReplaceOneAsync(j => j.IdJugador == juego.IdJugador, juego, new UpdateOptions { IsUpsert = true });
        }

        public Task GuardarJuegoUsuarioAsync(Juego juego)
        {
            return GuardarJuegoUsuario(juego);
        }

        // espera a que termine la operacion y retorna si tuvo exito
        public bool GuardarJuegoUsuarioEsperar(Juego juego)
        {
            var task = GuardarJuegoUsuario(juego);
            var res = task.Result;
            return res.ModifiedCount == 1;
        }


        public ListasEntidades GetEntidadesActualizadas(string tenant, string nombreJugador)
        {
            ListasEntidades ret = new ListasEntidades();

            Juego juego = GetJuegoUsuario(tenant, nombreJugador);

            ret.TipoEdificios = juego.TipoEdificios;
            ret.TipoUnidades = juego.TipoUnidades;

            return ret;
        }
    }
}
