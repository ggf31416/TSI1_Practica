using Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer
{
    public class BLTecnologia : IBLTecnologia
    {
        private IBLJuego blHandler;

        private void ActualizarTecnologias(string tenant, string idJugador)
        {
            throw new NotImplementedException();
        }

        private bool CheckarDependencias(Juego j,int IdTecnologia)
        {
            var estadoT = j.DataJugador.EstadoTecnologias;
            if (estadoT.ContainsKey(IdTecnologia))
            {
                return estadoT[IdTecnologia].Estado == EstadoData.EstadoEnum.A;
            }
            return false;
        }

        public bool DesarrollarTecnologia(string tenant, string idJugador, int idTecnologia)
        {
            // pedir tenant
            Juego j = blHandler.GetAllDataJuego(idJugador);

            
            var estadoT = j.DataJugador.EstadoTecnologias;
            if (!CheckarDependencias(j, idTecnologia)) return false;
            if (estadoT.ContainsKey(idTecnologia))
            {
                EstadoData estado = new EstadoData() { Estado = EstadoData.EstadoEnum.C, Tiempo = j.Tecnologias[idTecnologia].Tiempo };
                estadoT.Add(idTecnologia, estado);
            }
            return true;


        }
    }
}
