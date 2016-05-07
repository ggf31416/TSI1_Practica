using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
//using System.Net.Http;
//using System.Net.Http.Headers;
using System.Web;
using System.Web.Mvc;

namespace FrontEnd.Controllers
{
    public class EntidadesController : Controller
    {
        // GET: Edificios
        public ActionResult Index()
        {
            return View("Error");
        }

        [HttpPost]
        public ActionResult AltaTipoEdificio(Models.TipoEntidadModel tipoEdificio)
        {
            try
            {
                ServiceEntidadesClient client = new ServiceEntidadesClient();

                Shared.Entities.TipoEntidad newTipoEntidad = new Shared.Entities.TipoEdificio();

                newTipoEntidad = CopyEntity(tipoEdificio, newTipoEntidad);

                client.AltaTipoEdificio(newTipoEntidad);

                String msg;
                if (tipoEdificio.Id.Equals(0))
                {
                    msg = "Edificio Ingresado";
                }
                else
                {
                    msg = "Edificio Updateado";
                }

                return Json(new { success = true, responseText = msg }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                Console.WriteLine("An error occurred: '{0}'", e);
                return Json(new { success = false, responseText = "Error al ingresar Edificio!", error = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult AltaTipoUnidad(Models.TipoEntidadModel tipoUnidad)
        {
            try
            {
                ServiceEntidadesClient client = new ServiceEntidadesClient();

                Shared.Entities.TipoEntidad newTipoEntidad = new Shared.Entities.TipoUnidad();

                newTipoEntidad = CopyEntity(tipoUnidad, newTipoEntidad);

                client.AltaTipoUnidad(newTipoEntidad);

                String msg;
                if (tipoUnidad.Id.Equals(0))
                {
                    msg = "Unidad Ingresado";
                }
                else
                {
                    msg = "Unidad Updateado";
                }

                return Json(new { success = true, responseText = msg }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                Console.WriteLine("An error occurred: '{0}'", e);
                return Json(new { success = false, responseText = "Error al ingresar Unidad!", error = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        private Shared.Entities.TipoEntidad CopyEntity(Models.TipoEntidadModel tipoEntidadOrigen, Shared.Entities.TipoEntidad tipoEntidadDestino)
        {
            tipoEntidadDestino.Nombre = tipoEntidadOrigen.Nombre;
            tipoEntidadDestino.Id = tipoEntidadOrigen.Id;
            tipoEntidadDestino.Vida = tipoEntidadOrigen.Vida;
            tipoEntidadDestino.Ataque = tipoEntidadOrigen.Ataque;
            tipoEntidadDestino.Defensa = tipoEntidadOrigen.Defensa;
            tipoEntidadDestino.Imagen = tipoEntidadOrigen.Imagen;
            tipoEntidadDestino.TiempoConstruccion = tipoEntidadOrigen.TiempoConstruccion;

            return tipoEntidadDestino;
        }

        [HttpGet]
        public ActionResult GetAllTipoEdificios()
        {
            try
            {
                ServiceEntidadesClient client = new ServiceEntidadesClient();

                Shared.Entities.TipoEdificio[] listaTipoEdificios = client.GetAllTipoEdificios();

                List<Models.TipoEntidadModel> ret = new List<Models.TipoEntidadModel>();
                foreach (Shared.Entities.TipoEdificio tE in listaTipoEdificios)
                {
                    Models.TipoEntidadModel newTipoEntidad = new Models.TipoEntidadModel();

                    newTipoEntidad.Id = tE.Id;
                    newTipoEntidad.Nombre = tE.Nombre;
                    newTipoEntidad.Vida = tE.Vida;
                    newTipoEntidad.Defensa = tE.Defensa;
                    newTipoEntidad.Imagen = tE.Imagen;
                    newTipoEntidad.Ataque = tE.Ataque;
                    newTipoEntidad.TiempoConstruccion = tE.TiempoConstruccion;

                    ret.Add(newTipoEntidad);
                }

                return Json(new { success = true, responseText = "Edificios: ", ret = ret, cant = ret.Count }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                Console.WriteLine("An error occurred: '{0}'", e);
                return Json(new { success = false, responseText = "Error al obtener los TipoEdificios! " + e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult GetAllTipoUnidades()
        {
            try
            {
                ServiceEntidadesClient client = new ServiceEntidadesClient();

                Shared.Entities.TipoUnidad[] listaTipoEdificios = client.GetAllTipoUnidades();

                List<Models.TipoEntidadModel> ret = new List<Models.TipoEntidadModel>();
                foreach (Shared.Entities.TipoUnidad tE in listaTipoEdificios)
                {
                    Models.TipoEntidadModel newTipoEntidad = new Models.TipoEntidadModel();

                    newTipoEntidad.Id = tE.Id;
                    newTipoEntidad.Nombre = tE.Nombre;
                    newTipoEntidad.Vida = tE.Vida;
                    newTipoEntidad.Defensa = tE.Defensa;
                    newTipoEntidad.Imagen = tE.Imagen;
                    newTipoEntidad.Ataque = tE.Ataque;
                    newTipoEntidad.TiempoConstruccion = tE.TiempoConstruccion;

                    ret.Add(newTipoEntidad);
                }

                return Json(new { success = true, responseText = "Unidades: ", ret = ret, cant = ret.Count }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                Console.WriteLine("An error occurred: '{0}'", e);
                return Json(new { success = false, responseText = "Error al obtener los TipoUnidades! " + e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult AsociarEdificioUnidad(Models.EdificioUnidadModel edificioUnidad)
        {
            try
            {
                ServiceEntidadesClient client = new ServiceEntidadesClient();

                Shared.Entities.TipoEdificio sharedTipoEdificio = new Shared.Entities.TipoEdificio();
                Shared.Entities.TipoUnidad sharedTipoUnidad = new Shared.Entities.TipoUnidad();

                sharedTipoEdificio.Id = edificioUnidad.IdEdificio;
                sharedTipoUnidad.Id = edificioUnidad.IdUnidad;

                client.AsociarEdificioUnidad(sharedTipoEdificio, sharedTipoUnidad);

                return Json(new { success = true, responseText = "Edificio y Unidad Asociados" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                Console.WriteLine("An error occurred: '{0}'", e);
                return Json(new { success = false, responseText = "Error al Asociar Edificio con Unidad!", error = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult uploadImage(HttpPostedFileBase file)
        {
            try
            {
                var fileName = Path.GetFileName(file.FileName);
                // store the file inside ~/App_Data/uploads folder
                var path = Path.Combine(Server.MapPath("~/SPA/backOffice/ImagenesSubidas"), fileName);
                file.SaveAs(path);

                return Json(new { success = true, responseText = "Imagen subida con exito" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                Console.WriteLine("An error occurred: '{0}'", e);
                return Json(new { success = false, responseText = "Error al ingresar Entidad!", error = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}