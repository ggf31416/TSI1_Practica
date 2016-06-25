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

        // GET: LoginFacebook
        public ActionResult LoginFacebook(string tenant)
        {
            ViewBag.Tenant = tenant;
            return View();
        }
        
        // GET: Aldea
        public ActionResult Aldea(string tenant)
        {
            ViewBag.Tenant = tenant;
            return View();
        }

        [HttpPost]
        public ActionResult login(string tenant, ClienteJuego clienteJuego)
        {
            try
            {
                //clienteJuego.idJuego = tenant;
                clienteJuego.idJuego = clienteJuego.idJuego;
                ServiceTableroClient client = new ServiceTableroClient();
                Shared.Entities.ClienteJuego cli = new Shared.Entities.ClienteJuego();
                cli.clienteId = clienteJuego.clienteId;
                cli.token = clienteJuego.token;

                HttpCookie myCookie = new HttpCookie("clienteId");
                DateTime now = DateTime.UtcNow;

                myCookie.Value = cli.clienteId;
                myCookie.Expires = now.AddMonths(1);

                Response.Cookies.Add(myCookie);

                HttpCookie token = new HttpCookie("token");

                token.Value = cli.token;
                token.Expires = now.AddMonths(1);

                Response.Cookies.Add(token);

                //bool result = client.login(cli, clienteJuego.idJuego);
                bool result = client.login(cli, tenant);
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
                clienteJuego.idJuego = clienteJuego.idJuego;
                //clienteJuego.idJuego = tenant;
                ServiceTableroClient client = new ServiceTableroClient();
                Shared.Entities.ClienteJuego cli = new Shared.Entities.ClienteJuego();
                cli.clienteId = clienteJuego.clienteId;
                cli.token = clienteJuego.token;

                HttpCookie myCookie = new HttpCookie("clienteId");
                DateTime now = DateTime.UtcNow;

                myCookie.Value = cli.clienteId;
                myCookie.Expires = now.AddMonths(1);

                Response.Cookies.Add(myCookie);

                HttpCookie token = new HttpCookie("token");

                token.Value = cli.token;
                token.Expires = now.AddMonths(1);

                Response.Cookies.Add(token);


                cli.apellido = clienteJuego.apellido;
                cli.nombre = clienteJuego.nombre;
                cli.username = clienteJuego.username;
                //client.register(cli, clienteJuego.idJuego);
                client.register(cli, tenant);
                return Json(new { status = true });
            }
            catch (Exception e)
            {
                return Json(new { status = false });
            }
        }
    }
}