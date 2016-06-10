using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FrontEnd.Hubs
{
    public class IdentificadorSignalR : IUserIdProvider
    {
        public string GetUserId(IRequest request)
        {
            var cookie = request.Cookies["clienteId"];
            string clienteId = cookie.Value;
            return clienteId;
        }
    }
}