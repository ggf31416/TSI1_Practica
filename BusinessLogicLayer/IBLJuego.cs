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
        Juego GetJuegoUsuarioSinGuardar(string tenant, string idUsuario);
        void GuardarJuegoAsync(Juego j);
        bool GuardarJuegoEsperar(Juego j);
        bool ActualizarJuego(Juego j);
        Dictionary<String, int> QuitarUnidades(Juego j, Contribucion contr, bool guardar);
        ListasEntidades GetEntidadesActualizadas(string tenant, string nombreJugador);
        void ModificarUnidadesRecursos(string tenant, string idUsuario, Dictionary<int, int> unidades, Dictionary<string, double> agregarRecursos, double mult);
    }
}
