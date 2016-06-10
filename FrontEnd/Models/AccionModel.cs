using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FrontEnd.Models
{
    public class AccionModel
    {
        public string Nombre { get; set; }
        public int Id { get; set; }
        public int IdJuego { get; set; }
        public int IdTecnologia { get; set; }
        public string NombreAtributo { get; set; }
        public Nullable<int> ValorPor { get; set; }
        public Nullable<int> Valor { get; set; }
        public Nullable<int> IdEntidad { get; set; }
    }
}