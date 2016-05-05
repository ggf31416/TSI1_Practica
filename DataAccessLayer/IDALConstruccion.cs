using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer.Entities;

namespace DataAccessLayer
{
    public interface IDALConstruccion
    {

        void AddPrueba(Prueba prueba);

        void DeletePrueba(String nombre);

        void UpdatePrueba(Prueba pruebaUpdate);

        List<Prueba> GetAllPrueba();

        Prueba GetPrueba(String nombre);

    }
}
