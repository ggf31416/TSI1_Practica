using Shared.Entities;
using Shared.Entities.DataBatalla;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace WCFServiceWebRole2
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IService1
    {
        [OperationContract]
        void JugarUnidad(InfoCelda infoCelda);

        [OperationContract]
        void Accion(String tenant, String json);

        [OperationContract]
        bool login(ClienteJuego cliente, string nombreJuego);

        [OperationContract]
        void register(ClienteJuego cliente, string nombreJuego);

        /*[OperationContract]
        List<JugadorBasico> GetListaDeJugadoresAtacables(string jugadorAt);*/

        [OperationContract]
        void IniciarAtaque(string tenant, InfoAtaque info);

        //DATA JUEGO
        [OperationContract]
        Juego GetAllDataJuego(string tenant);

        [OperationContract]
        bool ConstruirEdificio(CEInputData ceid, string Tenant, string NombreJugador);

        [OperationContract]
        int EntrenarUnidad(EUInputData euid, string Tenant, string NombreJugador);

        [OperationContract]
        ListasEntidades GetEntidadesActualizadas(string tenant, string nombreJugador);

        [OperationContract]
        Juego GetJuegoUsuario(string tenant, string idUsuario);

        [OperationContract]
        bool DesarrollarTecnologia(string tenant, string idJugador, int idTecnologia);


        //SOCIALES
        [OperationContract]
        List<ClienteJuego> GetJugadoresAtacables(string Tenant, string NombreJugador);

        //CLANES
        [OperationContract]
        void CrearClan(string NombreClan, string Tenant, string IdJugador);
        [OperationContract]
        bool AbandonarClan(string Tenant, string IdJugador);
        [OperationContract]
        List<ClienteJuego> GetJugadoresSinClan(string Tenant, string IdJugador);
        [OperationContract]
        bool AgregarJugadorClan(ClienteJuego Jugador, string Tenant, string IdJugador);
        [OperationContract]
        List<ClienteJuego> GetJugadoresEnElClan(string Tenant, string IdJugador);
        [OperationContract]
        bool SoyAdministrador(string Tenant, string IdJugador);
        [OperationContract]
        int EnviarRecursos(List<RecursoAsociado> tributos, string IdJugadorDestino, string Tenant, string IdJugador);

        [OperationContract]
        string GetClanJugador(string Tenant, string IdJugador);

        [OperationContract]
        InfoBatalla GetEstadoBatalla(string tenant, string idJugador);

        [OperationContract]
        void ConectarSignalr(string tenant, String idJugador, String conId);
        [OperationContract]
        void DesconectarSignalr(string tenant, String idJugador, String conId);
        [OperationContract]
        void ReconectarSignalr(string tenant, String idJugador, String conId);

        [OperationContract]
        void EnviarUnidades(string tenant, string idDefensor, Contribucion contr);
    }
}
