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

        void InicializarConstruccion(string idUsuario);

        TableroConstruccion getTableroConstruccion(string idUsuario);

        void AddInfoCelda(string idUsuario, InfoCelda infoCelda);

        void DeleteInfoCelda(string idUsuario, InfoCelda infoCelda);

        void Refresh(string idUsuario, Shared.Entities.Juego juego);

        Shared.Entities.ValidarConstruccion ConstruirEdificio(int IdEdificio);

        PersistirEdificio(Shared.Entities.CEInputData ceid);

        Shared.Entities.ValidarUnidad EntrenarUnidad(int IdUnidad);

        void PersistirUnidades(Shared.Entities.EUInputData ceid);
    }
}
