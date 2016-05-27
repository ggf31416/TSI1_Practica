using Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer
{
    public class Batalla
    {
        private Dictionary<int, TipoUnidad> tiposUnidades = new Dictionary<int, TipoUnidad>();
        private Dictionary<int, TipoEdificio> tiposEdificios = new Dictionary<int, TipoEdificio>();
        private DataAccessLayer.Relacional.IDALEntidadesRO _dalRO;
        public string canalSignalR { get; set; }
        public Tablero tablero;

        public void inicializar()
        {
            /*_dalRO = new DataAccessLayer.Relacional.DALEntidades();
            tiposUnidades = _dalRO.GetAllTipoUnidades().ToDictionary(x => x.Id);
            tiposEdificios = _dalRO.GetAllTipoEdificios().ToDictionary(x => x.Id);*/
        }

        public Batalla(string atacante,string defensor)
        {
            inicializar();
            this.tablero = new Tablero();

        }


        private void agregarUnidades(Jugador jug)
        {
            foreach (ConjuntoUnidades cu in jug.Unidades)
            {
                Unidad x = getUnidadPorId(cu.UnidadId);
                IEnumerable<Unidad> lst = Enumerable.Repeat(x, cu.Cantidad).ToList();
                Random r = new Random();
                
                foreach(Unidad u in lst)
                {
                    //u.id = r.Next(1, Int32.MaxValue);
                    u.jugador = jug.Identificador;
                }
                tablero.agregarUnidades(jug.Identificador, lst);
            }
        }

        public int agregarUnidad(int idUnidad,String jugador,string unitId,int posX,int posY)
        {
            Unidad u = getUnidadPorId( idUnidad);
            u.id = unitId;
            u.posX = posX;
            u.posY = posY;
             tablero.agregarUnidad(jugador,u);
            //return id;
            return 0;
        }

        

        void crearBatalla(Jugador atacante,Jugador defensor)
        {
            tablero = new Tablero();
            agregarUnidades(atacante);
            agregarUnidades(defensor);
            tablero.agregarEdificios(defensor.Edificios);
        }

        private Unidad getUnidadPorId(int tipoId)
        {
            //TipoUnidad tu = tiposUnidades[tipoId];

            //Unidad u = new Unidad { ataque = tu.Ataque.Value, defensa = tu.Defensa.Value, tipo_id = tu.Id, vida = tu.Vida.Value };
            Unidad u = new Unidad { ataque = 10, defensa = 10, tipo_id = tipoId, vida = 100 };
            u.rango = 8; // hardcodeado;
            return u;
        }

        private Edificio getEdificioPorId(int tipoId, int pX, int pY,string j)
        {
            //TipoEdificio te = tiposEdificios[tipoId];
            Edificio e = new Edificio { tipo_id = tipoId, posX = pX, posY = pY, jugador = j };
            return e;
        }

        public void ejecutarTurno()
        {
            tablero.tickTiempo();
        }
    }
}

