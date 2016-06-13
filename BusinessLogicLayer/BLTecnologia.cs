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
            if (estadoT.ContainsKey(IdTecnologia.ToString()))
            {
                return estadoT[IdTecnologia.ToString()].Estado == EstadoData.EstadoEnum.Puedo;
            }
            return false;
        }

        private bool consumirRecursosSiPuedo(Juego j ,Tecnologia t)
        {
            var t_costo = t.TecnologiaRecursoCostos;
            var estado_recursos = j.DataJugador.EstadoRecursos;
            bool puedoConstruir = t_costo.All(costoRec => estado_recursos[costoRec.IdRecurso.ToString()].Total >= costoRec.Costo);
            if (puedoConstruir)
            {
                foreach (var costoRec in t_costo)
                {
                    estado_recursos[costoRec.IdRecurso.ToString()].Total -= costoRec.Costo;
                }
            }
            return puedoConstruir;
        }

        public bool DesarrollarTecnologia(string tenant, string idJugador, int idTecnologia)
        {
            // pedir tenant
            Juego j = blHandler.GetJuegoUsuarioSinActualizar(tenant, idJugador);

            
            var estadoT = j.DataJugador.EstadoTecnologias;
            if (!checkarDependencias(j, idTecnologia)) return false;
            if (!consumirRecursosSiPuedo(j, j.Tecnologias[idTecnologia])) return false;
            if (estadoT.ContainsKey(idTecnologia.ToString()))
            {
                EstadoData estado = new EstadoData() { Estado = EstadoData.EstadoEnum.C, Fin = DateTime.UtcNow. AddSeconds( j.Tecnologias[idTecnologia].Tiempo) };
                estadoT.Add(idTecnologia.ToString(), estado);
            }
            // guardar juego
            blHandler.GuardarJuego(j);
            return true;
        }

        /*public void CompletarTecnologia(string tenant, string idJugador, int idTecnologia)
        {
            // pedir tenant
            Juego j = blHandler.GetAllDataJuego(tenant);
            CompletarTecnologia(j, idTecnologia);
        }*/

        private void CompletarTecnologia(Juego j, string idTecnologia)
        {
            Tecnologia tec = j.Tecnologias.FirstOrDefault(t => t.Id.ToString() == idTecnologia);
            if (tec != null)
            {
                AplicarTecnologia(j, tec);

                j.DataJugador.EstadoTecnologias[idTecnologia.ToString()].Estado = EstadoData.EstadoEnum.A;
            }
        }

        private bool estaCompleta(Juego j, int idTecnologia)
        {
            return j.DataJugador.EstadoTecnologias[idTecnologia.ToString()].Fin > DateTime.UtcNow;
        }

        private void AplicarTecnologia(Juego j ,Tecnologia tec)
        {

            var entidades = j.TipoEdificios.Cast<TipoEntidad>().ToDictionary(e => e.Id);
            if (j.TipoUnidades != null)
            {
                foreach (var u in j.TipoUnidades)
                {
                    entidades[u.Id] = u;
                }

            }
            if (j.TipoEdificios != null)
            {
                foreach (var u in j.TipoEdificios)
                {
                    entidades[u.Id] = u;
                }
            }
            if (entidades.Count > 0)
            {
                foreach (Accion a in tec.AccionesAsociadas)
                {
                    AplicarAccion(j, a, entidades);
                }

            }

            // guardo juego afuera del metodo
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
            return tec.TecnologiaDependencias == null || tec.TecnologiaDependencias.All(dep => estado[dep.IdTecnologiaDepende.ToString()].Estado == EstadoData.EstadoEnum.A);
        }

        public void asegurarEstadoTecnologia(Juego j)
        {
            if (j.DataJugador.EstadoTecnologias == null)
            {
                j.DataJugador.EstadoTecnologias = new Dictionary<string, EstadoData>();
            }
            // si ya esta inicializado salgo
            if (j.DataJugador.EstadoTecnologias.Count > 0) return;
            foreach (Tecnologia t in j.Tecnologias)
            {
                string key = t.Id.ToString();
                if (true || !j.DataJugador.EstadoTecnologias.ContainsKey(key))
                {
                    EstadoData dataTec = new EstadoData() { Id = t.Id, Estado = EstadoData.EstadoEnum.NoPuedo, Fin = DateTime.UtcNow };
                    j.DataJugador.EstadoTecnologias.Add(key, dataTec);
                }
            }
        }

        

        public void ActualizarTecnologiasConstruibles(Juego j)
        {
            if (j.DataJugador.EstadoTecnologias == null)
                j.DataJugador.EstadoTecnologias = new Dictionary<string, EstadoData>();
            var estado = j.DataJugador.EstadoTecnologias;

            if (estado.Count == 0)
            {
                asegurarEstadoTecnologia(j);
            }
            var dicTecnologias = j.Tecnologias.ToDictionary(t => t.Id.ToString());
            foreach (var id in estado.Keys)
            {
                if (estado[id].Estado == EstadoData.EstadoEnum.NoPuedo)
                {

                    Tecnologia tec = dicTecnologias[id];
                    if (puedoDesarrollarDep(j, tec))
                    {
                        estado[id].Estado = EstadoData.EstadoEnum.Puedo;
                    }
                }
            }
        }

        public bool CompletarTecnologiasTerminadasSinGuardar(Juego j)
        {
            asegurarEstadoTecnologia(j); 
            var estado = j.DataJugador.EstadoTecnologias;
            
            bool algunaTermino = false;
            foreach (var kv in estado)
            {
                string idTec = kv.Key;
                EstadoData e = kv.Value;
                if (e.Estado == EstadoData.EstadoEnum.C && e.Fin < DateTime.UtcNow) 
                {
                    CompletarTecnologia(j, idTec);
                    algunaTermino = true;
                }
            }
            ActualizarTecnologiasConstruibles(j);
            return algunaTermino;
        }
    }






}
