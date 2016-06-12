using FrontEnd.Models;
using FrontEnd.ServiceTablero;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FrontEnd.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index(string tenant)
        {
            ViewBag.Tenant = tenant;
            return View();
        }


        [HttpPost]
        public ActionResult login(string tenant, ClienteJuego clienteJuego)
        {
            try
            {
                clienteJuego.idJuego = tenant;
                ServiceTableroClient client = new ServiceTableroClient();
                Shared.Entities.ClienteJuego cli = new Shared.Entities.ClienteJuego();
                cli.clienteId = clienteJuego.clienteId;
                cli.token = clienteJuego.token;
                bool result = client.login(cli, clienteJuego.idJuego);
                return Json(new { status = result });
            }
            catch (Exception e)
            {
                return Json(new { status = false });
            }
        }

        [HttpPost]
        public ActionResult register(string tenant, ClienteJuego clienteJuego)
        {
            try
            {
                clienteJuego.idJuego = tenant;
                ServiceTableroClient client = new ServiceTableroClient();
                Shared.Entities.ClienteJuego cli = new Shared.Entities.ClienteJuego();
                cli.clienteId = clienteJuego.clienteId;
                cli.token = clienteJuego.token;
                cli.apellido = clienteJuego.apellido;
                cli.nombre = clienteJuego.nombre;
                cli.username = clienteJuego.username;
                client.register(cli, clienteJuego.idJuego);
                return Json(new { status = true });
            }
            catch (Exception e)
            {
                return Json(new { status = false });
            }
        }


        // GET: Aldea
        public ActionResult Aldea(string tenant)
        {
            ViewBag.Tenant = tenant;
            return View();
        }

    }
}