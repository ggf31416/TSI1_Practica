using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FrontEnd.Models
{
    public class ClienteJuego
    {
        public int clienteId { get; set; }
        public string token { get; set; }
        public int idJuego { get; set; }
    }
}