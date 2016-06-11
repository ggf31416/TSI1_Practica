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
            string IdJugador;
            int IdElemento;
        }

        [HttpPost]
        public ActionResult DesarrollarTecnologia(string tenant, JugadorElemento data)
        {
            try
            {
                ServiceTableroClient client = new ServiceTableroClient();
                bool puede = false;
                //bool puede =  client.DesarrollarTecnologia(tenant, data);
                return Json(new { sucess = puede });
            }
            catch (Exception e)
            {
                return Json(new { sucess = false });
            }
        }
    }
}