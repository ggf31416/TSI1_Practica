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
            if (!BsonClassMap.IsClassMapRegistered(typeof(Prueba)))
            {
                BsonClassMap.RegisterClassMap<Prueba>();
            }
            Console.WriteLine("inicio");
            IDALConstruccion iDALConstruccion = new DALConstruccionMongo();
            Prueba prueba = new Prueba();
            prueba.Nombre = "Pelado2";
            iDALConstruccion.AddPrueba(prueba);
            Console.WriteLine("agregado Pelado2");
            Prueba prueba1 = iDALConstruccion.GetPrueba("Pelado2");
            Console.WriteLine(prueba1.Nombre);
            prueba1.Nombre = "Pelado3";
            iDALConstruccion.UpdatePrueba(prueba1);
            //prueba1 = iDALConstruccion.GetPrueba("Pelado3");
            //Console.WriteLine(prueba1.Nombre);
            //iDALConstruccion.DeletePrueba("Pelado3");
            //prueba1 = iDALConstruccion.GetPrueba("Pelado3");
            //Console.WriteLine(prueba1.Nombre);
            Console.ReadLine();
        }
    }
}
