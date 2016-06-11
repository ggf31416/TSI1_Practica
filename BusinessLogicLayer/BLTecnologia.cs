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

        private bool checkarDependencias(Juego j,int IdTecnologia)
        {
            var estadoT = j.DataJugador.EstadoTecnologias;
            if (estadoT.ContainsKey(IdTecnologia))
            {
                return estadoT[IdTecnologia].Estado == EstadoData.EstadoEnum.A;
            }
            return false;
        }

        private bool consumirRecursosSiPuedo(Juego j ,Tecnologia t)
        {
            var t_costo = t.TecnologiaRecursoCostos;
            var estado_recursos = j.DataJugador.EstadoRecursos;
            bool puedoConstruir = t_costo.All(costoRec => estado_recursos[costoRec.IdRecurso].Total >= costoRec.Costo);
            if (puedoConstruir)
            {
                foreach (var costoRec in t_costo)
                {
                    estado_recursos[costoRec.IdRecurso].Total -= costoRec.Costo;
                }
            }
            return puedoConstruir;
        }

        public bool DesarrollarTecnologia(string tenant, string idJugador, int idTecnologia)
        {
            // pedir tenant
            Juego j = blHandler.GetAllDataJuego(tenant);

            
            var estadoT = j.DataJugador.EstadoTecnologias;
            if (!checkarDependencias(j, idTecnologia)) return false;
            if (!consumirRecursosSiPuedo(j, j.Tecnologias[idTecnologia])) return false;
            if (estadoT.ContainsKey(idTecnologia))
            {
                EstadoData estado = new EstadoData() { Estado = EstadoData.EstadoEnum.C, Tiempo = j.Tecnologias[idTecnologia].Tiempo };
                estadoT.Add(idTecnologia, estado);
            }
            // guardar juego
            return true;
        }

        public void AplicarTecnologia(string tenant, string idJugador, int idTecnologia)
        {
            // pedir tenant
            Juego j = blHandler.GetAllDataJuego(tenant);
            

        }

        public void AplicarTecnologia(Juego j ,Tecnologia tec)
        {
            var entidades = j.TipoEdificios.Cast<TipoEntidad>().ToDictionary(e => e.Id);
            foreach(var u in j.TipoUnidades)
            {
                entidades[u.Id] = u;
            }
            foreach (Accion a in tec.AccionesAsociadas)
            {
                AplicarAccion(j, a, entidades);
            }
            // juardar juego
        }

        private void AplicarAccion(Juego j ,Accion accion,Dictionary<int,TipoEntidad> entidades)
        {
            DataActual estado = j.DataJugador;
            if (accion.IdEntidad.HasValue)
            {
                int idEnt = accion.IdEntidad.Value;

                TipoEntidad te = entidades[idEnt];
                string atr = accion.NombreAtributo.ToLowerInvariant();
                if (atr.Equals("ataque"))
                {
                    te.Ataque += accion.Valor;
                }
                else if (atr.Equals("defensa"))
                {
                    te.Defensa += accion.Valor;
                }
                else if (atr.Equals("vida"))
                {
                    te.Vida += accion.Valor;
                }
            }
        }
    }





}
}
