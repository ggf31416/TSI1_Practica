using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace FrontEnd.Hubs
{
    public class IdentificadorSignalR : IUserIdProvider
    {
        public string GetUserId(IRequest request)
        {
            if (request.Cookies.ContainsKey("clienteId"))
            {
                var cookie = request.Cookies["clienteId"];
                string clienteId = cookie.Value;
                Debug.WriteLine("[WARNING] GetUserId para cliente " + clienteId);
                return clienteId;
            }
            Debug.WriteLine("[WARNING] GetUserId para jugador no autenticado");
            return "NO_AUTENTICADO";
        }
    }
}