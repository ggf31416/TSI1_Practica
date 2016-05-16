using DataAccessLayer;
using EpPathFinding;
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
        private List<Edificio> edificios;
        private List<Unidad> unidades;
        private static int edificio_size = 4;
        private static int tablero_size = 10;
        private int sizeX = tablero_size * edificio_size;
        private int sizeY = tablero_size * edificio_size;

        public BLTablero(IDALTablero dal)
        {
            _dal = dal;
        }

        public void JugarUnidad(Shared.Entities.InfoCelda infoCelda)
        {
            _dal.JugarUnidad(infoCelda);
        }

        BaseGrid crearTableroPF()
        {
            bool[][] matrix = new bool[sizeX][];
            for(int i = 0; i < sizeX; i++)
            {
                matrix[i] = new bool[sizeY];
                for(int j = 0; j < sizeY; j++){
                    matrix[i][j] = true;
                }
            }
            
            foreach(var e in edificios)
            {
                for(int i = 0; i < e.sizeX; i++)
                {
                    for (int j = 0; j < e.sizeY; j++)
                    {
                        matrix[e.posX + i][e.posY + j] = false;
                    }
                }
            }
            BaseGrid grilla = new StaticGrid(sizeX, sizeY,matrix);
            return grilla;
            
        }

        JumpPointParam parametrosBusqueda(BaseGrid grilla)
        {
            bool cruzarJuntoObstaculo = false;
            bool cruzarPorDiagonal = false;
            HeuristicMode heuristica_distancia = HeuristicMode.MIXTA15; // Diagonales valen 1.5
            JumpPointParam param = new JumpPointParam(grilla, true, cruzarJuntoObstaculo, cruzarPorDiagonal, heuristica_distancia);
            return param;
        }

        public List<GridPos> buscarPath(Unidad u, JumpPointParam param, GridPos dest)
        {
            param.StartNode.x = u.posX;
            param.StartNode.y = u.posY;
            param.EndNode.x = dest.x;
            param.EndNode.y = dest.y;
            List<GridPos> res = JumpPointFinder.FindPath(param);
            return res;
        }

        public class ResultadoBusqPath
        {
            public int id_unidad { get; set; }
            public GridPos[] path { get; set; }

            
        }

        public ResultadoBusqPath[] buscarTodos(Dictionary<Unidad,GridPos> destinos)
        {
            BaseGrid grilla = crearTableroPF();
            JumpPointParam param = parametrosBusqueda(grilla);
            var res = new List<ResultadoBusqPath>();
            foreach (var p in destinos)
            {
                
                var r_path = buscarPath(p.Key, param, p.Value);
                var r = new ResultadoBusqPath() { id_unidad = p.Key.id, path = r_path.ToArray() };
                res.Add(r);
            }
            return res.ToArray();
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
