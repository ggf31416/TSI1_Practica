﻿using BusinessLogicLayer;
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
using Microsoft.AspNet.SignalR.Client;

namespace ServiceLayer
{
    [ServiceBehavior(IncludeExceptionDetailInFaults = true)]
    public class ServiceInteraccion : IServiceInteraccion
    {
        IHubProxy proxy;
        public ServiceInteraccion(){
            //http://www.asp.net/signalr/overview/guide-to-the-api/hubs-api-guide-net-client
            var hubConnection = new HubConnection("http://localhost:56927/");
           proxy = hubConnection.CreateHubProxy("ChatHub");
            //proxy.On<Stock>("UpdateStockPrice", stock => Console.WriteLine("Stock update for {0} new price {1}", stock.Symbol, stock.Price));
            hubConnection.Start().Wait();
        }

        public void Send(String msg)
        {
            //GlobalHost.DependencyResolver.UseRedis("40.84.2.155", 6379, "gabilo2016!", "ChatChannel");


            //var context = GlobalHost.ConnectionManager.GetHubContext<ChatHub>();
            proxy.Invoke("send", "Service",msg).Wait();
            //context.Clients.All.broadcastMessage("Service", msg);
        }
    }
}
