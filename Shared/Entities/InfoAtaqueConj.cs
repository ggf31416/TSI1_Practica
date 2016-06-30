using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Entities
{
    public class InfoAtaqueConj
    {
        public string Tenant { get; set; }
        public string Defensor { get; set; }
        public string Atacante { get; set; }
        public List<String> AlianzaAtacante { get; set; } = new List<string>();
        public List<String> AlianzaDefensora { get; set; } = new List<string>();
        // unidades que no son del defensor (el defensor usa todas las que tenga disponibles)
        public List< Contribucion> UnidadesContribuidas { get; set; } = new List<Contribucion>();

    }

    public class Contribucion
    {
        public string IdJugador { get; set; }
        public List<ConjuntoUnidades> UnidadesContribuidas { get; set; } = new List<ConjuntoUnidades>();
    }
}

