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

        //SOCIALES
        List<Shared.Entities.ClienteJuego> GetJugadoresAtacables(string Tenant, string IdJuego);

        //CLANES
        void CrearClan(string NombreClan, string Tenant, string IdJugador);
        bool AbandonarClan(string Tenant, string IdJugador);
        List<Shared.Entities.ClienteJuego> GetJugadoresSinClan(string Tenant, string IdJugador);
        bool AgregarJugadorClan(Shared.Entities.ClienteJuego Jugador, string Tenant, string IdJugador);
        List<Shared.Entities.ClienteJuego> GetJugadoresEnElClan(string Tenant, string IdJugador);
        bool SoyAdministrador(string Tenant, string IdJugador);

        int EnviarRecursos(List<Shared.Entities.RecursoAsociado> tributos, string IdJugadorDestino, string Tenant, string IdJugador);
    }
}
