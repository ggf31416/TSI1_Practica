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
        Juego GetJuegoUsuario(string tenant,string idUsuario);
        Juego GetJuegoUsuarioSinActualizar(string tenant, string idUsuario);
        void GuardarJuego(Juego j);
        void ActualizarJuego(Juego j);
    }
}
