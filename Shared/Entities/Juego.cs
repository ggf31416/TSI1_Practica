using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Entities
{
    public class Juego
    {

        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Imagen { get; set; }
        public string Url { get; set; }

        //public ICollection<Accion> accions { get; set; }
        //public ICollection<arbol_tecnologias> arbol_tecnologias { get; set; }
        //public  ICollection<arbol_tecnologias> arbol_tecnologias1 { get; set; }
        //public  ICollection<arbol_tecnologias> arbol_tecnologias2 { get; set; }
        //public  ICollection<Raza> razas { get; set; }
        //public  ICollection<Tecnologia> tecnologias { get; set; }
        public  ICollection<TipoEntidad> tipo_entidad { get; set; }
        //public  ICollection<TipoRecurso> tipo_recurso { get; set; }

    }
}
