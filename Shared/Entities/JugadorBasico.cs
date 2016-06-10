using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Entities
{
    /***
     * Clase que contiene la info necesaria para presentar una lista de jugadores para atacar
     */
    public class JugadorBasico
    {

        public String Id { get; set; }
        public String Nombre { get; set; }

        public override string ToString()
        {
            return Nombre;
        }
    }
}
