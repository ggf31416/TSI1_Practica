using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.Entities;

namespace BusinessLogicLayer
{
    public class BLConexion : IBLConexion
    {
        private DataAccessLayer.DALConexion _dal = new DataAccessLayer.DALConexion();

        



        public void agregarConexion(string tenant, string idJugador, string conId)
        {
            var conn = new ConexionSignalr() { ConnectionID = conId ,IdJugador = idJugador};
            
            _dal.agregarConexion(conn);
        }


        public void desconectar(string tenant, string idJugador, string conId)
        {
            var conn = new ConexionSignalr() { ConnectionID = conId, IdJugador = idJugador };
            _dal.eliminarConexion(conn);
        }

        public List<String> obtenerConexiones(string tenant,string idJugador)
        {
            return _dal.GetConexiones( idJugador);
        }

        public List<string> obtenerConexionesGrupo(string tenant,List<string> listaJugadores)
        {
            return _dal.GetConexionesGrupo( listaJugadores);
        }

        public List<String> obtenerConexionesClan(string tenant,string idJugador)
        {
            DataAccessLayer.DALUsuario _dalUsu = new DataAccessLayer.DALUsuario(tenant);
            var listaClientes = _dalUsu.GetJugadoresEnElClan(tenant, idJugador);
            var listaIds = listaClientes.Select(info => info.clienteId).ToList();
            return obtenerConexionesGrupo(tenant, listaIds);
        }
    }
}
