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
            int MAX_RECURSOS = 1000000;
            TimeSpan dif = DateTime.UtcNow - data.UltimaActualizacion;
            double segundos = dif.TotalSeconds;
            if (dif.TotalSeconds < 0)
            {
                Console.WriteLine("dt recursos es negativo " + dif.TotalSeconds);
                segundos = 0;
            }
            foreach (var cant in data.EstadoRecursos.Values)
            {
                cant.Total += (float)(cant.Produccion * segundos);
                if (cant.Total > MAX_RECURSOS)
                {
                    cant.Total = MAX_RECURSOS;
                }
                else if (cant.Total < 0)
                {
                    cant.Total = 0; // TODO: ELIMINAR EN  PRODUCCION
                }

            }

        }

        // elimina anomalia por bug que tuvimos en un momento
        private void eliminarAnomalias(DataActual data)
        {
            var eliminar = data.EstadoUnidades.Where(kv => kv.Value.Id == 0).ToList();
            foreach (var dataErr in eliminar)
            {
                Console.WriteLine("Elimino unidades anomalas ");
                data.EstadoUnidades.Remove(dataErr.Key);
            }
        }

        public bool actualizarUnidades(DataActual data)
        {
            bool cambio = false;
            eliminarAnomalias(data);
            var estUnidadesEnConstr = data.EstadoUnidades.Values.Where(x => x != null && x.Estado == EstadoData.EstadoEnum.C).ToList();
            foreach (var unidad in estUnidadesEnConstr)
            {
                if ( unidad.Fin <= DateTime.UtcNow) {
                    cambio = true;
                    string keyCompletada = unidad.Id.ToString()  + "#" + EstadoData.EstadoEnum.A;
                    string keyConstruccion = unidad.Id.ToString();

                    if (!data.EstadoUnidades.ContainsKey(keyCompletada))
                    {
                        data.EstadoUnidades.Add(keyCompletada, new EstadoData() { Id = unidad.Id, Cantidad = 0, Estado = EstadoData.EstadoEnum.A, Fin = DateTime.UtcNow });
                    }
                    data.EstadoUnidades[keyCompletada].Cantidad += unidad.Cantidad;
                    data.EstadoUnidades[keyCompletada].Estado = EstadoData.EstadoEnum.A;
                    data.EstadoUnidades.Remove(keyConstruccion);
                }
            }
            return cambio;
        }

        
        public bool actualizarEdificios(Juego juego)
        {
            bool cambio = false;
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


        public bool ActualizarJuegoSinGuardar(Juego j)
        {
            if (j != null)
            {
                bool cambioEdificios = actualizarEdificios(j);
                bool cambioUnidades = actualizarUnidades(j.DataJugador);
                actualizarRecursos(j.DataJugador);
                actualizarRecursosPorSegundo(j);
                IBLTecnologia tec = new BLTecnologia(this);
                bool cambioTec = tec.CompletarTecnologiasTerminadasSinGuardar(j);
                j.DataJugador.UltimaActualizacion = DateTime.UtcNow;
                return cambioEdificios || cambioUnidades || cambioTec;
            }
            return false;
        }

        public bool ActualizarJuego(Juego j)
        {
            if (j != null)
            {
                try
                {
                    bool cambio = ActualizarJuegoSinGuardar(j);
                    if (cambio)
                    {
                        Console.WriteLine("cambio algo");
                        return GuardarJuegoEsperar(j);
                    }
                    else
                    {
                        Console.WriteLine("Solo recursos");
                       return _dal.ModificarRecursos(j);
                    }
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex);
                }   
            }
            return false;
        }

        private void AgregarUnidades(Juego j,Dictionary<int,int> unidades)
        {
            var dataUnidades = j.DataJugador.EstadoUnidades;
            foreach(int tipoId in unidades.Keys)
            {
                var key = tipoId + "#" + EstadoData.EstadoEnum.A;
                if (dataUnidades.ContainsKey(key))
                {
                    dataUnidades[key].Cantidad += unidades[tipoId];
                }
                else
                {
                    var estado = new EstadoData() { Id = tipoId, Estado = EstadoData.EstadoEnum.A, Cantidad = unidades[tipoId], Fin = DateTime.UtcNow };
                    dataUnidades.Add(key, estado);
                }
            }
        }

        private void ModificarUnidadesRecursos(Juego j, Dictionary<int,int> unidades,Dictionary<string ,double> agregarRecursos,double mult)
        {
            AgregarUnidades(j, unidades);
            foreach(var idRec in agregarRecursos.Keys)
            {
                j.DataJugador.EstadoRecursos[idRec ].Total += (float)Math.Round(agregarRecursos[idRec] * mult);
            }
            
        }

        public void ModificarUnidadesRecursos(string tenant, string idUsuario,Dictionary<int, int> unidades, Dictionary<string, double> agregarRecursos, double mult)
        {
            var dataJuego = this.GetJuegoUsuarioSinGuardar(tenant, idUsuario);
            ModificarUnidadesRecursos(dataJuego, unidades, agregarRecursos, mult);
            GuardarJuegoAsync(dataJuego);
        }


        public Dictionary<String,int> QuitarUnidades(Juego j, Contribucion contr,bool guardar)
        {
            var res = new Dictionary<String, int>();
            var dataUnidades = j.DataJugador.EstadoUnidades;
            foreach (var cu in contr.UnidadesContribuidas)
            {
                int tipoId = cu.UnidadId;
                var key = tipoId + "#" + EstadoData.EstadoEnum.A;
                if (dataUnidades.ContainsKey(key))
                {
                    int cant = Math.Min(dataUnidades[key].Cantidad,cu.Cantidad);
                    res.Add(tipoId.ToString(), cant);
                    dataUnidades[key].Cantidad -= cant;
                }
            }
            if (guardar)
            {
                _dal.ModificarUnidades(j);
            }
            return res;
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

        public Juego GetJuegoUsuarioSinGuardar(string tenant, string idUsuario)
        {
            var juego = _dal.GetJuegoUsuario(tenant, idUsuario);
            ActualizarJuegoSinGuardar(juego);
            return juego;
        }

        public void GuardarJuegoAsync(Juego j)
        {
            _dal.GuardarJuegoUsuarioAsync(j);
        }




        public bool GuardarJuegoEsperar(Juego j)
        {
            try
            {
                return _dal.GuardarJuegoUsuarioEsperar(j);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
            
        }

    }
}