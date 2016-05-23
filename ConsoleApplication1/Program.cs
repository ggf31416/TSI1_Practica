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
            Console.WriteLine("inicio");
            IDALConstruccion iDALConstruccion = new DALConstruccionMongo();
            InfoCelda infoCelda1 = new InfoCelda();
            infoCelda1.Id = 1;
            infoCelda1.PosX = 1;
            infoCelda1.PosY = 2;
            //iDALConstruccion.InicializarConstruccion(1);
            iDALConstruccion.AddInfoCelda(1, infoCelda1);
            Console.WriteLine("agregado infocelda");
            Console.ReadLine();
        }
    }
}
