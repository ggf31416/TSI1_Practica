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

        public BLTecnologia(IBLJuego juegoHandler)
        {
            blHandler = juegoHandler;
        }

        private bool checkarDependencias(Juego j,int IdTecnologia)
        {
            var estadoT = j.DataJugador.EstadoTecnologias;
            if (estadoT.ContainsKey(IdTecnologia))
            {
                return estadoT[IdTecnologia].Estado == EstadoData.EstadoEnum.Puedo;
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
                EstadoData estado = new EstadoData() { Estado = EstadoData.EstadoEnum.C, Faltante =  j.Tecnologias[idTecnologia].Tiempo * 1000 };
                estadoT.Add(idTecnologia, estado);
            }
            // guardar juego
            return true;
        }

        public void CompletarTecnologia(string tenant, string idJugador, int idTecnologia)
        {
            // pedir tenant
            Juego j = blHandler.GetAllDataJuego(tenant);
            Tecnologia tec = j.Tecnologias.FirstOrDefault(t => t.Id == idTecnologia);
            if (tec != null) {
                AplicarTecnologia(j, tec);
                j.DataJugador.EstadoTecnologias[idTecnologia].Faltante = 0;
                j.DataJugador.EstadoTecnologias[idTecnologia].Estado = EstadoData.EstadoEnum.A;
            }
        }

        private bool estaCompleta(Juego j, int idTecnologia)
        {
            return j.DataJugador.EstadoTecnologias[idTecnologia].Faltante < 0;
        }

        private void AplicarTecnologia(Juego j ,Tecnologia tec)
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
            ActualizarTecnologiasConstruibles(j, tec);
            // guardar juego
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

        private bool puedoDesarrollarDep(Juego j, Tecnologia tec)
        {
            var estado = j.DataJugador.EstadoTecnologias;
            return tec.TecnologiaDependencias.All(dep => estado[dep.IdTecnologiaDepende].Estado == EstadoData.EstadoEnum.A);
        }

        private void ActualizarTecnologiasConstruibles(Juego j, Tecnologia tec)
        {
            var estado = j.DataJugador.EstadoTecnologias;
            foreach (var id in estado.Keys)
            {
                if (estado[id].Estado == EstadoData.EstadoEnum.NoPuedo)
                {
                    if (puedoDesarrollarDep(j, tec))
                    {
                        estado[id].Estado = EstadoData.EstadoEnum.Puedo;
                    }
                }
            }
        }
    }






}
