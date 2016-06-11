using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FrontEnd.Models
{
    public class TecnologiaModel
    {
        public string Nombre { get; set; }
        public int Id { get; set; }
        public string Imagen { get; set; }
        public int TiempoConstruccion { get; set; }
        public int IdEdificio { get; set; }
        public int IdJuego { get; set; }
        public List<AccionModel> AccionesAsociadas { get; set; }
        public List<Costo> Costos { get; set; }
        public List<TecnologiaDependeModel> TecnologiaDependencias { get; set; }
    }
}