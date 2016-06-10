using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public interface IDALUsuario
    {
        bool login(Shared.Entities.ClienteJuego client);
        void register(Shared.Entities.ClienteJuego client);
        bool authenticate(Shared.Entities.ClienteJuego client);
        void logout(Shared.Entities.ClienteJuego client);

        
    }
}
