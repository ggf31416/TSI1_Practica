using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Entities
{
    public class Raza
    {
        public string Nombre { get; set; }
        public int Id { get; set; }
        public string Imagen { get; set; }
        public Nullable<int> IdJuego { get; set; }

        public virtual Juego Juego { get; set; }
        public virtual ICollection<TipoEdificio> TipoEdificios { get; set; }
        public virtual ICollection<TecnologiaDependencia> TecnologiaDependencias { get; set; }
    }
}
