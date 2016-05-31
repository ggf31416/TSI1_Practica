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
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult login(Cliente cliente)
        {
            try
            {
                ServiceTableroClient client = new ServiceTableroClient();
                Shared.Entities.Cliente cli = new Shared.Entities.Cliente();
                client.login(cliente);
                return Json(new { sucess = true });
            }
            catch (Exception e)
            {
                return Json(new { sucess = false });
            }
        }

    }
}