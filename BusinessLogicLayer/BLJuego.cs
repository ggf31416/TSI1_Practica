using DataAccessLayer;
using EpPathFinding;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceStack.Redis;
using Microsoft.AspNet.SignalR.Client.Hubs;
using System.ServiceModel;
using Shared.Entities;

namespace BusinessLogicLayer
{
    public class BLJuego : IBLJuego
    {
        private IDALJuego _dal;

        public BLJuego(IDALJuego dal)
        {
            _dal = dal;
        }

        


        private List<TipoEdificio> cargarEdificios(Juego j, EstadoData.EstadoEnum estado)
        {
            Tablero miBase = j.Tablero;
            var ocupadas = miBase.Celdas.Where(c => c.IdTipoEdificio.HasValue && c.IdTipoEdificio >= 0 && c.Estado.Estado == estado);
            var res = new List<TipoEdificio>();
            foreach (TableroCelda tc in ocupadas)
            {
                TipoEdificio te = j.TipoEdificios.FirstOrDefault(t => t.Id == tc.IdTipoEdificio);
                res.Add(te);
            }
            return res;
        }

        

        private void actualizarRecursosPorSegundo(Juego j)
        {
            var recursos = j.DataJugador.EstadoRecursos;
            var lst = cargarEdificios(j,EstadoData.EstadoEnum.A);
            foreach (var cant in recursos.Values)
            {
                cant.Produccion = 0;
            }
            foreach (TipoEdificio e in lst)
            {
                foreach (var prod in e.RecursosAsociados)
                {
                    recursos[prod.IdRecurso].Produccion += prod.Valor;
                }
            }
        }

        private void actualizarRecursos(DataActual data)
        {
            TimeSpan dif = DateTime.Now - data.UltimaActualizacion;
            foreach (var cant in data.EstadoRecursos.Values)
            {
                cant.Total += (float)(cant.Produccion * dif.TotalSeconds);
            }

        }


        public void actualizarUnidades(DataActual data)
        {
            foreach (var unidad in data.EstadoUnidades.Values)
            {
                if (unidad.Estado == EstadoData.EstadoEnum.C && unidad.Fin <= DateTime.Now)

                    unidad.Estado = EstadoData.EstadoEnum.A;
            }
        }

        


        public bool actualizarEdificios(Juego juego)
        {
            bool cambio = true;
            var edificiosConstruyendo = juego.Tablero.Celdas.Where(c => c.IdTipoEdificio.HasValue && c.IdTipoEdificio >= 0 && c.Estado.Estado == EstadoData.EstadoEnum.A);
            var recursos = juego.DataJugador.EstadoRecursos;
            foreach (var edificio in edificiosConstruyendo)
            {
                if (edificio.Estado.Fin <= DateTime.Now)
                {
                    edificio.Estado.Estado = EstadoData.EstadoEnum.A;
                    cambio = true;
                    TimeSpan dif = DateTime.Now - edificio.Estado.Fin;
                    foreach (var prod in juego.TipoEdificios[edificio.Id].RecursosAsociados)
                    {
                        recursos[prod.IdRecurso].Total += (float)(prod.Valor * dif.TotalSeconds);
                    }
                }
            }
            return cambio;
        }




        public void ActualizarJuego(Juego j)
        {
            actualizarEdificios(j);
            actualizarRecursos(j.DataJugador);
            actualizarRecursosPorSegundo(j);
            IBLTecnologia tec = new BLTecnologia(this);
            tec.CompletarTecnologiasTerminadas(j);
            j.DataJugador.UltimaActualizacion = DateTime.Now;
        }

        public Juego GetAllDataJuego(string tenant)
        {
            return _dal.GetJuego(tenant);
        }

        public Juego GetJuegoUsuario(string tenant, string idUsuario)
        {
            var juego =  _dal.GetJuegoUsuario(tenant, idUsuario);
            ActualizarJuego(juego);
            return juego;
        }

        public void GuardarJuego(Juego j)
        {
            _dal.GuardarJuegoUsuarioAsync(j);
        }
    }
}