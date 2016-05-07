using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer
{
    public class BLTablero : IBLTablero
    {
        private IDALTablero _dal;

        public BLTablero(IDALTablero dal)
        {
            _dal = dal;
        }

        public void JugarUnidad(Shared.Entities.InfoCelda infoCelda)
        {
            _dal.JugarUnidad(infoCelda);
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
