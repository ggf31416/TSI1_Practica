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
    [ServiceContract(Namespace = "http://localhost:8837/tsi1/")]
    public interface IServiceInteraccion
    {
        [OperationContract]
        void Send(String msg);
    }
}
