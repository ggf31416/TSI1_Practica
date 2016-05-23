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

        void InicializarConstruccion(int idUsuario);

        TableroConstruccion getTableroConstruccion(int idUsuario);

        void AddInfoCelda(int idUsuario, InfoCelda infoCelda);

        void DeleteInfoCelda(int idUsuario, InfoCelda infoCelda);

        void Refresh(int idUsuario, Shared.Entities.Juego juego);

        //void AddPrueba(Prueba prueba);

        //void DeletePrueba(String nombre);

        //void UpdatePrueba(Prueba pruebaUpdate);

        //List<Prueba> GetAllPrueba();

        //Prueba GetPrueba(String nombre);

    }
}
