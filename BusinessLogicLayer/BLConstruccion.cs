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

        public bool ConstruirEdificio(CEInputData ceid)
        {
            ValidarConstruccion vE = _dal.ConstruirEdificio(ceid.IdTipoEdificio);

            //Checkear si la posicion esta vacia
            bool vacia = false;
            foreach (var celda in vE.Tablero.Celdas)
            {
                if(celda.PosFila == ceid.PosFila && celda.PosColumna == ceid.PosColumna)
                {
                    vacia = celda.IdTipoEdificio == -1;
                    break;
                }
            }

            //Checkear si tiene recursos suficientes
            bool suficientesRecursos = false;
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
                return _dal.PersistirEdificio(ceid);
            }
            else
            {
                return false;
            }
        }

        public int EntrenarUnidad(EUInputData euid)
        {
            //TODO: ACTUALIZAR BASE

            ValidarUnidad vU = _dal.EntrenarUnidad(euid.IdTipoUnidad);

            //Checkear si tiene recursos suficientes
            int cantidad = 0;
            Dictionary<int, int> maxUnidadesPorRecurso = new Dictionary<int, int>();
            foreach (var costo in vU.TipoUnidad.Costos)
            {
                maxUnidadesPorRecurso[costo.IdRecurso] = vU.Recursos[costo.IdRecurso.ToString()] / costo.Valor;
            }
            List<int> aux = maxUnidadesPorRecurso.Values.ToList();
            cantidad = aux.Min();
            if (cantidad > 0)
            {
                EUInputData newEUID = new EUInputData();
                newEUID.IdTipoUnidad = euid.IdTipoUnidad;
                newEUID.Cantidad = cantidad;
                if (_dal.PersistirUnidades(newEUID))
                {
                    return cantidad;
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return cantidad;
            }

            //TODO: ACTUALIZAR BASE
        }
    }
}
