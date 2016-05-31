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
    public class BLTablero : IBLTablero
    {

        private IDALTablero _dal;

        public BLTablero(IDALTablero dal)
        {
            _dal = dal;
        }


        public void ejecutarBatallasEnCurso()
        {
            foreach(Batalla b in batallas.Values)
            {
                b.ejecutarTurno();
            }
        }

        public Dictionary<string, Batalla> batallas = new Dictionary<string, Batalla>();

        private void crearBatalla(string jugador)
        {
            if (!batallas.ContainsKey(jugador))
            {
                Batalla tmp = new Batalla("", jugador);
                batallas.Add(jugador, tmp);
                tmp.inicializar();
            }
        }

        public void agregarEdificio(AccionMsg msg)
        {
            BLServiceClient serviceClient = new BLServiceClient();
            ServiceInteraccionClient client = new ServiceInteraccionClient(serviceClient.binding, serviceClient.address);
            Batalla b = obtenerBatalla(msg.Jugador);
            b.tablero.agregarEdificio(new Edificio { tipo_id = msg.Id, jugador = msg.Jugador, posX = msg.PosX, posY = msg.PosY });
            AccionMsg msgSend = new AccionMsg { Accion = "AddEd", Id = msg.Id, PosX = msg.PosX, PosY =msg.PosY };
            client.Send(JsonConvert.SerializeObject(msgSend));

        }

        // es jugarEdificio
        public void JugarUnidad(Shared.Entities.InfoCelda infoCelda)
        {  //_dal.JugarUnidad(infoCelda);

            //client.Send("{\"Id\":" + infoCelda.Id + ",\"PosX\":" + infoCelda.PosX + ",\"PosY\":" + infoCelda.PosY + "}");
        }

      

        private void agregarUnidad(string jugador, int tipo_id, string unit_id, int posX, int posY)
        {

            BLServiceClient serviceClient = new BLServiceClient();
            ServiceInteraccionClient client = new ServiceInteraccionClient(serviceClient.binding, serviceClient.address);
            Batalla b = obtenerBatalla(jugador);
            b.agregarUnidad(tipo_id, jugador, unit_id, posX, posY);


            dynamic jsonObj = new { A = "AddUn", Id = tipo_id, PosX = posX, PosY = posY, Unit_id = unit_id };
            string s = JsonConvert.SerializeObject(jsonObj);
            client.Send(s);

            var path = b.tablero.ordenMoverUnidad(unit_id, 20, 15);
            dynamic jsonObj2 = new { A = "MoveUnit", Id = tipo_id, Unit_id = unit_id, PosX = posX, PosY = posY, Path = path.path };
            string s2 = JsonConvert.SerializeObject(jsonObj2);
            client.Send(s2);
        }

        private Batalla obtenerBatalla(string jugador)
        {
            if (!batallas.ContainsKey(jugador))
            {
                batallas.Add(jugador, new Batalla(jugador, ""));
            }
            Batalla b = batallas[jugador];
            return b;
        }

        public void Accion(string json)
        {
            var obj = JsonConvert.DeserializeObject<AccionMsg>(json);
            string accion = obj.Accion;

            if (accion.Equals("AddUnidad"))
            {
                //string nombreTipo = "Tipo: " + (obj.Id.GetType().FullName);

                int tipoId = (int)obj.Id; ;
                int posX = (int)Math.Round((double)obj.PosX);
                int posY = (int)Math.Round((double)obj.PosY);
                agregarUnidad(obj.Jugador, tipoId, obj.IdUnidad, posX, posY);
            }
            else if (accion.Equals("AddEd"))
            {
                agregarEdificio(obj);
            }
        }

        public void login(Cliente cliente, int idJuego)
        {
            IDALUsuario iDALUsuario = new DALUsuario(idJuego);
            iDALUsuario.login(cliente);
        }

        public bool authenticate(Cliente cliente, int idJuego)
        {
            IDALUsuario iDALUsuario = new DALUsuario(idJuego);
            return iDALUsuario.authenticate(cliente);
        }
    }
}
