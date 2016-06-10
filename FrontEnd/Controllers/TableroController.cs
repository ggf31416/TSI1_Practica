using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ServiceStack.Redis;
using Microsoft.AspNet.SignalR;
using FrontEnd.ServiceTablero;
using Shared.Entities;

namespace FrontEnd.Controllers
{
    public class TableroController : Controller
    {

        [HttpPost]
        public ActionResult Accion(string data)
        {
            try
            {
                ServiceTableroClient client = new ServiceTableroClient();
                client.Accion(data);
                return Json(new { sucess = true });
            }
            catch (Exception e)
            {
                return Json(new { sucess = false });
            }
        }


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

        [HttpPost]
        public ActionResult IniciarAtaque(InfoAtaque info)
        {
            try
            {
                ServiceTableroClient client = new ServiceTableroClient();
                client.IniciarAtaque(info);
                return Json(new { sucess = true });
            }
            catch (Exception e)
            {
                return Json(new { sucess = false });
            }
        }


        [HttpGet]
        public ActionResult GetListaDeJugadoresAtacables(string jugador)
        {
            try
            {
                ServiceTableroClient client = new ServiceTableroClient();
                var res = client.GetListaDeJugadoresAtacables(jugador);
                return Json(new { success = true, ret = res }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { sucess = false });
            }
        }


        [HttpPost]
        public ActionResult ConstruirEdificio(Models.CEInputDataModel ceid)
        {
            try
            {
                ServiceTableroClient client = new ServiceTableroClient();

                CEInputData ceInputData = new CEInputData();

                ceInputData.IdTipoEdificio = ceid.IdTipoEdificio;
                ceInputData.PosFila = ceid.PosFila;
                ceInputData.PosColumna = ceid.PosColumna;

                bool ret = client.ConstruirEdificio(ceInputData);
                
                return Json(new { sucess = true, ret = ret});
            }
            catch(Exception e)
            {
                return Json(new { sucess = false });
            }
        }

        [HttpPost]
        public ActionResult EntrenarUnidad(Models.EUInputDataModel euid)
        {
            try
            {
                ServiceTableroClient client = new ServiceTableroClient();

                EUInputData euInputData = new EUInputData();

                euInputData.IdTipoUnidad = euid.IdTipoUnidad;
                euInputData.Cantidad = euid.Cantidad;

                int ret = client.EntrenarUnidad(euInputData);

                return Json(new { sucess = true, ret = ret });
            }
            catch (Exception e)
            {
                return Json(new { sucess = false });
            }
        }

    }

}