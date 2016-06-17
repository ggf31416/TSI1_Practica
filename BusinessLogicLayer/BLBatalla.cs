using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using Shared.Entities;
using System.Diagnostics;

namespace BusinessLogicLayer
{
    public class BLBatalla : IBLBatalla
    {

        // representa las batallas en curso en este servidor
        public Dictionary<string, Batalla> batallasPorJugador = new Dictionary<string, Batalla>();
        public List<Batalla> batallas = new List<Batalla>();

        public Dictionary<string, Jugador> jugadores { get; private set; } = new Dictionary<string, Jugador>();

        private static BLBatalla instancia = null;
        private IBLJuego blJuego;

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
                var client = getCliente();
                var encoladas = new List<string>();
                var batallas_copy = new List<Batalla>(batallas);
                foreach (Batalla b in batallas_copy)
                {
                    if (b.EnCurso)
                    {
                        Stopwatch sw2 = Stopwatch.StartNew();
                        b.ejecutarTurno();
                        Console.WriteLine("un turno demoro:  " + sw2.ElapsedMilliseconds + " ms");
                        sw2.Restart();
                        string jsonAcciones = b.generarListaAccionesTurno();
                        Console.WriteLine("generar acciones demoro:  " + sw2.ElapsedMilliseconds + " ms");
                        if (jsonAcciones.Length > 0)
                        {
                            sw2.Restart();
                            client.SendLista(b.GetListaJugadores(), jsonAcciones);
                            Console.WriteLine("SendLista demoro:  " + sw2.ElapsedMilliseconds + " ms");
                            Console.WriteLine("Long: " + jsonAcciones.Length);
                        }
                        

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
            ServiceInteraccionClient client = new ServiceInteraccionClient(serviceClient.binding, serviceClient.address);
            Batalla b = obtenerBatalla(msg.Jugador);
            if (b == null) return;
            b.tablero.agregarEdificio(new Edificio { tipo_id = msg.Id, jugador = msg.Jugador, posX = msg.PosX, posY = msg.PosY });
            AccionMsg msgSend = new AccionMsg { Accion = "AddEd", Id = msg.Id, PosX = msg.PosX, PosY = msg.PosY };
            client.Send(JsonConvert.SerializeObject(msgSend));

        }



        private static ServiceInteraccionClient getCliente()
        {
            BLServiceClient serviceClient = new BLServiceClient();
            ServiceInteraccionClient client = new ServiceInteraccionClient(serviceClient.binding, serviceClient.address);
            return client;
        }



        private void agregarUnidad(string jugador, int tipo_id, string unit_id, int posX, int posY)
        {
            ServiceInteraccionClient client = getCliente();
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
                getCliente().SendGrupo(obj.Jugador, jsonBatalla);  
            }
        }


        public void IniciarBatalla(string tenant, InfoAtaque info)
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
            
            Batalla b = new Batalla(jAt, jDef);

            if (batallasPorJugador.ContainsKey(info.Jugador)){
                batallasPorJugador[info.Jugador].EnCurso = false;
            }
            if (batallasPorJugador.ContainsKey(info.Enemigo)){
                batallasPorJugador[info.Enemigo].EnCurso = false;
            }
            batallasPorJugador[info.Jugador] = b;
            batallasPorJugador[info.Enemigo] = b;
            batallas.Add(b);
            notificarAsync(info, "IniciarAtaque");
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

        private void notificarAsync(InfoAtaque info,string tipo)
        {
            var client = getCliente();
            var notificar = new string[] { info.Jugador, info.Enemigo };
            for (int i = 0; i < notificar.Length; i++)
            {
                var informacionAtaque = new NotificacionAtaque()
                {
                    Tipo = tipo,
                    TiempoAtaque = 15,
                    Atacante = "Atacante", // cambiar
                    Defensor = "Defensor",
                    SoyAliado = false,
                    SoyAtacante = (i == 0)

                };
                string msg = JsonConvert.SerializeObject(informacionAtaque);
                client.SendGrupo(notificar[i], msg);
            }
        }

        public void IniciarAtaque(string tenant, InfoAtaque info)
        {
            notificarAsync(info, "NotificacionAtaque");
            Planificador.getInstancia().IniciarAtaque(tenant, info, 15);   
        }

        public string getJsonBatalla(string tenant, string idUsuario)
        {
            if (batallasPorJugador.ContainsKey(idUsuario))
            {
                Console.WriteLine("getJsonBatalla Tenant: " + tenant + "userId " + idUsuario);
                return batallasPorJugador[idUsuario].GenerarJson(idUsuario);
            }
            return null;
        }
    }


}
