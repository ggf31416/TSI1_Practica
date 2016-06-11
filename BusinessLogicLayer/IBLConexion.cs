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
         void agregarConexion(string tenant, ConexionSignalr conn);
         void desconectar(string tenant, ConexionSignalr conn);
         List<String> obtenerConexiones(string tenant, string idJugador);
        List<string> obtenerConexionesGrupo(string tenant, List<string> listaJugadores);
    }
}
