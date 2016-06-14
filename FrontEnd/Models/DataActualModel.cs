using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FrontEnd.Models
{
    public class DataActualModel
    {
        public Dictionary<string, EstadoDataModel> EstadoUnidades { get; set; }
        public Dictionary<string, EstadoDataModel> EstadoTecnologias { get; set; }
        public Dictionary<string, EstadoRecursoModel> EstadoRecursos { get; set; }
        public DateTime UltimaActualizacion { get; set; }
        public string Clan { get; set; }
    }
}