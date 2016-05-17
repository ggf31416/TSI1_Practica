using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer
{
    public class Unidad
    {
        public int posX { get; set; }
        public int posY { get; set; }
        public int id { get; set; }
        public int tipo_id { get; set; }
        public string jugador { get; set; }
        public int ataque { get; set; } = 10;
        public float vida { get; set; } = 100;
        public int defensa { get; set; } = 10;
        public int rango { get; set; } = 8;

        public float distancia(Unidad otra)
        {
            return (posX - otra.posX) * (posX - otra.posX) + (posY - otra.posY) * (posY - otra.posY);
        }
    }
}
