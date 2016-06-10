using BusinessLogicLayer;
using Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;

namespace ServiceLayer
{
    [ServiceBehavior(IncludeExceptionDetailInFaults = true)]
    public class ServiceTablero : IServiceTablero
    {
        private static IBLTablero blHandler;
        private static IBLJuego blJuegoHandler;
        private static IBLTecnologia blTecnologiaHandler;
        private static IBLConstruccion blConstruccionHandler;

        public ServiceTablero()
        {
            blHandler = Program.blHandler;
            blJuegoHandler = Program.blJuegoHandler;
            blTecnologiaHandler = Program.blTecnologiaHandler;
            blConstruccionHandler = Program.blConstruccionHandler;
        }

        public void JugarUnidad(InfoCelda infoCelda) {
            blHandler.JugarUnidad(infoCelda);
        }

        public void Accion(string json)
        {
            blHandler.Accion(json);
        }

        public bool login(ClienteJuego cliente, string idJuego)
        {
            return blHandler.login(cliente, idJuego);
        }

        public List<JugadorBasico> GetListaDeJugadoresAtacables(string jugadorAt)
        {
            return blHandler.GetListaDeJugadoresAtacables(jugadorAt);
        }

        public void IniciarAtaque(InfoAtaque info)
        {
            blHandler.IniciarAtaque(info);
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

        public bool ConstruirEdificio(CEInputData ceid)
        {
            return blConstruccionHandler.ConstruirEdificio(ceid);
        }

        public int EntrenarUnidad(EUInputData euid)
        {
            return blConstruccionHandler.EntrenarUnidad(euid);
        }

        public ListasEntidades GetEntidadesActualizadas(string tenant, string nombreJugador)
        {
            return blJuegoHandler.GetEntidadesActualizadas(tenant, nombreJugador);
        }

        public bool DesarrollarTecnologia(string tenant, string idJugador,int idTecnologia)
        {
            return blTecnologiaHandler.DesarrollarTecnologia(tenant, idJugador, idTecnologia);
        }

    }
}
