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
        private int idJuego;
        private IMongoDatabase database;
        private IMongoCollection<Cliente> collection;

        public DALUsuario(int idJuego)
        {
            this.idJuego = idJuego;
            database = client.GetDatabase(idJuego.ToString());
            collection = database.GetCollection<Cliente>("usuario");
        }
        
        private void inicializarUsuario(Cliente cliente)
        {
            collection.InsertOne(cliente);
            IDALConstruccion iDALConstruccion = new DALConstruccion(this.idJuego);
            iDALConstruccion.InicializarConstruccion(cliente.clienteId);
        }

        public bool login(Shared.Entities.ClienteJuego client)
        {
            var query = from usuario in collection.AsQueryable<Cliente>()
                        where usuario.clienteId == client.clienteId
                        select usuario;
            if (query.Count() == 0)
            {
                return false;
            }
            else
            {
                Cliente c = query.First();
                c.token = client.token;
                collection.ReplaceOne(cliente => cliente.clienteId == c.clienteId, c);
                return true;
            }
        }

        public void register(Shared.Entities.ClienteJuego client)
        {
            Cliente c = new Cliente();
            c.clienteId = client.clienteId;
            c.token = client.token;
            c.nombre = client.nombre;
            c.apellido = client.apellido;
            c.username = client.username;
            inicializarUsuario(c);
        }

        public bool authenticate(Shared.Entities.ClienteJuego client)
        {
            var query = from usuario in collection.AsQueryable<Cliente>()
                        where usuario.clienteId == client.clienteId && usuario.token == client.token
                        select usuario;
            return query.Count() > 0;
        }

        
    }
}
