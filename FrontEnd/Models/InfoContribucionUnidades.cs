using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FrontEnd.Models
{
    public class InfoContribucionUnidades
    {
        public string IdDefensor { get; set; }
        public List<Shared.Entities.ConjuntoUnidades> Unidades { get; set; }
    }
}