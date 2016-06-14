using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using Microsoft.AspNet.SignalR;

[assembly: OwinStartup(typeof(FrontEnd.Startup))]

namespace FrontEnd
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            //GlobalHost.DependencyResolver.UseRedis("40.84.2.155", 6379, "gabilo2016!", "ChatChannel");
            GlobalHost.DependencyResolver.Register(typeof(IUserIdProvider), () => new Hubs.IdentificadorSignalR());
            app.MapSignalR();
            
        }
    }
}
