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

        public string GrupoSignalR { get; set; }
        public bool EnCurso { get; set; }
        public CampoBatalla tablero;

        public ConfigBatalla Config { get; set; }


        public Dictionary<int,int> UnidadesSobrevivientes(String jugId)
        {
            if (jugadores.ContainsKey(jugId))
            {
                return tablero.UnidadesSobrevivientes(jugadores[jugId]);
            }
            return new Dictionary<int, int>();
        }

        public String IdDefensor()
        {
            return this.defensor.Id;
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
            tablero.Clanes[atacante.Id] = atacante.Clan;
            tablero.Clanes[defensor.Id] = defensor.Clan;
            this.GrupoSignalR = "bat_" + this.defensor.Id;
        }


        private List<Unidad> obtenerObjetosUnidad(Jugador jug)
        {
            List<Unidad> res = new List<Unidad>();
            foreach (ConjuntoUnidades cu in jug.Unidades.Values)
            {
                for(int i = 0; i < cu.Cantidad; i++)
                {
                    Unidad x = getUnidadPorId(cu.UnidadId, jug.Id);
                }
            }
            return res;
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
        

        private bool perdioUnClan()
        {
            return quedanUnidadesClan().Values.Any(b => b == false);
        }

        private Dictionary<string,bool>  quedanUnidadesClan()
        {
            Dictionary<string, bool> tieneUnidades = new Dictionary<string, bool>();
            foreach(Jugador j in this.jugadores.Values)
            {
                if (j.Unidades.Count > 0 && j.Unidades.Any(cu => cu.Value.Cantidad > 0)) tieneUnidades[j.Clan] = true;
                if (tablero.QuedanUnidadesJugador(j.Id)) tieneUnidades[j.Clan] = true;
            }
            return tieneUnidades;
        }

        public String obtenerClanGanador()
        {
            var tienen = quedanUnidadesClan();
            var conUnidades = tienen.Where(keyValor => keyValor.Value == true) ;

            if (conUnidades.Count() == 1)
            {
                return conUnidades.First().Key;
                // solo una alianza obtuvo la victoria
            }
            else
            {
                // ninguno llego a perder o los 2 se destruyeron mutuamente
                return null;
            }
        }

        public List<String> ObtenerGanadores()
        {
            
            var res = new List<String>();
            var clanGan = obtenerClanGanador();
            if (clanGan == null) return res;
            foreach (Jugador j in jugadores.Values)
            {
                if (j.Clan.Equals(clanGan)) res.Add(j.Id);
            }
            return res;
        }

        public bool ganoAtacante()
        {
            return false == ObtenerGanadores().Contains(this.defensor.Id);
        }



        public void ejecutarTurno()
        {
            tablero.tickTiempo();
            if (tablero.Turno > this.Config.SegundosAtaque || perdioUnClan())
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
                var unidadesDesplegables = obtenerObjetosUnidad(jugador);
                int centroTableroX = tablero.sizeX / 2;
                int centroTableroY = tablero.sizeY / 2;
                if (idUsuario.Equals(this.defensor.Id))
                {
                    tablero.DeployUnidadesAutomatico(centroTableroX, centroTableroY, unidadesDesplegables);
                }
                else
                {
                    Random r = new Random();
                    int posX = centroTableroX;
                    int posY = centroTableroY;
                    // eligo al azar excepto en el centro del tablero
                    while (Math.Abs(posX - centroTableroX) < tablero.sizeX / 4)
                    {
                        posX = r.Next(0, tablero.sizeX);
                    }
                    while (Math.Abs(posY - centroTableroY) < tablero.sizeX / 4)
                    {
                        posY = r.Next(0, tablero.sizeY);
                    }
                    tablero.DeployUnidadesAutomatico(posX, posY, unidadesDesplegables);
                }
            }
        }





    }
}

