using Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Shared.Entities.DataBatalla;

namespace BusinessLogicLayer
{

    
    public class Batalla
    {
        private List<TipoUnidad> tiposUnidades = new List<TipoUnidad>();
        private List< TipoEdificio> tiposEdificios = new List< TipoEdificio>();
        public string Tenant { get; set; }
        private Dictionary<string, Jugador> jugadores = new Dictionary<string, Jugador>();
        public string BatallaId { get; set; }
        public int idxJugador = 1;

        public HashSet<string > movio = new HashSet<string >();

        private Jugador defensor;

        public string GrupoSignalR { get; set; }
        public bool EnCurso { get; set; }
        public bool EnFinalizacion { get; set; }
        public CampoBatalla campo;
        public Tablero tableroOriginal { get; set; }

        public ConfigBatalla Config { get; set; } = new ConfigBatalla();


        public Dictionary<int,int> UnidadesSobrevivientes(String jugId)
        {
            if (jugadores.ContainsKey(jugId))
            {
                return campo.UnidadesSobrevivientes(jugadores[jugId]);
            }
            return new Dictionary<int, int>();
        }

        public Dictionary<int,int> UnidadesPerdidas(String jugId)
        {
            if (jugadores.ContainsKey(jugId))
            {
                return campo.UnidadesPerdidas(jugadores[jugId]);
            }
            return new Dictionary<int, int>();
        }

        public String IdDefensor()
        {
            return this.defensor.Id;
        }

        private void offsetRecomendado(int sizeX,int sizeY,out int offsetX, out int offsetY,out int newSizeX,out int newSizeY)
        {
            int maxSize = Math.Max(sizeX,sizeY);
            int offset = 0;
            if (maxSize < 7)
            {
                offset = 2;
            }
            else
            {
                offset = 1;
            }
            int half = Math.Abs(sizeX - sizeY) / 2;
            if (sizeX > sizeY)
            {
                offsetX = offset;
                offsetY = offset + half;
            }
            else
            {
                offsetX = offset + half;
                offsetY = offset;
            }
            
            offsetY = offset;
            newSizeX = maxSize + 2 * offset;
            newSizeY = maxSize + 2 * offset;
        }

        public Batalla(Tablero t, Jugador defensor,ConfigBatalla config,String tenant)
        {
            this.Tenant = tenant;
            this.tableroOriginal = t;
            int sizeTableroX = t.CantColumnas.GetValueOrDefault();
            int sizeTableroY = t.CantFilas.GetValueOrDefault();
            int offSetX, offSetY, sizeCampoX, sizeCampoY;
            offsetRecomendado(sizeTableroX, sizeTableroY, out offSetX, out offSetY,out sizeCampoX,out sizeCampoY);
            this.campo = new CampoBatalla(sizeCampoX, sizeCampoY, offSetX, offSetY);
            this.campo.JugadorDefensor = defensor.Id;
            this.EnCurso = true;
            this.defensor = defensor;
           
            jugadores.Add(defensor.Id, defensor);

            this.tiposEdificios = defensor.tiposEdificio;
            this.tiposUnidades = defensor.tiposUnidad;
            campo.agregarEdificios(defensor.Edificios);

            campo.Clanes[defensor.Id] = defensor.Clan;
            this.GrupoSignalR = "bat_" + this.defensor.Id;
            this.Config = config;
        }

        public void AgregarJugador(Jugador j)
        {
            jugadores[ j.Id]=  j;
            j.ShortId = this.idxJugador++.ToString();
            campo.Clanes[j.Id] = j.Clan;
        }


        private List<Unidad> obtenerObjetosUnidad(Jugador jug,int max)
        {
            List<Unidad> res = new List<Unidad>();
            foreach (ConjuntoUnidades cu in jug.Unidades.Values)
            {
                int cant = cu.Cantidad;
                for(int i = 0; i < cant; i++)
                {
                    if (max > 0)
                    {
                        Unidad x = getUnidadPorId(cu.UnidadId, jug.Id);
                        x.id = jug.ShortId + "*" + max;
                        res.Add(x);
                        cu.Cantidad--;
                        max--;
                    }

                }
            }
            return res;
        }

        public int agregarUnidad(int id_tipo,String jugador,string unitId,int posX,int posY)
        {
            if (!this.jugadores.ContainsKey(jugador) || !(this.jugadores[jugador].Unidades.ContainsKey(id_tipo))) return 0;
            if (this.jugadores[jugador].Unidades[id_tipo].Cantidad > 0)
            {
                this.movio.Add(jugador); // desactivo el deploy automatico
                Unidad u = getUnidadPorId(id_tipo, jugador);
                this.jugadores[jugador].Unidades[id_tipo].Cantidad -= 1;
                u.id = unitId;
                u.posX = posX;
                u.posY = posY;
                campo.agregarUnidad(jugador, u);
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
                u = new Unidad { ataque = tu.Ataque.GetValueOrDefault(),
                    defensa = tu.Defensa.GetValueOrDefault(),
                    tipo_id = tu.Id,
                    jugador = idJugador,
                    hp = tu.Vida.GetValueOrDefault()

                };
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
                tieneUnidades[j.Clan] = false;
                if (j.Unidades.Count > 0 && j.Unidades.Any(cu => cu.Value.Cantidad > 0)) tieneUnidades[j.Clan] = true;
                if (campo.QuedanUnidadesJugador(j.Id)) tieneUnidades[j.Clan] = true;
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
            var ganadores = ObtenerGanadores();
            return ganadores.Count > 0 &&  false == ganadores.Contains(this.defensor.Id);
        }



        public void ejecutarTurno()
        {
            campo.tickTiempo();
            double tiempoTotal = campo.Turno * Config.MilisTurno / 1000.0;
            double tiempoCombate = tiempoTotal - Config.TiempoDeploy;
            if (!this.campo.EnCombate && tiempoTotal > Config.TiempoDeploy )
            {
                foreach(string jId in this.jugadores.Keys)
                {
                    if (!movio.Contains(jId))
                    {
                        movio.Add(jId);
                        DeployUnidadesAutomatico(jId);

                    }
                }
                this.campo.EnCombate = true;   
            }
            if (tiempoCombate > this.Config.TiempoBatalla   || perdioUnClan())
            {
                this.EnCurso = false;
            }
        }

        public string generarListaAccionesTurno()
        {
            List<AccionMsg> list = campo.Acciones;
            if (list.Count == 0) return "";
            var obj = new
            {
                A = "ListaAcciones",
                L = list
            };
            string res = JsonConvert.SerializeObject(obj,new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
            return res;
        }




        public String[] GetListaJugadores()
        {
            return jugadores.Keys.ToArray();
        }

        public string GenerarJson(string IdJugador)
        {

            InfoBatalla res = GenerarInfoBatalla(IdJugador);
            return JsonConvert.SerializeObject(res);
        }

        public InfoBatalla GenerarInfoBatalla(string IdJugador)
        {
            var res = new InfoBatalla();
            res.IdJugador = IdJugador;
            res.ShortId = jugadores[IdJugador].ShortId;
            foreach (Jugador j in jugadores.Values)
            {
                bool incluirEdificios = j.Id.Equals(defensor.Id);
                InfoJugador infoJ = j.GenerarInfo(incluirEdificios, false);
                res.jugadores.Add(infoJ.Id, infoJ);

            }

            this.campo.RellenarInfoBatalla(res);
            res.tiposEdificio = this.tiposEdificios;
            res.tiposUnidad = this.tiposUnidades;
            res.SizeX = this.campo.sizeX;
            res.SizeY = this.campo.sizeY;
            var dataExtra = new
            {
                Pasto = tableroOriginal.ImagenTerreno,
                Fondo = tableroOriginal.ImagenFondo,
            };
            res.Data = JsonConvert.SerializeObject(dataExtra);
            return res;
        }

        public void DeployUnidadesAutomatico(string idUsuario)
        {
            if (this.jugadores.ContainsKey(idUsuario)){
                Jugador jugador = jugadores[idUsuario];
                var unidadesDesplegables = obtenerObjetosUnidad(jugador,20);
                int centroTableroX = campo.sizeX / 2;
                int centroTableroY = campo.sizeY / 2;
                if (idUsuario.Equals(this.defensor.Id))
                {
                    campo.DeployUnidadesAutomatico(centroTableroX, centroTableroY, unidadesDesplegables);
                }
                else
                {
                    Random r = new Random();
                    int posX = centroTableroX;
                    int posY = centroTableroY;
                    // eligo al azar excepto en el centro del tablero
                    while (Math.Abs(posX - centroTableroX) < campo.sizeX / 4)
                    {
                        posX = r.Next(0, campo.sizeX);
                    }
                    while (Math.Abs(posY - centroTableroY) < campo.sizeX / 4)
                    {
                        posY = r.Next(0, campo.sizeY);
                    }
                    campo.DeployUnidadesAutomatico(posX, posY, unidadesDesplegables);
                }
            }
        }




        public void borrarAcciones()
        {
            this.campo.Acciones.Clear();
        }
    }
}

