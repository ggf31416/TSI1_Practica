using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Entities
{
    public class UsuarioEstadisticas
    {
        public int id { get; set; }
        public List<FechaCantidad> registros;
        public List<FechaCantidad> sesiones;

        public UsuarioEstadisticas()
        {
            this.id = 1;
        }
    }
}
