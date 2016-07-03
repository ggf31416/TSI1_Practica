using BusinessLogicLayer;
using Shared.Entities;
using System.Collections.Generic;
using System.ServiceModel;
using Shared.Entities.DataBatalla;

namespace WCFServiceWebRole2
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    [ServiceBehavior(IncludeExceptionDetailInFaults = true)]
    public class Service1 : IService1
    {
        private static IBLJuego blJuegoHandler = new BLJuego(new DataAccessLayer.DALJuego());
        private static IBLTablero blHandler = BLTablero.getInstancia();
        private static IBLTecnologia blTecnologiaHandler = new BLTecnologia(blJuegoHandler);
        private static IBLConstruccion blConstruccionHandler = new BLConstruccion(new DataAccessLayer.DALConstruccion());
        private static IBLUsuario blUsuarioHandler = new BLUsuario(new DataAccessLayer.DALUsuario());
        private static IBLConexion blConexHandler = new BLConexion();
        private static IBLBatalla blBatalla = BLBatalla.getInstancia(blJuegoHandler);

        public void JugarUnidad(InfoCelda infoCelda)
        {
            blHandler.JugarUnidad(infoCelda);
        }

        public void Accion(string tenant, string json)
        {
            blBatalla.Accion(tenant, json);
        }

        public bool login(ClienteJuego cliente, string idJuego)
        {
            return blHandler.login(cliente, idJuego);
        }

        /*public List<JugadorBasico> GetListaDeJugadoresAtacables(string jugadorAt)
        {
            return blHandler.GetListaDeJugadoresAtacables(jugadorAt);
        }*/

        public void IniciarAtaque(string tenant, InfoAtaque info)
        {
            blBatalla.IniciarAtaque(tenant, info);
        }

        public void register(ClienteJuego cliente, string idJuego)
        {
            blHandler.register(cliente, idJuego);
        }

        //DATA JUEGO
        public Juego GetAllDataJuego(string tenant)
        {
            return blJuegoHandler.GetAllDataJuego(tenant);
        }

        public bool ConstruirEdificio(CEInputData ceid, string Tenant, string NombreJugador)
        {
            return blConstruccionHandler.ConstruirEdificio(ceid, Tenant, NombreJugador);
        }

        public int EntrenarUnidad(EUInputData euid, string Tenant, string NombreJugador)
        {
            return blConstruccionHandler.EntrenarUnidad(euid, Tenant, NombreJugador);
        }

        public ListasEntidades GetEntidadesActualizadas(string tenant, string nombreJugador)
        {
            return blJuegoHandler.GetEntidadesActualizadas(tenant, nombreJugador);
        }

        public Juego GetJuegoUsuario(string tenant, string idUsuario)
        {
            return blJuegoHandler.GetJuegoUsuario(tenant, idUsuario);
        }

        public bool DesarrollarTecnologia(string tenant, string idJugador, int idTecnologia)
        {
            return blTecnologiaHandler.DesarrollarTecnologia(tenant, idJugador, idTecnologia);
        }

        //SOCIALES
        public List<ClienteJuego> GetJugadoresAtacables(string Tenant, string NombreJugador)
        {
            return blUsuarioHandler.GetJugadoresAtacables(Tenant, NombreJugador);
        }

        //CLANES
        public void CrearClan(string NombreClan, string Tenant, string IdJugador)
        {
            blUsuarioHandler.CrearClan(NombreClan, Tenant, IdJugador);
        }

        public bool AbandonarClan(string Tenant, string IdJugador)
        {
            return blUsuarioHandler.AbandonarClan(Tenant, IdJugador);
        }

        public List<ClienteJuego> GetJugadoresSinClan(string Tenant, string IdJugador)
        {
            return blUsuarioHandler.GetJugadoresSinClan(Tenant, IdJugador);
        }

        public bool AgregarJugadorClan(ClienteJuego Jugador, string Tenant, string IdJugador)
        {
            return blUsuarioHandler.AgregarJugadorClan(Jugador, Tenant, IdJugador);
        }

        public List<ClienteJuego> GetJugadoresEnElClan(string Tenant, string IdJugador)
        {
            return blUsuarioHandler.GetJugadoresEnElClan(Tenant, IdJugador);
        }

        public bool SoyAdministrador(string Tenant, string IdJugador)
        {
            return blUsuarioHandler.SoyAdministrador(Tenant, IdJugador);
        }

        public void ConectarSignalr(string tenant, string idJugador, string conId)
        {
            blConexHandler.agregarConexion(tenant, idJugador, conId);
        }

        public void ReconectarSignalr(string tenant, string idJugador, string conId)
        {
            blConexHandler.agregarConexion(tenant, idJugador, conId);
        }

        public void DesconectarSignalr(string tenant, string idJugador, string conId)
        {
            blConexHandler.desconectar(tenant, idJugador, conId);
        }



        public int EnviarRecursos(List<RecursoAsociado> tributos, string IdJugadorDestino, string Tenant, string IdJugador)
        {
            return blUsuarioHandler.EnviarRecursos(tributos, IdJugadorDestino, Tenant, IdJugador);
        }

        public string GetClanJugador(string Tenant, string IdJugador)
        {
            return blUsuarioHandler.GetClanJugador(Tenant, IdJugador);
        }

        public InfoBatalla GetEstadoBatalla(string tenant, string idJugador)
        {
            return blBatalla.getEstadoBatalla(tenant, idJugador);
        }

        public void EnviarUnidades(string tenant, string idDefensor, Contribucion contr)
        {
            blBatalla.agregarContribucion(tenant, idDefensor, contr);
        }
    }
}
