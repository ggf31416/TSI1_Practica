using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace FrontEnd
{
    public class ChatHub : Hub
    {
        public void send(string name, string message)
        {
            Console.WriteLine(name + " -> " + message);
            Clients.All.broadcastMessage(name, message);
            Clients.Caller.broadcastMessage(name, "mensaje enivado");
        }

        public void agregarUsuarioGrupo(string idUsuario,string grupo)
        {
            Groups.Add(idUsuario, grupo);
        }
    }
}