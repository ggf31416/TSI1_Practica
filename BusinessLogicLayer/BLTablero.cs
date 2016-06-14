using DataAccessLayer;
using EpPathFinding;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceStack.Redis;
using Microsoft.AspNet.SignalR.Client.Hubs;
using System.ServiceModel;
using Shared.Entities;

namespace BusinessLogicLayer
{
    public class BLTablero : IBLTablero
    {

        
        private static BLTablero instancia = null;
        public static BLTablero getInstancia()
        {
            if (instancia == null) instancia = new BLTablero();
            return instancia;
        }

        

        private BLTablero()
        {
        }



        // es jugarEdificio
        public void JugarUnidad(Shared.Entities.InfoCelda infoCelda)
        {  //_dal.JugarUnidad(infoCelda);

            //client.Send("{\"Id\":" + infoCelda.Id + ",\"PosX\":" + infoCelda.PosX + ",\"PosY\":" + infoCelda.PosY + "}");
        }


        private static ServiceInteraccionClient getCliente()
        {
            BLServiceClient serviceClient = new BLServiceClient();
            ServiceInteraccionClient client = new ServiceInteraccionClient(serviceClient.binding, serviceClient.address);
            return client;
        }



        public bool login(ClienteJuego cliente, string nombreJuego)
        {
            IDALUsuario iDALUsuario = new DALUsuario(nombreJuego);
            return iDALUsuario.login(cliente);
        }

        public void register(ClienteJuego cliente, string nombreJuego)
        {
            IDALUsuario iDALUsuario = new DALUsuario(nombreJuego);
            iDALUsuario.register(cliente);
        }

        public bool authenticate(ClienteJuego cliente, string nombreJuego)
        {
            IDALUsuario iDALUsuario = new DALUsuario(nombreJuego);
            return iDALUsuario.authenticate(cliente);
        }

        /*public List<JugadorBasico> GetListaDeJugadoresAtacables(string jugadorAt)
        {
            Jugador at = jugadores.GetValueOrDefault(jugadorAt);
            if (at == null) {
                // nos pasaron mal el jugador
                return new List<JugadorBasico>();
            }
            // busco jugadores que no sean del mismo clan (por ahora cada jugador es un clan!!!)
            var posibles = jugadores.Values.Where(j => !j.Clan.Equals(at.Clan))
                .Select(j => new JugadorBasico { Id = j.Id, Nombre = j.Id });
            return posibles.ToList();
        }*/

    }
}
