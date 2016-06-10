using Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer.Entities;

namespace DataAccessLayer
{
    public interface IDALJuego
    {
        Juego GetJuego(string tenant);
        Juego GetJuegoUsuario(string tenant, string idUsuario);
        Task GuardarJuegoUsuarioAsync(Juego juego);
        bool GuardarJuegoUsuarioEsperar(Juego juego);

        ListasEntidades GetEntidadesActualizadas(string tenant, string nombreJugador);
    }
}
