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
            bool suficientesRecursos = false;
            foreach(var costo in vE.TipoEdificio.Costos)
            {
                //if(vE.recursos[costo.IdRecurso]. >= costo.Valor)
                //{

                //}
            }

            if (true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool EntrenarUnidad(EUInputData euid)
        {
            ValidarUnidad vE = _dal.EntrenarUnidad(euid.IdTipoUnidad);
            if (true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
