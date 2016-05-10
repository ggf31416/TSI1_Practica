using FrontEnd.ServiceEntidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FrontEnd.Controllers
{
    public class EntidadesController : Controller
    {
        // GET: Entidades
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult GetAllTipoEdificios()
        {
            try
            {
                ServiceEntidadesClient client = new ServiceEntidadesClient();

                 var ret = new List<TipoEntidad>(client.GetAllTipoEdificios());
                return Json(new { success = true, responseText = "Edificios: ", ret = ret, cant = ret.Count }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                Console.WriteLine("An error occurred: '{0}'", e);
                return Json(new { success = false, responseText = "Error al obtener los TipoEdificios! " + e.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult GetAllTipoUnidades()
        {
            try
            {
                ServiceEntidadesClient client = new ServiceEntidadesClient();
                var ret = new List<TipoUnidad>(client.GetAllTipoUnidades());
                
                return Json(new { success = true, responseText = "Unidades: ", ret = ret, cant = ret.Count }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                Console.WriteLine("An error occurred: '{0}'", e);
                return Json(new { success = false, responseText = "Error al obtener los TipoUnidades! " + e.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}