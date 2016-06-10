using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FrontEnd.Models
{
    public class TipoRecursoModel
    {
        public string Nombre { get; set; }
        public int Id { get; set; }
        public int IdJuego { get; set; }
        public string Imagen { get; set; }
    }
}