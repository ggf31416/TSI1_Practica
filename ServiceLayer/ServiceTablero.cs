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

        public ServiceTablero()
        {
            blHandler = Program.blHandler;
            blJuegoHandler = Program.blJuegoHandler;
        }

        public void JugarUnidad(InfoCelda infoCelda) {
            blHandler.JugarUnidad(infoCelda);
        }

        public void Accion(string json)
        {
            blHandler.Accion(json);
        }


        public List<JugadorBasico> GetListaDeJugadoresAtacables(string jugadorAt)
        {
            return blHandler.GetListaDeJugadoresAtacables(jugadorAt);
        }

        public void RegistrarJugador(string nombre)
        {
            blHandler.RegistrarJugador(nombre);
        }

        public void IniciarAtaque(InfoAtaque info)
        {
            blHandler.IniciarAtaque(info);
        }

        public void login(Cliente cliente, int idJuego)
        {
            blHandler.login(cliente, idJuego);
        }


        //DATA JUEGO
        public Juego GetAllDataJuego(Int32 idJuego)
        {
            return blJuegoHandler.GetAllDataJuego(idJuego);
        }

        public List<int> GetTecnologias(string tenant, string idJugador)
        {
            return bl
        }
    }
}
