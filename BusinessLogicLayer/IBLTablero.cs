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

        bool login(ClienteJuego cliente, string nombreJuego);
        void register(ClienteJuego cliente, string nombreJuego);
        bool authenticate(ClienteJuego cliente, string nombreJuego);
        //List<JugadorBasico> GetListaDeJugadoresAtacables(string jugadorAt);
       

    }
}
