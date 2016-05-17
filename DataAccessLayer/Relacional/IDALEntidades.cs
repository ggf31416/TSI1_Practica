using Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Relacional
{
    public interface IDALEntidadesRO
    {
        List<TipoEdificio> GetAllTipoEdificios();
        List<TipoUnidad> GetAllTipoUnidades();
    }
}
