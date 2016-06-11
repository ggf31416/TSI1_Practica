using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer;
using DataAccessLayer.Entities;
using MongoDB.Bson.Serialization;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            if (!BsonClassMap.IsClassMapRegistered(typeof(TableroConstruccion)))
            {
                BsonClassMap.RegisterClassMap<TableroConstruccion>();
            }
            if (!BsonClassMap.IsClassMapRegistered(typeof(ClienteJuego)))
            {
                BsonClassMap.RegisterClassMap<ClienteJuego>();
            }
            Console.WriteLine("inicio");
            IDALUsuario iDALUsuario = new DALUsuario(1);
            Shared.Entities.ClienteJuego cliente = new Shared.Entities.ClienteJuego();
            cliente.clienteId = "1";
            cliente.token = "token1";
            iDALUsuario.login(cliente);
            //IDALConstruccion iDALConstruccion = new DALConstruccionMongo("juego1");
            //InfoCelda infoCelda1 = new InfoCelda();
            //infoCelda1.Id = 1;
            //infoCelda1.PosX = 1;
            //infoCelda1.PosY = 2;
            //iDALConstruccion.InicializarConstruccion(1);
            //iDALConstruccion.AddInfoCelda(1, infoCelda1);
            Console.WriteLine("agregado usuario");
            Console.ReadLine();
        }
    }
}
