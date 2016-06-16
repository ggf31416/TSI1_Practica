using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FrontEnd.Models
{
    public class TributoModel
    {
        public string IdJugadorDestino { get; set; }
        public List<RecursoAsociado> Valores { get; set; }
    }
}