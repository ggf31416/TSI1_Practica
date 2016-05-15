using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceStack.Redis;
using Microsoft.AspNet.SignalR.Client.Hubs;
using System.ServiceModel;

namespace BusinessLogicLayer
{
    public class BLTablero : IBLTablero
    {
        private IDALTablero _dal;

        public BLTablero(IDALTablero dal)
        {
            _dal = dal;
        }

        public void JugarUnidad(Shared.Entities.InfoCelda infoCelda)
        {
            //_dal.JugarUnidad(infoCelda);

            BLServiceClient serviceClient = new BLServiceClient();
            ServiceInteraccionClient client = new ServiceInteraccionClient(serviceClient.binding, serviceClient.address);

            client.Send("{Id:" + infoCelda.Id + ",PosX:" + infoCelda.PosX + ",PosY:" + infoCelda.PosY + "}");
        }

    }
}
