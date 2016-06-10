using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer
{
    public class Entidad
    {
        public string id { get; set; }
        public string jugador { get; set; }
        public int ataque { get; set; } = 10;
        public float vida { get; set; } = 100;
        public int defensa { get; set; } = 10;
        public int rango { get; set; } = 8;

        public float posX { get; set; }
        public float posY { get; set; }

        public string target { get; set; } = null;

        public virtual bool estaViva
        {
            get { return vida >= 0; }
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


        public int posXr
        {
            get { return (int)Math.Round(posX); }
        }

        public int posYr
        {
            get { return (int)Math.Round(posY); }
        }
    }
}
