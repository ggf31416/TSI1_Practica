using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Entities
{
    public class AccionModificarAtributo : Accion
    {
        public Nullable<int> ValorPor { get; set; }
        public Nullable<int> Valor { get; set; }
        public Nullable<int> IdEntidad { get; set; }
    }
}
