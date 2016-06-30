using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using Shared.Entities;
using System.Diagnostics;
using System.Threading;
using DataAccessLayer;
using System.Threading.Tasks;
using System.Linq;

namespace BusinessLogicLayer
{
    public class BLBatalla : IBLBatalla
    {
        public ConfigBatalla config = new ConfigBatalla();
        // representa las batallas en curso en este servidor
        public Dictionary<string, Batalla> batallasPorJugador = new Dictionary<string, Batalla>();
        public List<Batalla> batallas = new List<Batalla>();

        public Dictionary<string, Jugador> jugadores { get; private set; } = new Dictionary<string, Jugador>();

        private static BLBatalla instancia = null;
        private IBLJuego blJuego;
        private DALAtaqueConj _dalAtConj = new DALAtaqueConj();

        public BLBatalla(IBLJuego bl)
        {
            setBLJuego(bl);
        }

        public BLBatalla() {}

        public static BLBatalla getInstancia(IBLJuego bl)
        {
            if (instancia == null) instancia = new BLBatalla(bl);
            return instancia;
        }

        public void setBLJuego(IBLJuego bl)
        {
            blJuego = bl;
        }

        private bool todaviaEstoyTrabajando = false;


        public void ejecutarBatallasEnCurso()
        {
            if (todaviaEstoyTrabajando)
            {
                Console.WriteLine("[WARNING] Turno todavia en ejecucion, Fecha: " + DateTime.Now.ToString("hh:mm:ss.fff tt"));
                return;
            }

            try
            {
                //Stopwatch sw = Stopwatch.StartNew();
                todaviaEstoyTrabajando = true;
                //Console.WriteLine(DateTime.Now.ToString("hh:mm:ss.fff tt"));
                var client = getClienteInteraccion();
                var encoladas = new List<string>();
                var batallas_copy = new List<Batalla>(batallas);
                foreach (Batalla b in batallas_copy)
                {
                    if (b.EnCurso)
                    {
                        try
                        {

                            Stopwatch sw2 = Stopwatch.StartNew();
                            b.ejecutarTurno();
                            Console.WriteLine("un turno demoro:  " + sw2.ElapsedMilliseconds + " ms");
                            sw2.Restart();
                            string jsonAcciones = b.generarListaAccionesTurno();
                            Console.WriteLine("generar acciones demoro:  " + sw2.ElapsedMilliseconds + " ms");
                            if (jsonAcciones.Length > 0)
                            {
                                try
                                {
                                    sw2.Restart();
                                    client.SendLista(b.GetListaJugadores(), jsonAcciones);
                                    Console.WriteLine("SendLista demoro:  " + sw2.ElapsedMilliseconds + " ms");
                                    Console.WriteLine("Long: " + jsonAcciones.Length);
                                    b.borrarAcciones();
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine("{0} (code={1})", ex.GetType().ToString(), ex.HResult);
                                }

                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.ToString());
                            b.EnCurso = false;
                        }
                        
                    }
                    else if (!b.EnFinalizacion)
                    {
                        Task.Factory.StartNew(() =>
                        {
                            FinBatalla(b);
                            this.batallas.Remove(b);
                        });
                    }
                    
                }
                //Console.WriteLine("Ejecutar turno:  " + sw.ElapsedMilliseconds + " ms");
            }
            catch( Exception ex)
            {
                Console.WriteLine("Error al ejecutar turno: " + ex.ToString());
            }
            finally
            {
                todaviaEstoyTrabajando = false;
            }
        }


        /*private void crearBatalla(string jugador)
        {
            if (!batallasPorJugador.ContainsKey(jugador))
            {
                Batalla tmp = new Batalla("", jugador);
                batallasPorJugador.Add(jugador, tmp);
                tmp.inicializar();
            }
        }*/

        public void agregarEdificio(AccionMsg msg)
        {
            BLServiceClient serviceClient = new BLServiceClient();
            Service1Client client = new Service1Client();
            Batalla b = obtenerBatalla(msg.Jugador);
            if (b == null) return;
            b.tablero.agregarEdificio(new Edificio { tipo_id = msg.Id, jugador = msg.Jugador, posX = msg.PosX, posY = msg.PosY });
            AccionMsg msgSend = new AccionMsg { Accion = "AddEd", Id = msg.Id, PosX = msg.PosX, PosY = msg.PosY };
            client.Send(JsonConvert.SerializeObject(msgSend));

        }



        private static Service1Client getClienteInteraccion()
        {
            BLServiceClient serviceClient = new BLServiceClient();
            Service1Client client = new Service1Client();
            return client;
        }



        private void agregarUnidad(string jugador, int tipo_id, string unit_id, int posX, int posY)
        {
            Service1Client client = getClienteInteraccion();
            Batalla b = obtenerBatalla(jugador);
            if (b == null) return;
            if (b.agregarUnidad(tipo_id, jugador, unit_id, posX, posY) == 1)
            {

                Entidad u = b.tablero.GetEntidadDesplegada(unit_id);
                var jsonObj = new  AccionMsg{ Accion = "AddUn", Id = tipo_id, PosX = posX, PosY = posY, IdUnidad = unit_id,Jugador = jugador};
                string s = JsonConvert.SerializeObject(jsonObj);

                client.SendLista(b.GetListaJugadores(), s);
            }

        }


        private Batalla obtenerBatalla(string jugador)
        {
            if (!batallasPorJugador.ContainsKey(jugador))
            {
                return null;
            }
            Batalla b = batallasPorJugador[jugador];
            return b;
        }

        public void Accion(string tenant, string json)
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
            else if (accion.Equals("BU"))
            {
                //string nombreTipo = "Tipo: " + (obj.Id.GetType().FullName);

                int tipoId = (int)obj.Id; ;
                jugadores[obj.Jugador].AgregarUnidad(tipoId);
            }
            else if (accion.Equals("AddEd"))
            {
                agregarEdificio(obj);
            }
            else if (accion.Equals("GetEstadoBatalla"))
            {
                string jsonBatalla = getJsonBatalla(tenant,obj.Jugador);
                // mando info batalla por signalr
                getClienteInteraccion().SendGrupo(obj.Jugador, jsonBatalla);  
            }
        }


        /*public void IniciarBatalla(string tenant, InfoAtaque info)
        {
            Juego datosAtaq = blJuego.GetJuegoUsuarioSinGuardar(tenant, info.Jugador);
            Jugador jAt = new Jugador();

            jAt.CargarDesdeJuego(datosAtaq);

            Juego datosDef = blJuego.GetJuegoUsuarioSinGuardar(tenant, info.Enemigo);
            Jugador jDef = new Jugador();
            if (datosDef != null)
            {
                
                jDef.CargarDesdeJuego(datosDef);
            }
            else
            {
                ConjuntoUnidades cu = new ConjuntoUnidades() { Cantidad = 5, UnidadId = jAt.tiposUnidad[0].Id };
                jDef = new Jugador()
                {
                    Id = info.Enemigo,
                    Clan = info.Enemigo,
                    tipos = jAt.tipos
                };
                jDef.Unidades.Add(cu.UnidadId,cu);
            }
            
            Batalla b = new Batalla(datosAtaq.Tablero,jAt, jDef);

            if (batallasPorJugador.ContainsKey(info.Jugador)){
                batallasPorJugador[info.Jugador].EnCurso = false;
            }
            if (batallasPorJugador.ContainsKey(info.Enemigo)){
                batallasPorJugador[info.Enemigo].EnCurso = false;
            }
            batallasPorJugador[info.Jugador] = b;
            batallasPorJugador[info.Enemigo] = b;
            batallas.Add(b);
            notificar(info, "IniciarAtaque");
        }*/

        public void IniciarBatalla(string tenant,string IdBatalla)
        {
            AtaqueConjunto info =  _dalAtConj.obtenerAtaqueConj(tenant, IdBatalla);
            IniciarBatalla(tenant, info);
        }

        private void IniciarBatalla(string tenant, AtaqueConjunto info)
        {
            Console.WriteLine("Inicia Batalla!");
            var contribuciones = _dalAtConj.obtenerContribuciones(tenant,info.IdBatalla);
            var jugadores = new List<Jugador>();

            Juego datosDef = blJuego.GetJuegoUsuarioSinGuardar(tenant, info.Defensor);
            Jugador jDef = new Jugador();
            if (datosDef != null)
            {

                jDef.CargarDesdeJuego(datosDef, true);
                jugadores.Add(jDef);
            }

            Batalla b = new Batalla(datosDef.Tablero, jDef, this.config, tenant);
            b.BatallaId = info.IdBatalla;
            batallasPorJugador[info.Defensor] = b;

            foreach (Contribucion contr in contribuciones)
            {
                Juego datosContr = blJuego.GetJuegoUsuarioSinGuardar(tenant, info.Atacante);
                Jugador jug = new Jugador();
                jug.CargarDesdeJuego(datosContr, false);
                jug.CargarDesdeContribucion(contr);
                b.AgregarJugador(jug);
                batallasPorJugador[jug.Id] = b;
            }

            batallas.Add(b);
           NotificarInicioAtaque(b.GetListaJugadores(), "IniciarAtaque");
        }




        class NotificacionAtaque
        {
            public string Tipo { get; set; }
            public long TiempoAtaque { get; set; }
            public string Atacante { get; set; }
            public string Defensor { get; set; }
            public bool SoyAtacante { get; set; }
            public bool SoyAliado { get; set; }
        }

        class Notificacion
        {
            public string Tipo { get; set; }
            [JsonProperty(PropertyName="Msg")]
            public string Mensaje { get; set; }
        }

        private void notificarFin(String jug,string mensaje)
        {
            var client = getClienteInteraccion();

                Notificacion msg = new Notificacion() { Tipo = "FinBatalla", Mensaje = mensaje };
                client.SendGrupo(jug, JsonConvert.SerializeObject(msg));
            
        }

        private void NotificarInicioAtaque(String[] jugadores,string tipo)
        {
            var client = getClienteInteraccion();
            var informacionAtaque = new NotificacionAtaque()
            {
                Tipo = tipo,
            };
            string msg = JsonConvert.SerializeObject(informacionAtaque);
            
            client.SendLista(jugadores , msg);
        }

        private void notificar(InfoAtaque info,string tipo,int segundos)
        {
            var client = getClienteInteraccion();
            var notificar = new string[] { info.Jugador, info.Enemigo };
            for (int i = 0; i < notificar.Length; i++)
            {
                var informacionAtaque = new NotificacionAtaque()
                {
                    Tipo = tipo,
                    TiempoAtaque = segundos,
                    Atacante = "Atacante", // cambiar
                    Defensor = "Defensor",
                    SoyAliado = false,
                    SoyAtacante = (i == 0)

                };
                string msg = JsonConvert.SerializeObject(informacionAtaque);
                client.SendGrupo(notificar[i], msg);
            }
        }

        private bool setAtacabilidadJugador(string tenant,string idJugador,bool atacable)
        {
            IDALUsuario dalUsuario = new DALUsuario(tenant);
            return dalUsuario.SetAtacableJugador(tenant, idJugador, atacable);
        }

        private string getNombreJugador(string tenant,string idJugador)
        {
            IDALUsuario dalUsuario = new DALUsuario(tenant);
            return dalUsuario.GetUserName(tenant, idJugador);
        }

        private Contribucion obtenerUnidades(Juego j)
        {
            Contribucion c = new Contribucion() { IdJugador = j.IdJugador };
            foreach(var u in j.DataJugador.EstadoUnidades.Values)
            {
                if (u.Estado == EstadoData.EstadoEnum.A)
                {
                    c.UnidadesContribuidas.Add(new ConjuntoUnidades() { UnidadId = u.Id, Cantidad = u.Cantidad });
                }
            }
            c.CantUnidades = c.UnidadesContribuidas.Sum(cu => cu.Cantidad);
            c.SoyAtacante = true;
            return c;
        }

        // inicia preparativos para ataque
        public string IniciarAtaque(string tenant, InfoAtaque info)
        {

            //setAtacabilidadJugador(tenant, info.Enemigo,false);
            AtaqueConjunto conj = new AtaqueConjunto() { Atacante = info.Jugador, Defensor = info.Enemigo, IdBatalla = info.Enemigo, Tenant = tenant };
            Juego infoAtacante = blJuego.GetJuegoUsuarioSinGuardar(tenant, conj.Atacante);
            conj.ClanAtacante = infoAtacante.DataJugador.Clan;
            Juego infoDefensor = blJuego.GetJuegoUsuarioSinGuardar(tenant, conj.Atacante);
            conj.ClanDefensor = infoDefensor.DataJugador.Clan;
            conj.FechaAtaque = DateTime.UtcNow.AddSeconds(this.config.SegundosAtaque);
            conj.Jugadores.Add(conj.Atacante);
            conj.Jugadores.Add(conj.Defensor);

            string userName = getNombreJugador(tenant, conj.Defensor);
            conj.NombreDefensor = userName;
            var idBatalla = "";
            try
            {
                idBatalla = _dalAtConj.guardarAtaqueConj(conj);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                idBatalla = _dalAtConj.guardarAtaqueConj(conj);
            }


            Contribucion contr = obtenerUnidades(infoAtacante);
            contr.NombreDefensor = userName;
            contr.IdBatalla = idBatalla;
            _dalAtConj.agregarContribucion(tenant, idBatalla, contr);
            //conj.UnidadesContribuidas.Add(contr);

            blJuego.QuitarUnidades(infoAtacante, contr, true);
            return "pene";
            //Planificador.getInstancia().IniciarAtaque(tenant, idBatalla, (this.config.SegundosAtaque));
            //notificar(info, "NotificacionAtaque", this.config.SegundosAtaque);
        }

        public string getJsonBatalla(string tenant, string idUsuario)
        {
            if (batallasPorJugador.ContainsKey(idUsuario))
            {
                Console.WriteLine("getJsonBatalla Tenant: " + tenant + "userId " + idUsuario);
                var res = batallasPorJugador[idUsuario].GenerarJson(idUsuario);
                var info = new AccionMsg() { Accion = "IniciarAtaque", Data = res };
                Service1Client client = getClienteInteraccion();
                string txt = JsonConvert.SerializeObject(info, new JsonSerializerSettings() { DefaultValueHandling = DefaultValueHandling.Ignore });
                client.SendGrupo(idUsuario, txt);
                return res;
            }
            return null;
        }

        public bool agregarContribucion(string tenant, string idBatalla, Contribucion contr)
        {

            DALAtaqueConj dal = new DALAtaqueConj();
            AtaqueConjunto atConj = dal.obtenerAtaqueConj(tenant, idBatalla);
            if (atConj.Habilitado == false) return false;
            contr.FechaAtaque = atConj.FechaAtaque;
            dal.agregarContribucion(tenant, idBatalla,  contr);
            dal.guardarAtaqueConj(atConj);
            return true;

        }

        public List<InfoAtaqConj> GetSolicitudesAtaqueConjunto(string tenant, string IdJugador,string Clan)
        {
            var info = _dalAtConj.obtenerAtaquesClan(tenant, Clan, IdJugador);
            return info;
        }

        public List<InfoContribucion> GetEnviosEjercitos(string tenant,string IdJugador)
        {
            var lista = _dalAtConj.obtenerContribucionesUsuario(tenant, IdJugador);
            return lista;
        }

        public void FinBatalla(Batalla b)
        {
            Console.WriteLine("Inicia Fin Batalla");
            try
            {
                b.EnFinalizacion = true;
                var listaGanadores = b.ObtenerGanadores(); // obtiene jugador o jugadores (si es un clan) ganador
                Dictionary<string, double> agregar = new Dictionary<string, double>();
                bool ganoAtacante = b.ganoAtacante();
                int cantGanadores = listaGanadores.Count;
                if (b.ganoAtacante())
                {
                    var datosDefensor = blJuego.GetJuegoUsuarioSinGuardar(b.Tenant, b.IdDefensor());
                    var recursos = datosDefensor.DataJugador.EstadoRecursos;

                    foreach (string idRec in recursos.Keys)
                    {
                        agregar.Add(idRec, Math.Floor(recursos[idRec].Total));
                    }
                }
                Dictionary<string, string> mensajes = new Dictionary<string, string>();
                foreach (string jug in b.GetListaJugadores())
                {
                    double mult = 0;
                    string msg = "Empató la batalla.";
                    
                    if (ganoAtacante)
                    {
                        if (listaGanadores.Contains(jug))
                        {
                            mult = 1.0 / cantGanadores;
                            //msg = "Ganó la batalla.";
                        }
                        else if (jug.Equals(b.IdDefensor()))
                        {
                            mult = -1;
                            msg = "Perdió la batalla.";
                        }
                    }
                    if (listaGanadores.Contains(jug))
                    {
                        msg = "Ganó la batalla.";
                    }

                    mensajes[jug] = msg;
                    if (jug == b.IdDefensor())
                    {
                        blJuego.ModificarUnidadesRecursos(b.Tenant, jug, b.UnidadesPerdidas(jug), agregar, b.Config.FraccionRecursos * mult);
                    }
                    else
                    {
                        blJuego.ModificarUnidadesRecursos(b.Tenant, jug, b.UnidadesSobrevivientes(jug), agregar, b.Config.FraccionRecursos * mult);
                    }
                    
                }
                //setAtacabilidadJugador(b.Tenant, b.IdDefensor(), true);
                foreach (string jug in mensajes.Keys)
                {
                    notificarFin(jug, mensajes[jug]);
                }
                _dalAtConj.eliminarAtaqueConjunto(b.Tenant, b.BatallaId);
                Console.WriteLine("Termina Fin Batalla");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error Fin Batalla");
                Console.WriteLine(ex.ToString());
                CancelarBatalla(b.Tenant, b.BatallaId);
            }
            
        }

        public void CancelarBatalla(string tenant,string idBatalla)
        {
            Console.WriteLine("Inicia Cancelar Batalla");
            var contribuciones = _dalAtConj.obtenerContribuciones(tenant, idBatalla);
            foreach(var contr in contribuciones)
            {
                Dictionary<int, int> unidades = new Dictionary<int, int>();
                contr.UnidadesContribuidas.ForEach(cu => unidades.Add(cu.UnidadId, cu.Cantidad));
                blJuego.ModificarUnidadesRecursos(tenant, contr.IdJugador, unidades, new Dictionary<string, double>(), 0);
            }
            _dalAtConj.eliminarAtaqueConjunto(tenant,idBatalla);
            Console.WriteLine("Termina Cancelar Batalla");
        }


    }


}
