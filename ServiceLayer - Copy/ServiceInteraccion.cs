using BusinessLogicLayer;
using Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using FrontEnd;
using Microsoft.AspNet.SignalR;

namespace ServiceLayer
{
    [ServiceBehavior(IncludeExceptionDetailInFaults = true)]
    public class ServiceInteraccion : IServiceInteraccion
    {

        public ServiceInteraccion(){}

        public void Send(String msg)
        {
            GlobalHost.DependencyResolver.UseRedis("40.84.2.155", 6379, "gabilo2016!", "ChatChannel");

            var context = GlobalHost.ConnectionManager.GetHubContext<ChatHub>();

            context.Clients.All.broadcastMessage("Service", msg);
        }
    }
}
