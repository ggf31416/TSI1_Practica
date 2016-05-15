using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FrontEnd.Models
{
    public class TipoEntidadModel
    {
        public string Nombre { get; set; }
        public int Id { get; set; }
        public Nullable<int> Vida { get; set; }
        public Nullable<int> Ataque { get; set; }
        public Nullable<int> Defensa { get; set; }
        public string Imagen { get; set; }
        public Nullable<int> TiempoConstruccion { get; set; }
    }
}