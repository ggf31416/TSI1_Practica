using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FrontEnd.Models
{
    public class ListasEntidadesModel
    {
        public List<TipoEntidadModel> TipoEdificios { get; set; }
        public List<TipoEntidadModel> TipoUnidades { get; set; }
    }
}