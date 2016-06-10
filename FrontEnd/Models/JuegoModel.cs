using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FrontEnd.Models
{
    public class JuegoModel
    {
        public string Nombre { get; set; }
        public int Id { get; set; }
        public string Imagen { get; set; }
        public int Estado { get; set; }
        public string Url { get; set; }
        public int IdDisenador { get; set; }
    }
}