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
using Microsoft.AspNet.SignalR.Client;
using System.Diagnostics;

namespace ServiceLayer
{
    [ServiceBehavior(IncludeExceptionDetailInFaults = true)]
    public class ServiceInteraccion : IServiceInteraccion
    {
        IHubProxy proxy;
        IHubContext HubContext;
        bool usoRedis = false;

        public ServiceInteraccion()
        {
            try
            {
                if (usoRedis)
                {
                   GlobalHost.DependencyResolver.UseRedis("40.84.2.155", 6379, "gabilo2016!", "ChatChannel");
                   HubContext = GlobalHost.ConnectionManager.GetHubContext<ChatHub>();
                }
                else
                {
                    //http://www.asp.net/signalr/overview/guide-to-the-api/hubs-api-guide-net-client
                    var hubConnection = new HubConnection("http://localhost:56927/");
                    // proxy si quiero connecciones locales
                    proxy = hubConnection.CreateHubProxy("ChatHub");

                    hubConnection.Start().Wait();
                }
            }
            catch (Exception ex)
            {

            }

        }

        public void SendGrupo(String grupo, String msg)
        {
            Task.Factory.StartNew(() =>
            {
                try
                {
                    if (usoRedis)
                    {
                        HubContext.Clients.Group(grupo).broadcastMessage("Service", msg);
                    }
                    else
                    {
                        proxy.Invoke("SendGrupo", grupo, msg).Wait();
                    }
                }
                catch (TimeoutException toEx)
                {
                    Console.WriteLine("Timeout signlar Date " + DateTime.Now.ToShortTimeString() + " msg: " + msg);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Ocurrio un error al enviar signalr: " + ex.ToString());
                }
            });
        }


        public void SendLista(List<string> nombreUsuarios, String msg)
        {
            
                try
                {
                    if (usoRedis)
                    {
                        Stopwatch sw = Stopwatch.StartNew();

                        Debug.WriteLine("Obtener context demoro " + sw.ElapsedMilliseconds);
                        sw.Restart();
                        Stopwatch sw2 = Stopwatch.StartNew();
                        foreach (string user in nombreUsuarios)
                        {
                            sw.Restart();

                            HubContext.Clients.Group(user).broadcastMessage("Service", msg);
                            Debug.WriteLine("Enviar para  " + user + " demoro " + sw.ElapsedMilliseconds);
                        }
                        Debug.WriteLine("Enviar demoro " + sw2.ElapsedMilliseconds);
                    }
                    else
                    {
                        proxy.Invoke("SendLista", nombreUsuarios, msg).Wait();

                    }

                }
                catch (TimeoutException toEx)
                {
                    Console.WriteLine("Timeout signlar Date " + DateTime.Now.ToShortTimeString() + " msg: " + msg);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Ocurrio un error al enviar signalr: " + ex.ToString());
                }
            
           
        }


        public void SendUsuario(String usuario, String msg)
        {
            try
            {
                if (usoRedis)
                {
                    var context = GlobalHost.ConnectionManager.GetHubContext<ChatHub>();
                    context.Clients.User(usuario).broadcastMessage("Service", msg);
                }
                else
                {
                    proxy.Invoke("SendUsuario", usuario, msg).Wait();
                }             
            }
            catch (TimeoutException toEx)
            {
                Console.WriteLine("Timeout signlar Date " + DateTime.Now.ToShortTimeString() + " msg: " + msg);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ocurrio un error al enviar signalr: " + ex.ToString());
            }

        }



        public void Send(String msg)
        {
            try
            {
                if (usoRedis)
                {
                    var context = GlobalHost.ConnectionManager.GetHubContext<ChatHub>();
                    context.Clients.All.broadcastMessage("Service", msg);
                }
                else
                {
                    proxy.Invoke("send", "Service", msg).Wait();
                }
            }
            catch (TimeoutException toEx)
            {
                Console.WriteLine("Timeout signlar Date " + DateTime.Now.ToShortTimeString() + " msg: " + msg);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ocurrio un error al enviar signalr: " + ex.ToString());
            }

            //
        }
    }
}
