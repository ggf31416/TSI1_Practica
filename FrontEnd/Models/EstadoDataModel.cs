using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FrontEnd.Models
{
    public class EstadoDataModel
    {
        public Shared.Entities.EstadoData.EstadoEnum Estado { get; set; }
        public int Id { get; set; }
        public int Tiempo { get; set; }
        public int Cantidad { get; set; }
    }
}