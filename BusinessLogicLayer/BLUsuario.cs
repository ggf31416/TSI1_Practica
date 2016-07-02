using DataAccessLayer;
using EpPathFinding;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceStack.Redis;
using Microsoft.AspNet.SignalR.Client.Hubs;
using System.ServiceModel;
using Shared.Entities;

namespace BusinessLogicLayer
{
    public class BLUsuario : IBLUsuario
    {
        private IDALUsuario _dal;

        public BLUsuario(IDALUsuario dal)
        {
            _dal = dal;
        }

        //SOCIALES
        public List<ClienteJuego> GetJugadoresAtacables(string Tenant, string NombreJugador)
        {
            return _dal.GetJugadoresAtacables(Tenant, NombreJugador);
        }
        
        //CLANES
        public void CrearClan(string NombreClan, string Tenant, string IdJugador)
        {
            _dal.CrearClan(NombreClan, Tenant, IdJugador);
        }

        public bool AbandonarClan(string Tenant, string IdJugador)
        {
            return _dal.AbandonarClan(Tenant, IdJugador);
        }

        public List<ClienteJuego> GetJugadoresSinClan(string Tenant, string IdJugador)
        {
            return _dal.GetJugadoresSinClan(Tenant, IdJugador);
        }

        public bool AgregarJugadorClan(ClienteJuego Jugador, string Tenant, string IdJugador)
        {
            return _dal.AgregarJugadorClan(Jugador, Tenant, IdJugador);
        }

        public List<ClienteJuego> GetJugadoresEnElClan(string Tenant, string IdJugador)
        {
            return _dal.GetJugadoresEnElClan(Tenant, IdJugador);
        }

        public bool SoyAdministrador(string Tenant, string IdJugador)
        {
            return _dal.SoyAdministrador(Tenant, IdJugador);
        }

        public int EnviarRecursos(List<RecursoAsociado> tributos, string IdJugadorDestino, string Tenant, string IdJugador)
        {
            return _dal.EnviarRecursos(tributos, IdJugadorDestino, Tenant, IdJugador);
        }


    }
}
