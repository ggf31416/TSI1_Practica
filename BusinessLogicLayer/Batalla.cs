﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer
{
    class Batalla
    {
        public string canalSignalR { get; set; }
        public BLTablero tablero;

        private void agregarUnidades(Jugador jug)
        {
            foreach (ConjuntoUnidades cu in jug.Unidades)
            {
                Unidad x = getUnidadPorId(cu.UnidadId);
                IEnumerable<Unidad> lst = Enumerable.Repeat(x, cu.Cantidad).ToList();
                Random r = new Random();
                
                foreach(Unidad u in lst)
                {
                    u.id = r.Next(1, Int32.MaxValue);
                }
                tablero.agregarUnidades(jug.Identificador, lst);
            }
        }

        void crearBatalla(Jugador atacante,Jugador defensor)
        {
            tablero = new BLTablero(null);
            agregarUnidades(atacante);
            agregarUnidades(defensor);
            tablero.agregarEdificios(defensor.Edificios);
        }

        private Unidad getUnidadPorId(int unidadId)
        {
            throw new NotImplementedException();
        }
    }
}