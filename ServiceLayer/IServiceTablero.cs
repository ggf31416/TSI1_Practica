using Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer
{
    [ServiceContract(Namespace = "http://localhost:8836/tsi1/")]
    public interface IServiceTablero
    {
        [OperationContract]
        void JugarUnidad(InfoCelda infoCelda);

        [OperationContract]
        void Accion(String json);

        [OperationContract]
        void login(Cliente cliente);
    }
}
