using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FrontEnd.Controllers
{
    public class TecnologiaController : Controller
    {
        // GET: Tecnologia
        public ActionResult Index()
        {
            return View();
        }

        public class JugadorElemento
        {
            public int IdElemento { get; set; }
        }

        [HttpPost]
        public ActionResult DesarrollarTecnologia(string tenant, JugadorElemento data)
        {
            try
            {
                string idJugador = Request.Cookies["clienteId"].Value;
                ServiceTableroClient client = new ServiceTableroClient();
                bool puede =  client.DesarrollarTecnologia(tenant, idJugador,data.IdElemento);
                return Json(new { sucess = puede });
            }
            catch (Exception e)
            {
                return Json(new { sucess = false });
            }
        }
    }
}