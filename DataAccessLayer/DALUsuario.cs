using DataAccessLayer.Entities;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DataAccessLayer
{
    public class DALUsuario : IDALUsuario
    {
        const string connectionstring = "mongodb://40.84.2.155";
        private static IMongoClient client = new MongoClient(connectionstring);
        private string juego;
        private IMongoDatabase database;
        private IMongoCollection<Cliente> collection;

        public DALUsuario(string juego)
        {
            this.juego = juego;
            database = client.GetDatabase(juego);
            collection = database.GetCollection<Cliente>("usuario");
        }
        
        private void inicializarUsuario(Cliente cliente)
        {
            collection.InsertOne(cliente);
            IDALConstruccion iDALConstruccion = new DALConstruccionMongo(this.juego);
            iDALConstruccion.InicializarConstruccion(cliente.clienteId);
        }

        public void login(Shared.Entities.Cliente client)
        {
            var query = from usuario in collection.AsQueryable<Cliente>()
                        where usuario.clienteId == client.clienteId
                        select usuario;
            if (query.Count() == 0)
            {
                Cliente cliente = new Cliente(client.clienteId, client.token);
                inicializarUsuario(cliente);
            }
            else
            {
                Cliente c = query.First();
                c.token = client.token;
                collection.ReplaceOne(cliente => cliente.clienteId == c.clienteId, c);
            }
            
        }

    }
}
