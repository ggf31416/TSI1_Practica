using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace FrontEnd.Hubs
{
    public class AtlasHub : Hub
    {
        
        // Este metodo sirve para hacer broadcasts iniciados desde el servidor
        public void EnviarMensajeBroadcast(string motivo,string msg)
        {
            var context = GlobalHost.ConnectionManager.GetHubContext<AtlasHub>();
            /*
             * recibirMensaje es el nombre del metodo javascript que debe estar presente
             * para recibir el mensaje;
            */
            context.Clients.All.recibirMensaje(motivo, msg);
        }

        // este metodo es para broadcasts iniciados desde el cliente
        public void BroadcastDesdeCliente(string motivo, string msg)
        {
            Clients.All.recibirMensaje(motivo, msg);
        }
    }
}