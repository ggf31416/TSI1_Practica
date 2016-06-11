using Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer
{
    public interface IBLUsuario
    {
        //SOCIALES
        List<ClienteJuego> GetJugadoresAtacables(string Tenant, string IdJuego);

        //CLANES
        void CrearClan(string NombreClan, string Tenant, string IdJugador);
        bool AbandonarClan(string Tenant, string IdJugador);
        List<ClienteJuego> GetJugadoresSinClan(string Tenant, string IdJugador);
        bool AgregarJugadorClan(ClienteJuego Jugador, string Tenant, string IdJugador);
        List<ClienteJuego> GetJugadoresEnElClan(string Tenant, string IdJugador);
        bool SoyAdministrador(string Tenant, string IdJugador);
        int EnviarRecursos(List<RecursoAsociado> tributos, string IdJugadorDestino, string Tenant, string IdJugador);
    }
}
