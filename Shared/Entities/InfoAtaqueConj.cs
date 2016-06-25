using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Entities
{
    class InfoAtaqueConj
    {
        public string Defensor { get; set; }
        public string Atacante { get; set; }
        public List<String> AlianzaAtacante { get; set; }
        public List<String> AlianzaDefensora { get; set; }
        // unidades que no son del defensor (el defensor usa todas las que tenga disponibles)
        public Dictionary<String, ConjuntoUnidades> UnidadesContribuidas { get; set; }

    }
}

