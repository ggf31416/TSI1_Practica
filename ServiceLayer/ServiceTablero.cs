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

        public ServiceTablero()
        {
            blHandler = Program.blHandler;
            blJuegoHandler = Program.blJuegoHandler;
            blTecnologiaHandler = Program.blTecnologiaHandler;
        }

        public void JugarUnidad(InfoCelda infoCelda) {
            blHandler.JugarUnidad(infoCelda);
        }

        public void Accion(string json)
        {
            blHandler.Accion(json);
        }

        public bool login(ClienteJuego cliente, int idJuego)
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

        public void register(ClienteJuego cliente, int idJuego)
        {
            blHandler.register(cliente, idJuego);
        }

        //DATA JUEGO
        public Juego GetAllDataJuego(string tenant)
        {
            return blJuegoHandler.GetAllDataJuego(tenant);
        }

        public bool DesarrollarTecnologia(string tenant, string idJugador,int idTecnologia)
        {
            return blTecnologiaHandler.DesarrollarTecnologia(tenant, idJugador, idTecnologia);
        }

       

    }
}
