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
            var ocupadas = miBase.Celdas.Where(c => c.IdTipoEdificio.HasValue && c.IdTipoEdificio >= 0 && (c.Estado != null && c.Estado.Estado == estado));
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
            foreach (var cant in recursos.Values)
            {
                if(cant.Total < 0)
                        cant.Total = 0;
            }
            var lst = cargarEdificios(j,EstadoData.EstadoEnum.A);
            foreach (var cant in recursos.Values)
            {
                cant.Produccion = 0;
            }
            foreach (TipoEdificio e in lst)
            {
                foreach (var prod in e.RecursosAsociados)
                {
                    recursos[prod.IdRecurso.ToString()].Produccion += prod.Valor;
                }
            }
        }

        private void actualizarRecursos(DataActual data)
        {
            TimeSpan dif = DateTime.UtcNow - data.UltimaActualizacion;
            foreach (var cant in data.EstadoRecursos.Values)
            {
                cant.Total += (float)(cant.Produccion * dif.TotalSeconds);
            }

        }


        public void actualizarUnidades(DataActual data)
        {
            var estUnidadesEnConstr = data.EstadoUnidades.Values.Where(x => x != null && x.Estado == EstadoData.EstadoEnum.C).ToList();
           
            foreach (var unidad in estUnidadesEnConstr)
            {
                if ( unidad.Fin <= DateTime.UtcNow) {

                    string key = unidad.Id.ToString()  + "#" + EstadoData.EstadoEnum.A;

                    if (!data.EstadoUnidades.ContainsKey(key))
                    {
                        data.EstadoUnidades.Add(key, new EstadoData() { Id = unidad.Id, Cantidad = 0, Estado = EstadoData.EstadoEnum.A, Fin = DateTime.UtcNow });
                    }
                    data.EstadoUnidades[key].Cantidad += unidad.Cantidad;
                    data.EstadoUnidades[key].Estado = EstadoData.EstadoEnum.A;
                    unidad.Cantidad = 0;
                } 
            }
        }

        
        public bool actualizarEdificios(Juego juego)
        {
            bool cambio = true;
            var edificiosConstruyendo = juego.Tablero.Celdas.Where(c => c.IdTipoEdificio.HasValue && c.IdTipoEdificio >= 0 && (c.Estado != null && c.Estado.Estado == EstadoData.EstadoEnum.C));
            var recursos = juego.DataJugador.EstadoRecursos;
            foreach (var edificio in edificiosConstruyendo)
            {
                if (edificio.Estado.Fin <= DateTime.UtcNow)
                {
                    edificio.Estado.Estado = EstadoData.EstadoEnum.A;
                    cambio = true;
                    TimeSpan dif = DateTime.UtcNow - edificio.Estado.Fin;
                    var tipoEdificio = juego.TipoEdificios.FirstOrDefault(e => e.Id== edificio.IdTipoEdificio);
                    foreach (var prod in tipoEdificio.RecursosAsociados)
                    {
                        recursos[prod.IdRecurso.ToString()].Total += (float)(prod.Valor * dif.TotalSeconds);
                    }
                }
            }
            return cambio;
        }


        public void ActualizarJuegoSinGuardar(Juego j)
        {
            if (j != null)
            {
                actualizarEdificios(j);
                actualizarUnidades(j.DataJugador);
                actualizarRecursos(j.DataJugador);
                actualizarRecursosPorSegundo(j);
                IBLTecnologia tec = new BLTecnologia(this);
                tec.CompletarTecnologiasTerminadasSinGuardar(j);
                j.DataJugador.UltimaActualizacion = DateTime.UtcNow;
            }   
        }

        public void ActualizarJuego(Juego j)
        {
            if (j != null)
            {
                try
                {
                    ActualizarJuegoSinGuardar(j);
                    GuardarJuego(j);
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex);
                }
                
            }
        }

        public Juego GetAllDataJuego(string tenant)
        {
            return _dal.GetJuegoUsuario(tenant, "ejemploUsuario");
        }

        public ListasEntidades GetEntidadesActualizadas(string tenant, string nombreJugador)
        {
            return _dal.GetEntidadesActualizadas(tenant, nombreJugador);
        }

        public Juego GetJuegoUsuario(string tenant, string idUsuario)
        {
            var juego =  _dal.GetJuegoUsuario(tenant, idUsuario);
            ActualizarJuego(juego);
            return juego;
        }

        public Juego GetJuegoUsuarioSinActualizar(string tenant, string idUsuario)
        {
            var juego = _dal.GetJuegoUsuario(tenant, idUsuario);
            ActualizarJuegoSinGuardar(juego);
            return juego;
        }

        public void GuardarJuego(Juego j)
        {
            _dal.GuardarJuegoUsuarioAsync(j);
        }


    }
}