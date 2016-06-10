using Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer
{
    public interface IBLJuego
    {
        Juego GetAllDataJuego(string tenant);

        void ActualizarJuego(Juego j);

        ListasEntidades GetEntidadesActualizadas(string tenant, string nombreJugador);
    }
}
