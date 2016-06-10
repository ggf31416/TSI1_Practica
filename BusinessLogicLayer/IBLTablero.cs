using Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer
{
    public interface IBLTablero
    {
        void JugarUnidad(InfoCelda infoCelda);
        void Accion(string json);
        bool login(ClienteJuego cliente, int idJuego);
        void register(ClienteJuego cliente, int idJuego);
        bool authenticate(ClienteJuego cliente, int idJuego);
        List<JugadorBasico> GetListaDeJugadoresAtacables(string jugadorAt);
        void IniciarAtaque(InfoAtaque info);


        bool ConstruirEdificio(CEInputData ceid);
        bool EntrenarUnidad(EUInputData euid);
    }
}
