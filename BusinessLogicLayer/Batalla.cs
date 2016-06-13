using Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace BusinessLogicLayer
{

    
    public class Batalla
    {
        private Dictionary<int, TipoUnidad> tiposUnidades = new Dictionary<int, TipoUnidad>();
        private Dictionary<int, TipoEdificio> tiposEdificios = new Dictionary<int, TipoEdificio>();
        public int JuegoId { get; set; }
        private List<Jugador> jugadores = new List<Jugador>();


        private Jugador defensor;
        private DataAccessLayer.Relacional.IDALEntidadesRO _dalRO;
        public string CanalSignalR { get; set; }
        public bool EnCurso { get; set; }
        public CampoBatalla tablero;


    

        public void inicializar()
        {
            /*_dalRO = new DataAccessLayer.Relacional.DALEntidades();
            tiposUnidades = _dalRO.GetAllTipoUnidades().ToDictionary(x => x.Id);
            tiposEdificios = _dalRO.GetAllTipoEdificios().ToDictionary(x => x.Id);*/
        }

        public Batalla(string atacante,string defensor)
        {
            inicializar();
            this.tablero = new CampoBatalla();
            this.EnCurso = true;

        }

        public Batalla(Jugador atacante,Jugador defensor)
        {
            inicializar();
            this.tablero = new CampoBatalla();
            this.tablero.JugadorDefensor = defensor.Id;
            this.EnCurso = true;
            this.defensor = defensor;
            jugadores.Add(atacante);
            jugadores.Add(defensor);
        }


        private void agregarUnidades(Jugador jug)
        {
            foreach (ConjuntoUnidades cu in jug.Unidades.Values)
            {
                Unidad x = getUnidadPorId(cu.UnidadId);
                IEnumerable<Unidad> lst = Enumerable.Repeat(x, cu.Cantidad).ToList();
                Random r = new Random();
                
                foreach(Unidad u in lst)
                {
                    //u.id = r.Next(1, Int32.MaxValue);
                    u.jugador = jug.Id;
                }
                tablero.agregarUnidades(jug.Id, lst);
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
            tablero = new CampoBatalla();
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
            if (tablero.Turno > 300 || tablero.PerdioUnJugador())
            {
                this.EnCurso = false;
            }
        }

        public string generarListaAccionesTurno()
        {
            List<AccionMsg> list = tablero.Acciones;
            if (list.Count == 0) return "";
            var obj = new
            {
                A = "ListaAcciones",
                L = list
            };
            string res = JsonConvert.SerializeObject(obj,new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
            return res;
        }

        public string GenerarJson()
        {
            // tipo de retorno anonimo
            var res = new
            {
                A = "IniciarAtaque",
                unidades = new List<Unidad>(),
                jugadores = new List<String>()
            };
            foreach(Jugador j in jugadores)
            {
                bool incluirEdificios = j.Equals(defensor);
                string jsonJugador = j.GenerarJson(incluirEdificios, false);
                res.jugadores.Add(jsonJugador);
            }
            return JsonConvert.SerializeObject(res);
        }


    }
}

