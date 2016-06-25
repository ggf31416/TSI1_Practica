using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace WCFServiceWebRole1
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IService1
    {
        [OperationContract]
        void Send(String msg);

        [OperationContract]
        void SendLista(List<string> nombreUsuarios, String msg);

        [OperationContract]
        void SendGrupo(String grupo, String msg);

        [OperationContract]
        void SendUsuario(String usuario, String msg);

    }

}
