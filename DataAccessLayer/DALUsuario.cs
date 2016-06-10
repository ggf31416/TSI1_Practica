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
        private string nombreJuego;
        private IMongoDatabase database;
        private IMongoCollection<ClienteJuego> collection;
        //private IMongoCollection<JugadorConexion> conexionesJug;
        private IMongoCollection<Shared.Entities.FechaCantidad> collectionFechaCantidad;

        public DALUsuario(string nombreJuego)
        {
            this.nombreJuego = nombreJuego;
            database = client.GetDatabase(nombreJuego);
            collection = database.GetCollection<ClienteJuego>("usuario");
            //conexionesJug = database.GetCollection<JugadorConexion>("usuario");
            collectionFechaCantidad = database.GetCollection<Shared.Entities.FechaCantidad>("fecha_cantidad");
        }
        
        private void inicializarUsuario(ClienteJuego cliente)
        {
            cliente.creacion = DateTime.UtcNow;
            collection.InsertOne(cliente);
            IDALConstruccion iDALConstruccion = new DALConstruccion();
            iDALConstruccion.InicializarConstruccion(cliente.id, this.nombreJuego);
        }

        public bool login(Shared.Entities.ClienteJuego client)
        {
            var query = from usuario in collection.AsQueryable<ClienteJuego>()
                        where usuario.id == client.clienteId
                        select usuario;
            if (query.Count() == 0)
            {
                return false;
            }
            else
            {
                ClienteJuego c = query.First();
                c.token = client.token;
                collection.ReplaceOne(cliente => cliente.id == c.id, c);
                Shared.Entities.FechaCantidad fechaCantidad = new Shared.Entities.FechaCantidad();
                fechaCantidad.cantidad = 1;
                fechaCantidad.fecha = DateTime.UtcNow;
                collectionFechaCantidad.InsertOne(fechaCantidad);
                return true;
            }
        }

        public void register(Shared.Entities.ClienteJuego client)
        {
            ClienteJuego c = new ClienteJuego();
            c.id = client.clienteId;
            c.nombre = client.nombre;
            c.apellido = client.apellido;
            c.username = client.username;
            inicializarUsuario(c);
        }

        public bool authenticate(Shared.Entities.ClienteJuego client)
        {
            var query = from usuario in collection.AsQueryable<ClienteJuego>()
                        where usuario.id == client.clienteId && usuario.token == client.token
                        select usuario;
            return query.Count() > 0;
        }

        public void logout(Shared.Entities.ClienteJuego client)
        {
            ClienteJuego updateCliente = new ClienteJuego();
            updateCliente.id = client.clienteId;
            updateCliente.nombre = client.nombre;
            updateCliente.apellido = client.apellido;
            updateCliente.username = client.username;
            updateCliente.token = null;
            collection.ReplaceOne(cliente => cliente.id == updateCliente.id, updateCliente);
        }
    }
}
