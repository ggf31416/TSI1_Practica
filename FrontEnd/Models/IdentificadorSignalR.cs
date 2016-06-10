using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FrontEnd.Models
{
    public class IdentificadorSignalR : IUserIdProvider
    {
        public string GetUserId(IRequest request)
        {
            throw new NotImplementedException("GetUserId no implementado");
        }
    }
}