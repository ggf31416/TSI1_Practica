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
        void InicializarConstruccion(string idUsuario, string nombreJuego);

        //TableroConstruccion getTableroConstruccion(string idUsuario);

        //void AddInfoCelda(string idUsuario, InfoCelda infoCelda);

        //void DeleteInfoCelda(string idUsuario, InfoCelda infoCelda);

        //void Refresh(string idUsuario, Shared.Entities.Juego juego);

        Shared.Entities.ValidarConstruccion ConstruirEdificio(int IdEdificio, string tenant, string nombreJugador);

        bool PersistirEdificio(Shared.Entities.CEInputData ceid, string tenant, string nombreJugador);

        Shared.Entities.ValidarUnidad EntrenarUnidad(int IdUnidad, string tenant, string nombreJugador);

        bool PersistirUnidades(Shared.Entities.EUInputData euid, string tenant, string nombreJugador);
    }
}
