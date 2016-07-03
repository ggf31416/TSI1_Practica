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
        private string NO_AUTH;
        private ServiceTableroClient cliente;

        public ChatHub()
        {
            cliente = new ServiceTableroClient();
            NO_AUTH = new Hubs.IdentificadorSignalR().NO_AUTENTICADO;
        }

        public void send(string name, string message)
        {
            Console.WriteLine(name + " -> " + message);
            Clients.All.broadcastMessage(name, message);

            //Clients.Caller.broadcastMessage(name, "mensaje enivado");
        }

        public void SendGrupo(string grupo,string mensaje)
        {
            try
            {
                string nombre = grupo;
                Clients.Group(grupo).broadcastMessage(nombre, mensaje);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError("Error en ChatHub.SendGrupo");
                System.Diagnostics.Trace.TraceError(ex.ToString());
            }
        }

        public void agregarUsuarioGrupo(string idUsuario,string grupo)
        {
            try { 
                Groups.Add(idUsuario, grupo);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError("Error en ChatHub.agregarUsuarioGrupo");
                System.Diagnostics.Trace.TraceError(ex.ToString());
            }
}

        public void  SendUsuario(String usuario, String msg)
        {

            try
            {
                Clients.User(usuario).broadcastMessage("Service", msg);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError("Error en ChatHub.SendUsuario");
                System.Diagnostics.Trace.TraceError(ex.ToString());
            }
        }

        public void SendLista(List<string> nombreUsuarios, String msg)
        {
            try { 
                foreach (string user in nombreUsuarios)
                {
                    Clients.Group(user).broadcastMessage("Service", msg);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError("Error en ChatHub.SendLista");
                System.Diagnostics.Trace.TraceError(ex.ToString());
            }
}


        public override Task OnConnected()
        {
            try { 
                var userName = new Hubs.IdentificadorSignalR().GetUserId(Context.Request);
           
                if (userName != NO_AUTH)
                {
                    Groups.Add(Context.ConnectionId, userName);
                    /*Task.Factory.StartNew(
                        () => cliente.ConectarSignalr("", userName, Context.ConnectionId)
                    );*/
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError("Error en ChatHub.OnConnected");
                System.Diagnostics.Trace.TraceError(ex.ToString());
            }
            return base.OnConnected();
        }

        public override Task OnReconnected()
        {
            try
            {
                var userName = new Hubs.IdentificadorSignalR().GetUserId(Context.Request);
                if (userName != NO_AUTH)
                {
                    /*Task.Factory.StartNew(
                        () => cliente.ReconectarSignalr("", userName, Context.ConnectionId)
                    );*/
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError("Error en ChatHub.OnConnected");
                System.Diagnostics.Trace.TraceError(ex.ToString());
            }
             return base.OnReconnected();
        }


        public override Task OnDisconnected(bool stopCalled)
        {
            try
            {
                var userName = new Hubs.IdentificadorSignalR().GetUserId(Context.Request);
                if (userName != NO_AUTH)
                {
                    cliente.DesconectarSignalr("", userName, Context.ConnectionId);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError("Error en ChatHub.OnConnected");
                System.Diagnostics.Trace.TraceError(ex.ToString());
            }
            return base.OnDisconnected(stopCalled);
        }

    }
}