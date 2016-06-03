using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer
{
    public class AccionMsg
    {
        /* 
         * propertyName es el nombre de la propiedad en el JSON
         * si no se me pasa la propiedad el valor es null o default(tipo) si no es nullable
         * 
         */

        [JsonProperty(propertyName: "A")]
        public string Accion { get; set; }

        [JsonProperty(propertyName: "J")]
        public string Jugador { get; set; }

        [JsonProperty(propertyName: "Id")]
        public int Id { get; set; }

        [JsonProperty(propertyName: "IdUn")]
        public string IdUnidad { get; set; }

        [JsonProperty(propertyName: "PosX")]
        public int PosX { get; set; }

        [JsonProperty(propertyName: "PosY")]
        public int PosY { get; set; }

        [JsonProperty(propertyName: "VN")]
        public float ValorN { get; set; }

    }

    public class AccionMoverUnidad : AccionMsg
    {
        public EpPathFinding.GridPos Path { get; set; }
    }
}
