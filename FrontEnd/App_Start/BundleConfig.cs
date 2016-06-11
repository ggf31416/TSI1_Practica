using System.Web;
using System.Web.Optimization;

namespace FrontEnd
{
    public class BundleConfig
    {
        // Para obtener más información sobre Bundles, visite http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Utilice la versión de desarrollo de Modernizr para desarrollar y obtener información. De este modo, estará
            // preparado para la producción y podrá utilizar la herramienta de compilación disponible en http://modernizr.com para seleccionar solo las pruebas que necesite.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));



            bundles.Add(new ScriptBundle("~/bundles/angular").Include(
                        "~/Scripts/angular.min.js",
                        "~/Scripts/angular-route.min.js",
                        "~/SPA/frontOffice/modulos.js",
                        "~/SPA/frontOffice/services/juego.service.js",
                        "~/SPA/frontOffice/controllers/juego.controller.js",
                        "~/SPA/frontOffice/services/recursos.service.js",
                        "~/SPA/frontOffice/controllers/recurso.controller.js",
                        "~/SPA/frontOffice/services/edificios.service.js",
                        "~/SPA/frontOffice/controllers/edificio.controller.js",
                        "~/SPA/frontOffice/services/unidades.service.js",
                        "~/SPA/frontOffice/controllers/unidad.controller.js",
                        "~/SPA/frontOffice/models/unidad.js",
                        "~/SPA/frontOffice/services/aldeas.service.js",
                        "~/SPA/frontOffice/controllers/aldea.controller.js",
                        "~/SPA/frontOffice/services/login.service.js",
                        "~/SPA/frontOffice/controllers/login.controller.js",
                        "~/SPA/app.js"));
        }
    }
}
