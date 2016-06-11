using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FrontEnd.Models
{
    public class DataActualModel
    {
        public Dictionary<int, EstadoDataModel> EstadoUnidades { get; set; }
        public Dictionary<int, EstadoDataModel> EstadoTecnologias { get; set; }
        public Dictionary<int, EstadoRecursoModel> EstadoRecursos { get; set; }
        public DateTime UltimaActualizacion { get; set; }
    }
}