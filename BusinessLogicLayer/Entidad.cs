using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer
{
    public class Entidad
    {

        public Entidad()
        {

        }

        public void DesdeTipo(Shared.Entities.TipoEntidad te)
        {
            this.tipo_id = te.Id;
            this.ataque = te.Ataque.GetValueOrDefault();
            this.defensa = te.Defensa.GetValueOrDefault();
            this.hp = te.Vida.GetValueOrDefault();
        }

        [JsonProperty(propertyName: "Unit_id")]
        public string id { get; set; }

        [JsonProperty(propertyName: "Id")]
        public int tipo_id { get; set; }

        [JsonProperty(propertyName: "jugador")]
        public string jugador { get; set; }

        [JsonProperty(propertyName: "ataque")]
        public int ataque { get; set; } = 10;

        [JsonProperty(propertyName: "hp")]
        public float hp { get; set; } = 100;

        [JsonProperty(propertyName: "defensa")]
        public int defensa { get; set; } = 10;

        [JsonProperty(propertyName: "rango")]
        public int rango { get; set; } = 8;

        [JsonIgnore()]
        public float posX { get; set; } // posicion exacta

        [JsonIgnore()]
        public float posY { get; set; }  // posicion exacta


        [JsonProperty(propertyName: "PosX")]
        public int posXr
        {
            get { return (int)Math.Round(posX); }
        }

        [JsonProperty(propertyName: "PosY")]
        public int posYr
        {
            get { return (int)Math.Round(posY); }
        }

        public string target { get; set; } = null;

        public virtual bool estaViva
        {
            get { return hp >= 0; }
        }



        public float distancia(Entidad otra)
        {
            return (posX - otra.posX) * (posX - otra.posX) + (posY - otra.posY) * (posY - otra.posY);
        }

        public float distancia2(Entidad otra)
        {
            return (posX - otra.posX) * (posX - otra.posX) + (posY - otra.posY) * (posY - otra.posY);
        }

        public bool enRango(Entidad enemiga)
        {
            return distancia2(enemiga) <= this.rango * this.rango;
        }

    }
}
