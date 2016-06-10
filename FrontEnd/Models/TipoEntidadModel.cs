using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FrontEnd.Models
{
    public class TipoEntidadModel
    {
        public int IdJuego { get; set; }
        public string Nombre { get; set; }
        public int Id { get; set; }
        public Nullable<int> Vida { get; set; }
        public Nullable<int> Ataque { get; set; }
        public Nullable<int> Defensa { get; set; }
        public string Imagen { get; set; }
        public Nullable<int> TiempoConstruccion { get; set; }
        public List<Costo> Costos { get; set; }
        public List<Int32> UnidadesAsociadas { get; set; }
        public List<RecursoAsociado> RecursosAsociados { get; set; }
    }
}