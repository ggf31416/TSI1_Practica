using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FrontEnd.Models
{
    public class ClienteJuego
    {
        public string clienteId { get; set; }
        public string token { get; set; }
        public int idJuego { get; set; }
        public string username { get; set; }
        public string nombre { get; set; }
        public string apellido { get; set; }
    }
}