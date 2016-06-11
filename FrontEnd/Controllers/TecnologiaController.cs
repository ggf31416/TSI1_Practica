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

        [HttpGet]
        public ActionResult GetTecnologiasDesarrollables(string tenant,string idJugador)
        {
            
        }
    }
}