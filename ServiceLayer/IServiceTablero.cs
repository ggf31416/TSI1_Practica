using Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer
{
    [ServiceContract(Namespace = "http://localhost:8836/tsi1/")]
    public interface IServiceTablero
    {
        [OperationContract]
        void JugarUnidad(InfoCelda infoCelda);

        [OperationContract]
        void Accion(String json);

        [OperationContract]
        bool login(ClienteJuego cliente, string nombreJuego);

        [OperationContract]
        void register(ClienteJuego cliente, string nombreJuego);

        [OperationContract]
        List<JugadorBasico> GetListaDeJugadoresAtacables(string jugadorAt);

        [OperationContract]
        void IniciarAtaque(InfoAtaque info);

        //DATA JUEGO
        [OperationContract]
        Juego GetAllDataJuego(string tenant);

        [OperationContract]
        bool ConstruirEdificio(CEInputData ceid);

        [OperationContract]
        int EntrenarUnidad(EUInputData euid);

        [OperationContract]
        ListasEntidades GetEntidadesActualizadas(string tenant, string nombreJugador);

        [OperationContract]
        bool DesarrollarTecnologia(string tenant, string idJugador, int idTecnologia);
    }
}
