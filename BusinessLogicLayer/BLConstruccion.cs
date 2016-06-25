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
    public class BLConstruccion : IBLConstruccion
    {
        private IDALConstruccion _dal;

        public BLConstruccion(IDALConstruccion dal)
        {
            _dal = dal;
        }

        public bool ConstruirEdificio(CEInputData ceid, string Tenant, string NombreJugador)
        {
            BLJuego blJuego = new BLJuego(new DALJuego());
            blJuego.GetJuegoUsuario(Tenant, NombreJugador);

            ValidarConstruccion vE = _dal.ConstruirEdificio(ceid.IdTipoEdificio, Tenant, NombreJugador);

            //Checkear si la posicion esta vacia
            bool vacia = false;
            foreach (var celda in vE.Tablero.Celdas)
            {
                if(celda.PosFila == ceid.PosFila && celda.PosColumna == ceid.PosColumna)
                {
                    vacia = celda.IdTipoEdificio == null;
                    break;
                }
            }

            //Checkear si tiene recursos suficientes
            bool suficientesRecursos = true;
            foreach(var costo in vE.TipoEdificio.Costos)
            {
                if (vE.Recursos[costo.IdRecurso.ToString()] < costo.Valor)
                {
                    suficientesRecursos = false;
                    break;
                }
            }

            if(vacia && suficientesRecursos)
            {
                return _dal.PersistirEdificio(ceid, Tenant, NombreJugador);
            }
            else
            {
                return false;
            }
        }

        // retorna min(cantidad que podemos hacer con los recursos existentes, solicitada)
        private int validarUnidad(Juego j, int idUnidad,int cantidadSolicitada)
        {
            //Checkear si tiene recursos suficientes
            var recursos = j.DataJugador.EstadoRecursos;

            Dictionary<int, int> maxUnidadesPorRecurso = new Dictionary<int, int>();
            
            var tipo = j.TipoUnidades.FirstOrDefault(u => u.Id == idUnidad);
            if (tipo == null) return 0;
            foreach (var costo in tipo.Costos)
            {
                maxUnidadesPorRecurso[costo.IdRecurso] = ((int)recursos[costo.IdRecurso.ToString()].Total) / costo.Valor;
            }
            List<int> aux = maxUnidadesPorRecurso.Values.ToList();
            aux.Add(cantidadSolicitada);
            return  aux.Min();
        }

        private bool agregarUnidad_ModificarJuego(Juego j, EUInputData euid)
        {
            var estadoUnidades = j.DataJugador.EstadoUnidades;
            var estadoRecursos = j.DataJugador.EstadoRecursos;
            TipoUnidad tipo = j.TipoUnidades.FirstOrDefault(t => t.Id == euid.IdTipoUnidad);

            if (tipo == null) return false;

            EstadoData EstadoData;
            if (estadoUnidades.TryGetValue(euid.IdTipoUnidad.ToString(), out EstadoData))
            {
                long msDeberianFaltar = (long)EstadoData.Cantidad * tipo.TiempoConstruccion.Value * 100;
                if (EstadoData.Faltante > msDeberianFaltar || EstadoData.Faltante < 0) // sanidad
                {
                    EstadoData.Fin = DateTime.UtcNow.AddMilliseconds(msDeberianFaltar); ;
                }
                EstadoData.Cantidad += euid.Cantidad;
                EstadoData.Fin = EstadoData.Fin.AddMilliseconds(euid.Cantidad * tipo.TiempoConstruccion.Value * 100);
            }
            else
            {
                EstadoData = new Shared.Entities.EstadoData() { Id = euid.IdTipoUnidad, Cantidad = euid.Cantidad, Estado = EstadoData.EstadoEnum.C };
                EstadoData.Fin = DateTime.UtcNow.AddMilliseconds((int)tipo.TiempoConstruccion * 100);
            }
            estadoUnidades[tipo.Id.ToString()] = EstadoData;
            Shared.Entities.EstadoRecurso EstRec = new Shared.Entities.EstadoRecurso();
            foreach (var costo in tipo.Costos)
            {
                estadoRecursos.TryGetValue(costo.IdRecurso.ToString(), out EstRec);
                EstRec.Total = EstRec.Total - costo.Valor;

                estadoRecursos[costo.IdRecurso.ToString()] = EstRec;
            }
            return true;

        }

        public int EntrenarUnidad(EUInputData euid, string Tenant, string NombreJugador)
        {
            BLJuego blJuego = new BLJuego(new DALJuego());
            Juego juego = blJuego.GetJuegoUsuarioSinGuardar(Tenant, NombreJugador);
            if (juego == null)
            {
                return 0; // no tenemos datos del jugador, nos pasaron algo mal
            }


            int cantidad = validarUnidad(juego, euid.IdTipoUnidad, euid.Cantidad);
            if (cantidad > 0)
            {
                EUInputData newEUID = new EUInputData();
                newEUID.IdTipoUnidad = euid.IdTipoUnidad;
                newEUID.Cantidad = cantidad;
                agregarUnidad_ModificarJuego(juego, newEUID);
                if (blJuego.GuardarJuegoEsperar(juego))
                {
                    return cantidad;
                }
                else
                {
                    Console.WriteLine("Algo salio mal al guardar de vuelta el juego");
                    return 0;
                }
            }
            else
            {
                return cantidad;
            }
        }
    }
}
