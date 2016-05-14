using BusinessLogicLayer;
using Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;

namespace ServiceLayer
{
    [ServiceBehavior(IncludeExceptionDetailInFaults = true)]
    public class ServiceTablero : IServiceTablero
    {
        private static IBLTablero blHandler;

        public ServiceTablero()
        {
            blHandler = Program.blHandler;
        }

        public void JugarUnidad(InfoCelda infoCelda) {
            blHandler.JugarUnidad(infoCelda);
        }
    }
}
