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

        private IMongoCollection<AtaqueConjunto> getColeccion(string tenant)
        {
            IMongoDatabase database = client.GetDatabase(tenant);
            IMongoCollection<AtaqueConjunto> collection = database.GetCollection<AtaqueConjunto>("ataqConj");
            return collection;
        }

        private IMongoCollection<Contribucion> getColeccionContr(string tenant)
        {
            IMongoDatabase database = client.GetDatabase(tenant);
            IMongoCollection<Contribucion> collection = database.GetCollection<Contribucion>("ataqConjContr");
            return collection;
        }

        // guarda o actualiza el ataque conjunto
        public string guardarAtaqueConj(AtaqueConjunto info)
        {
            var idxBuilder = Builders<AtaqueConjunto>.IndexKeys;
            var collection = getColeccion(info.Tenant);
            collection.Indexes.CreateOneAsync(idxBuilder.Hashed(_info => _info.ClanAtacante));
            collection.Indexes.CreateOneAsync(idxBuilder.Hashed(_info => _info.ClanDefensor));
            collection.ReplaceOne(_info => _info.IdBatalla == info.IdBatalla, info, new UpdateOptions() { IsUpsert = true });
            return info.IdBatalla;
        }

        

        public AtaqueConjunto obtenerAtaqueConj(string tenant, string idBatalla)
        {
            var collection = getColeccion(tenant);
            AtaqueConjunto res = collection.AsQueryable().FirstOrDefault(info => info.Tenant == tenant && info.IdBatalla == idBatalla);
            return res;
        }

        // obtiene ataques del clan en el que no participa el jugador
        public List<InfoAtaqConj> obtenerAtaquesClan(string tenant,string clan,string IdJugador)
        {
            var collection = getColeccion(tenant);
            var filtro = collection.AsQueryable().Where(at => at.ClanAtacante == clan || at.ClanDefensor == clan).Where(at => at.Jugadores.Contains(IdJugador));


            List<InfoAtaqConj> res = filtro.Select(
                atConj => new InfoAtaqConj()
                {
                    IdBatalla = atConj.IdBatalla,
                    NombreDefensor = atConj.NombreDefensor,
                    SoyClanAtacante = clan == atConj.ClanAtacante || IdJugador == atConj.Atacante,
                    Tenant = atConj.Tenant,
                    SegundosFaltante = (int)(atConj.FechaAtaque - DateTime.UtcNow).TotalSeconds
                })
                .ToList();
            return res;
        }

        

        
        public void agregarContribucion(string tenant,string IdBatalla,Contribucion contr)
        {
            var idxBuilder = Builders<Contribucion>.IndexKeys;
            var collection = getColeccionContr(tenant);
            collection.Indexes.CreateOneAsync(idxBuilder.Hashed(_info => _info.IdBatalla));
            collection.Indexes.CreateOneAsync(idxBuilder.Hashed(_info => _info.IdJugador));
 
            collection.InsertOne(contr);
            //var update = Builders<InfoAtaqueConj>.Update.Push(info => info.UnidadesContribuidas, contr);
            //var res = collection.UpdateOne(info => info.Tenant == tenant && info.Defensor == idDefensor, update);
            //return res.IsModifiedCountAvailable && res.ModifiedCount == 1;
        }

       public List<Contribucion> obtenerContribuciones(string tenant, string IdBatalla)
        {
            var collection = getColeccionContr(tenant);
            var res = collection.AsQueryable().Where(_info => _info.IdBatalla == IdBatalla).ToList();
            return res;
        }

        public List<InfoContribucion> obtenerContribucionesUsuario(string tenant,string IdJugador)
        {
            var collection = getColeccionContr(tenant);
            var res = collection.AsQueryable().Where(_info => _info.IdJugador == IdJugador).
                Select(contr => new InfoContribucion()
                {
                    NombreDefensor = contr.NombreDefensor,
                    CantUnidades = contr.CantUnidades,
                    SoyAtacante = contr.SoyAtacante,
                    SegundosFaltantes = (int)(contr.FechaAtaque - DateTime.UtcNow).TotalSeconds
                }
                ).ToList();
            return res;
        }

        public void eliminarAtaqueConjunto(string tenant, string IdBatalla)
        {
            var col = getColeccion(tenant);
            col.DeleteOne(a => a.IdBatalla == IdBatalla);
            var collection = getColeccionContr(tenant);
            collection.DeleteMany(_info => _info.IdBatalla == IdBatalla);
        }



        
    }
}
