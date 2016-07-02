using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Entities
{
    public class AtaqueConjunto
    {
        //[BsonRepresentation(BsonType.ObjectId)]
        [BsonId]
        public string IdBatalla { get; set;}
        public string NombreDefensor { get; set; }
        public string Tenant { get; set; }
        public string Defensor { get; set; }
        public string Atacante { get; set; }
        public string ClanAtacante { get; set; }
        public string ClanDefensor { get; set; }
        public List<String> Jugadores { get; set; } = new List<string>();
        public bool Habilitado { get; set; } = true;
        //public List<String> AlianzaAtacante { get; set; } = new List<string>();
        //public List<String> AlianzaDefensora { get; set; } = new List<string>();
        public DateTime FechaAtaque { get; set; }
        // unidades que no son del defensor (el defensor usa todas las que tenga disponibles)
        //public List< Contribucion> UnidadesContribuidas { get; set; } = new List<Contribucion>();

    }

    public class InfoContribucion
    {
        public string NombreDefensor { get; set; }
        public bool SoyAtacante { get; set; }
        public int CantUnidades { get; set; }
        public int SegundosFaltantes { get; set; }
    }

    public class Contribucion
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Tenant { get; set; }
        public string IdJugador { get; set; }
        public string IdBatalla { get; set; }
        public List<ConjuntoUnidades> UnidadesContribuidas { get; set; } = new List<ConjuntoUnidades>();
        public string NombreDefensor { get; set; }
        public bool SoyAtacante { get; set; }
        public int CantUnidades { get; set; }
        public DateTime FechaAtaque { get; set; }
    }

    public class InfoAtaqConj
    {
        public string IdBatalla { get; set; }
        public string NombreDefensor { get; set; }
        public string Tenant { get; set; }
        public bool SoyClanAtacante { get; set; }
        public int SegundosFaltante { get; set; }
    }


}

