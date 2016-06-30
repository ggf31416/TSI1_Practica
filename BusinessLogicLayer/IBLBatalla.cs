using Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer
{
    public interface IBLBatalla
    {
        void Accion(string tenant, string json);
        void IniciarAtaque(string tenant, InfoAtaque info);
        void IniciarBatalla(string tenant, InfoAtaque info);
        string getJsonBatalla(string tenant, string idUsuario);
        bool agregarContribucion(string tenant, string idDefensor, Contribucion contr);
    }
}
