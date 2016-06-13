using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.Entities;

namespace BusinessLogicLayer
{
    public class BLConexion
    {
        private DataAccessLayer.DALConexion _dal = new DataAccessLayer.DALConexion();



        public void agregarConexion(string tenant,ConexionSignalr conn)
        {
            _dal.agregarConexion(tenant, conn);
        }

        public void desconectar(string tenant, ConexionSignalr conn)
        {
            _dal.eliminarConexion(tenant, conn);
        }

        public List<String> obtenerConexiones(string tenant,string idJugador)
        {
            return _dal.GetConexiones(tenant, idJugador);
        }

        public List<string> obtenerConexionesGrupo(string tenant,List<string> listaJugadores)
        {
            return _dal.GetConexionesGrupo(tenant, listaJugadores);
        }
    }
}
