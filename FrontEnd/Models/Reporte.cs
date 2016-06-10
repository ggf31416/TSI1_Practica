using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FrontEnd.Models
{
    public class Reporte
    {
        public List<FechaCantidad> registros { get; set; }
        public List<FechaCantidad> sesiones { get; set; }
    }
}