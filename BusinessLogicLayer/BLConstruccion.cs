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
            _dal.ConstruirEdificio(ceid.IdTipoEdificio);
            if ()
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
