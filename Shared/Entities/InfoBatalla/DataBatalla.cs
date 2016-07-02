using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Entities.DataBatalla
{
    public class InfoBatalla
    {
        public string A { get; set; } = "IniciarAtaque";
        public bool Finalice { get; set; } = false;
        public List<String> unidades { get; set; } = new List<String>();
        public List<String> edificios { get; set; } = new List<String>();
        public List<TipoUnidad> tiposUnidad { get; set; } = new List<TipoUnidad>();
        public List<TipoEdificio> tiposEdificio { get; set; } = new List<TipoEdificio>();
        public Dictionary<string, InfoJugador> jugadores { get; set; } = new Dictionary<string, InfoJugador>();

        public string IdJugador { get; set; }
        public string ShortId { get; set; }

    }

    public class InfoJugador
    {
        public string Id { get; set; }
        public string ShortId { get; set; }
        public string Clan { get; set; }
        public List<ConjuntoUnidades> Unidades { get; set; } = new List<ConjuntoUnidades>();
        //public List<Edificio> Edificios { get; set; } = new List<Edificio>();
        public Dictionary<String, InfoTipo> Tipos { get; set; } = new Dictionary<String, InfoTipo>();
        //public Dictionary<int, CantidadRecurso> Recursos { get; set; }  // clave Recurso.ID
    }

    public class InfoTipo
    {
        public int id { get; set; }
        public float hp { get; set; }
        public int ataque { get; set; }
        public int rango { get; set; }
        public bool esUnidad { get; set; }
    }



}
