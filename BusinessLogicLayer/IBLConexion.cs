using Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer
{
   public interface IBLConexion
    {
        void agregarConexion( string tenant, String idJugador, String conId);
        void desconectar(string tenant, String idJugador, String conId);
        List<String> obtenerConexiones(string tenant, string idJugador);
        List<string> obtenerConexionesGrupo(string tenant, List<string> listaJugadores);
    }
}
