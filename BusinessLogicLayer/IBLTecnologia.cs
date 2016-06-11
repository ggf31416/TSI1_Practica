using Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer
{
    public interface IBLTecnologia
    {
        bool DesarrollarTecnologia(string tenant, string idJugador,int idTecnologia);
        bool CompletarTecnologiasTerminadasSinGuardar(Juego j);
    }
}
