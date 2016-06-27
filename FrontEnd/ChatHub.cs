using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using System.Threading.Tasks;

namespace FrontEnd
{
    public class ChatHub : Hub
    {

        public ChatHub()
        {
            
        }

        public void send(string name, string message)
        {
            Console.WriteLine(name + " -> " + message);
            Clients.All.broadcastMessage(name, message);
            //Clients.Caller.broadcastMessage(name, "mensaje enivado");
        }

        public void SendGrupo(string grupo,string mensaje)
        {
            string nombre = grupo;
            Clients.Group(grupo).broadcastMessage(nombre, mensaje);
        }

        public void agregarUsuarioGrupo(string idUsuario,string grupo)
        {
            Groups.Add(idUsuario, grupo);
        }

        public void  SendUsuario(String usuario, String msg)
        {
            Clients.User(usuario).broadcastMessage("Service", msg);
        }

        public void SendLista(List<string> nombreUsuarios, String msg)
        {
            foreach (string user in nombreUsuarios)
            {
                Clients.Group(user).broadcastMessage("Service", msg);
            }
        }
        

        public override Task OnConnected()
        {
            var userName = new Hubs.IdentificadorSignalR().GetUserId(Context.Request);
            Groups.Add(Context.ConnectionId, userName);

            return base.OnConnected();
        }

        public override Task OnReconnected()
        {
            return base.OnReconnected();
        }


        public override Task OnDisconnected(bool stopCalled)
        {
            return base.OnDisconnected(stopCalled);
        }

    }
}