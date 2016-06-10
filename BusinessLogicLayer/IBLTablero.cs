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

        List<JugadorBasico> GetListaDeJugadoresAtacables(string jugadorAt);
        void RegistrarJugador(string nombre);
        void IniciarAtaque(InfoAtaque info);

        void login(Cliente cliente, int idJuego);
        bool authenticate(Cliente cliente, int idJuego);
    }
}
