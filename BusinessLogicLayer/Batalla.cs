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
        private List<TipoUnidad> tiposUnidades = new List<TipoUnidad>();
        private List< TipoEdificio> tiposEdificios = new List< TipoEdificio>();
        public string Tenant { get; set; }
        private Dictionary<string, Jugador> jugadores = new Dictionary<string, Jugador>();


        private Jugador defensor;
        private DataAccessLayer.Relacional.IDALEntidadesRO _dalRO;
        public string GrupoSignalR { get; set; }
        public bool EnCurso { get; set; }
        public CampoBatalla tablero;




    

        public Dictionary<int,int> UnidadesSobrevivientes(String jugId)
        {
            if (jugadores.ContainsKey(jugId))
            {
                return tablero.UnidadesSobrevivientes(jugadores[jugId]);
            }
            return new Dictionary<int, int>();
        }

    

        public Batalla(Tablero t,Jugador atacante,Jugador defensor)
        {
            int sizeTableroX = t.CantColumnas.GetValueOrDefault();
            int sizeTableroY = t.CantFilas.GetValueOrDefault();
            int offSet = 4;
            this.tablero = new CampoBatalla(sizeTableroX + 2 * offSet, sizeTableroY + 2 * offSet, offSet, offSet);
            this.tablero.JugadorDefensor = defensor.Id;
            this.EnCurso = true;
            this.defensor = defensor;
            jugadores.Add(atacante.Id,atacante);
            jugadores.Add(defensor.Id, defensor);
            this.tiposEdificios = atacante.tiposEdificio;
            this.tiposUnidades = atacante.tiposUnidad;
            tablero.agregarEdificios(defensor.Edificios);

            this.GrupoSignalR = "bat_" + this.defensor.Id;
            //inicializar();
        }


        private void agregarUnidades(Jugador jug)
        {
            foreach (ConjuntoUnidades cu in jug.Unidades.Values)
            {
                Unidad x = getUnidadPorId(cu.UnidadId,jug.Id);
                IEnumerable<Unidad> lst = Enumerable.Repeat(x, cu.Cantidad).ToList();
                Random r = new Random();
                
                foreach(Unidad u in lst)
                {
                    u.jugador = jug.Id;
                }
                tablero.agregarUnidades(jug.Id, lst);
            }
        }

        public int agregarUnidad(int id_tipo,String jugador,string unitId,int posX,int posY)
        {
            if (!this.jugadores.ContainsKey(jugador) || !(this.jugadores[jugador].Unidades.ContainsKey(id_tipo))) return 0;
            if (this.jugadores[jugador].Unidades[id_tipo].Cantidad > 0)
            {
                Unidad u = getUnidadPorId(id_tipo, jugador);
                this.jugadores[jugador].Unidades[id_tipo].Cantidad -= 1;
                u.id = unitId;
                u.posX = posX;
                u.posY = posY;
                tablero.agregarUnidad(jugador, u);
                return 1;
            }
            return 0;
        }

        private Unidad getUnidadPorId(int tipoId,string idJugador)
        {
            Unidad u = null;
            if (!jugadores.ContainsKey(idJugador)) return u;
            TipoUnidad tu = jugadores[idJugador].tipos.GetValueOrDefault(tipoId) as TipoUnidad;
            if (tu != null)
            {
                u = new Unidad { ataque = tu.Ataque.GetValueOrDefault(), defensa = tu.Defensa.GetValueOrDefault(), tipo_id = tu.Id, hp = tu.Vida.GetValueOrDefault() };
                u.rango = 8; // hardcodeado;
            }
            return u;
        }

        private Edificio getEdificioPorId(int tipoId, int pX, int pY, string j)
        {
            if (!jugadores.ContainsKey(j)) return null;
            TipoEdificio te = jugadores[j].tipos.GetValueOrDefault(tipoId) as TipoEdificio;
            Edificio e = null;
            if (te != null)
            {
                e = new Edificio { tipo_id = tipoId, posX = pX, posY = pY, jugador = j};
                e.DesdeTipo(te);
            }
            return e;
        }

        

        private bool  perdioUnClan()
        {
            Dictionary<string, bool> tieneUnidades = new Dictionary<string, bool>();
            foreach(Jugador j in this.jugadores.Values)
            {
                if (j.Unidades.Count > 0 && j.Unidades.Any(cu => cu.Value.Cantidad > 0)) tieneUnidades[j.Clan] = true;
                if (tablero.QuedanUnidadesJugador(j.Id)) tieneUnidades[j.Clan] = true;
            }
            return tieneUnidades.Values.Any(b => b == false);
        }

        public void ejecutarTurno()
        {
            tablero.tickTiempo();
            if (tablero.Turno > 300 || perdioUnClan())
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

        public class InfoBatalla
        {
            public string A { get; set; } = "IniciarAtaque";
            public bool Finalice { get; set; } = false;
            public List<Unidad>  unidades { get; set; }  = new List<Unidad>();
            public List<Edificio> edificios { get; set; } = new List<Edificio>();
            public List<TipoUnidad> tiposUnidad { get; set; } = new List<TipoUnidad>();
            public List<TipoEdificio> tiposEdificio { get; set; } = new List<TipoEdificio>();
            public Dictionary<string,InfoJugador> jugadores { get; set; } = new Dictionary<string,InfoJugador>();

            public string IdJugador { get; set; }

        }


        public String[] GetListaJugadores()
        {
            return jugadores.Keys.ToArray();
        }

        public string GenerarJson(string IdJugador)
        {

            var res = new InfoBatalla();
            res.IdJugador = IdJugador;
            foreach(Jugador j in jugadores.Values)
            {
                bool incluirEdificios = j.Id.Equals(defensor.Id);
                InfoJugador infoJ = j.GenerarInfo(incluirEdificios, false);
                res.jugadores.Add(infoJ.Id,infoJ);

            }

        
            this.tablero.RellenarInfoBatalla(res);
            res.tiposEdificio = this.tiposEdificios;
            res.tiposUnidad = this.tiposUnidades;
            return JsonConvert.SerializeObject(res);
        }

        public void DeployUnidadesAutomatico(string idUsuario)
        {
            if (this.jugadores.ContainsKey(idUsuario)){
                Jugador jugador = jugadores[idUsuario];
                if (idUsuario.Equals(this.defensor.Id))
                {
                    
                }
                //tablero.DeployUnidadesAutomatico()
            }
        }

    }
}

