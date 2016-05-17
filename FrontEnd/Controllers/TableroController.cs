using FrontEnd.ServiceEntidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ServiceStack.Redis;
using Microsoft.AspNet.SignalR;

namespace FrontEnd.Controllers
{
    public class TableroController : Controller
    {

        [HttpPost]
        public ActionResult JugarUnidad(Models.InfoCeldaModel infoCelda)
        {
            try
            {
                ServiceTableroClient client = new ServiceTableroClient();

                Shared.Entities.InfoCelda sharedInfoCelda = new Shared.Entities.InfoCelda();

                sharedInfoCelda.Id = infoCelda.Id;
                sharedInfoCelda.PosX = infoCelda.PosX;
                sharedInfoCelda.PosY = infoCelda.PosY;

                client.JugarUnidad(sharedInfoCelda);

                //var redisClient = new RedisClient("40.84.2.155", 6379, "gabilo2016!");

                //redisClient.PublishMessage("ChatChannel", "HOLAHOLA");

                //var context = GlobalHost.ConnectionManager.GetHubContext<ChatHub>();

                //context.Clients.All.broadcastMessage("chuareService", "koalaService");

                return Json(new { success = true,
                                  responseText = "Unidad Jugada",
                                  ID = sharedInfoCelda.Id,
                                  PosX = sharedInfoCelda.PosX,
                                  PosY = sharedInfoCelda.PosY
                                },
                                JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                Console.WriteLine("An error occurred: '{0}'", e);
                return Json(new { success = false, responseText = "Error al Jugar Unidad! " + e.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }

    }
}