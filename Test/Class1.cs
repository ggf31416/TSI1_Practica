using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.Entities;
using DataAccessLayer;

namespace Test
{
    public class Class1
    {

        static void Main(string[] args)
        {
            IDALConstruccion iDALConstruccion = new DALConstruccionMongo();
            Prueba prueba = new Prueba();
            prueba.Nombre = "Pepito";
            iDALConstruccion.AddPrueba(prueba);
        }

    }
}
