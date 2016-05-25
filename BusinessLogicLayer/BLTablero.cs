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


namespace BusinessLogicLayer
{
    public class BLTablero : IBLTablero
    {

        private IDALTablero _dal;

        public BLTablero(IDALTablero dal)
        {
            _dal = dal;
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

        // es jugarEdificio
        public void JugarUnidad(Shared.Entities.InfoCelda infoCelda)
        {  //_dal.JugarUnidad(infoCelda);

            BLServiceClient serviceClient = new BLServiceClient();
            ServiceInteraccionClient client = new ServiceInteraccionClient(serviceClient.binding, serviceClient.address);
            string jugador = "jugador1"; // TODO: DESHARCODEAR


            //client.Send("{\"Id\":" + infoCelda.Id + ",\"PosX\":" + infoCelda.PosX + ",\"PosY\":" + infoCelda.PosY + "}");
        }


        private void agregarUnidad(string jugador,int tipo_id,string unit_id,int posX,int posY)
        {

            BLServiceClient serviceClient = new BLServiceClient();
            ServiceInteraccionClient client = new ServiceInteraccionClient(serviceClient.binding, serviceClient.address);
            Batalla b = batallas[jugador];
             b.agregarUnidad(tipo_id, jugador);


            dynamic jsonObj = new { A = "AddUn", Id = tipo_id, PosX = posX, PosY = posY,Unit_id = unit_id };
            string s = JsonConvert.SerializeObject(jsonObj);
            client.Send(s);

            var path = b.tablero.ordenMoverUnidad(unit_id, 20, 15);
            dynamic jsonObj2 = new { A = "MoveUnit", Id = tipo_id, Unit_id = unit_id, PosX = posX, PosY = posY, Path = path.path };
            string s2 = JsonConvert.SerializeObject(jsonObj2);
            client.Send(s2);
        }

        public void Accion(string json)
        {
            dynamic obj = JsonConvert.DeserializeObject(json);
            string jugador = obj.J;
            string accion = obj.A;
            if (accion.Equals("AddUnidad"))
            {

            }
        }



        //public double CalcPartTimeEmployeeSalary(int idEmployee, int hours)
        //{
        //    //throw new NotImplementedException();
        //    Shared.Entities.Employee emp = GetEmployee(idEmployee);
        //    if(emp == null || (emp.GetType() == typeof(Shared.Entities.FullTimeEmployee)))
        //    {
        //        throw new Exception("Empleado no es Part time o no existe");
        //    }
        //    else
        //    {
        //        return hours * ((Shared.Entities.PartTimeEmployee)emp).HourlyRate;
        //    }
        //}
    }
}
