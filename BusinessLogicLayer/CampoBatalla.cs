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
using System.Diagnostics;

namespace BusinessLogicLayer
{
    public class CampoBatalla
    {

        Random r = new Random();
        
        private List<Edificio> edificios = new List<Edificio>();
        public string JugadorDefensor { get; set; }
        private Dictionary<String, List<Unidad>> unidadesPorJugador = new Dictionary<string, List<Unidad>>();
        private Dictionary<string, Entidad> entidades = new Dictionary<string, Entidad>();
        private Dictionary<string, ResultadoBusqPath> paths = new Dictionary<string, ResultadoBusqPath>();
        private static int edificio_size = 4;
        private static int tablero_size = 10;
        private int sizeX = tablero_size * edificio_size;
        private int sizeY = tablero_size * edificio_size;

        private bool[][] matrixEdificios;
        private bool[][] matrixUnidades;

        JumpPointParam param = null;
        private Stopwatch sw;
        private long nanosPrevio;
        public List<AccionMsg> Acciones { get; private set; } = new List<AccionMsg>();


        public void RellenarInfoBatalla(Batalla.InfoBatalla info )
        {
            foreach (var ent in entidades.Values)
            {
                if (ent is Unidad) {
                    info.unidades.Add(ent as Unidad);
                }
                else
                {
                    info.edificios.Add(ent as Edificio);
                }
                
            }
        }

        public int Turno { get; private set; }  = 0;

        public CampoBatalla()
        {
            param = configurar();
            sw = Stopwatch.StartNew();
            nanosPrevio = sw.ElapsedMilliseconds;
        }

        public bool QuedanUnidadesJugador(string jugador)
        {
            // cambiar para clan
            if (!unidadesPorJugador.ContainsKey(jugador)) return false;
            return unidadesPorJugador[jugador].Count > 0 && 
                unidadesPorJugador[jugador].Any(u => u.estaViva);
        }

        

        // agregar edificios masivo
        public void agregarEdificios(IEnumerable<Edificio> lst)
        {
            edificios.AddRange(lst);

        }

        // agrega edificio (que ya debe tener posicion) y actualiza la grilla
        public void agregarEdificio(Edificio ed)
        {
            edificios.Add(ed);
            walkableFalse(ed);  
        }
        

        public void agregarUnidad(String jugador, Unidad u)
        {
            if (!unidadesPorJugador.ContainsKey(jugador))
            {
                unidadesPorJugador.Add(jugador, new List<Unidad>());
            }
            string id = u.id;

            u.jugador = jugador;
            entidades[id] = u;
            unidadesPorJugador[jugador].Add(u);
             
        }

        public void agregarUnidades(String jugador, IEnumerable<Unidad> unidades)
        {
            if (!unidadesPorJugador.ContainsKey(jugador))
            {
                unidadesPorJugador.Add(jugador, new List<Unidad>());
            }

            unidadesPorJugador[jugador].AddRange(unidades);

        }

        

        

      
        void hacerUnwalkableEdificios()
        {
            matrixEdificios = new bool[sizeX][];
            for (int i = 0; i < sizeX; i++)
            {
                matrixEdificios[i] = new bool[sizeY];
                for (int j = 0; j < sizeY; j++)
                {
                    matrixEdificios[i][j] = true;
                }
            }

            foreach (var e in edificios)
            {
                for (int i = 0; i < e.sizeX; i++)
                {
                    for (int j = 0; j < e.sizeY; j++)
                    {
                        matrixEdificios[e.posXr + i][e.posYr + j] = false;
                    }
                }
            }
        }

        bool[][] hacerUnwalkableUnidades()
        {
            if (matrixEdificios == null)
            {
                hacerUnwalkableEdificios();
            }
            matrixUnidades = new bool[sizeX][];
            for (int i = 0; i < sizeX; i++)
            {
                matrixUnidades[i] = new bool[sizeY];
                Array.Copy(matrixEdificios[i], matrixUnidades[i], matrixEdificios[i].Length);
            }
            foreach (Entidad e in this.entidades.Values)
            {
                if (e is Unidad && e.estaViva)
                {
                    if (e.posXr < sizeX && e.posYr < sizeY)
                    {
                        matrixUnidades[e.posXr][e.posYr] = false;
                    }
                    else
                    {
                        Console.WriteLine("Error: Pos({0},{1}) fuera de tablero", e.posXr, e.posYr);
                    }
                    
                }
            }
            return matrixUnidades;
        }

        BaseGrid crearTableroPF()
        {
            hacerUnwalkableEdificios();
            var m = hacerUnwalkableUnidades();
            BaseGrid grilla = new StaticGrid(sizeX, sizeY, m);
            return grilla;
        }

        BaseGrid actualizarTableroFP()
        {
            var m = hacerUnwalkableUnidades();
            BaseGrid grilla = new StaticGrid(sizeX, sizeY, m);
            return grilla;
        }



        void walkableFalse(Edificio e)
        {
            for (int i = 0; i < e.sizeX; i++)
            {
                for (int j = 0; j < e.sizeY; j++)
                {
                   this.param.SearchGrid.SetWalkableAt(e.posXr + i,e.posYr + j, false);
                }
            }
        }

        JumpPointParam parametrosBusqueda(BaseGrid grilla)
        {
            bool cruzarJuntoObstaculo = false;
            bool cruzarPorDiagonal = false;
            HeuristicMode heuristica_distancia = HeuristicMode.MIXTA15; // Diagonales valen 1.5
            JumpPointParam param = new JumpPointParam(grilla, true, cruzarJuntoObstaculo, cruzarPorDiagonal, heuristica_distancia);
            return param;
        }

        public List<GridPos> buscarPath(Unidad u, GridPos dest)
        {
            GridPos start = new GridPos((int)Math.Round(u.posX), (int)Math.Round(u.posY));
            param.Reset(start, dest);
            //this.param = new JumpPointParam(param.SearchGrid, start, dest, param.AllowEndNodeUnWalkable, param.CrossCorner, param.CrossAdjacentPoint, HeuristicMode.MIXTA15);
            List<GridPos> res = JumpPointFinder.FindPath(param);
            List<Node> nodosUnwalk = ((StaticGrid)this.param.SearchGrid).buscarUnwalkables();
            if (!param.SearchGrid.IsWalkableAt(dest.x, dest.y))
            {
                Console.WriteLine("Is walkable at " + dest.x + "," + dest.y + "? : " + false);
            }

            //param.SearchGrid.Reset();
            return res;
        }

   
        public class ResultadoBusqPath
        {
            //public string id_unidad { get; set; }
            public GridPos[] path { get; set; }
            public int idxActual = 0;

        }

        private float euclides2(float dx, float dy)
        {
            return dx * dx + dy * dy;
        }

        private double euclides(float dx, float dy)
        {
            return Math.Sqrt(dx * dx + dy * dy);
        }

        public Unidad buscarEnemigoMasCercano(Unidad u)
        {
            double  distancia = Double.PositiveInfinity;
            Unidad nearest = null;
            foreach (String j in this.unidadesPorJugador.Keys)
            {
                if (!j.Equals(u.jugador))
                {
                    var enemigos = this.unidadesPorJugador[j];
                    foreach (Unidad e in enemigos.Where(e => e.estaViva))
                    {
                        double d = euclides2(u.posX - e.posX, u.posY - e.posY);
                        if (d < distancia)
                        {
                            distancia = d;
                            nearest = e;
                        }
                    }
                }
            }
            return nearest;
        }



        public Edificio buscarEdificioEnemigoMasCercano(Unidad u)
        {
            float distancia = 0;
            Edificio nearest = null;
            if (!u.jugador.Equals(JugadorDefensor))
            {
                foreach (Edificio ed in this.edificios.Where(e => e.estaViva))
                {
                    if (!ed.jugador.Equals(u.jugador))
                    {
                        float d = euclides2(u.posX - ed.posX, u.posY - ed.posY);
                        if (d < distancia)
                        {
                            d = distancia;
                            nearest = ed;
                        }
                    }
                }

            }
            return nearest;
        }

        Entidad buscarMasCercano(Unidad u)
        {
            Unidad near_u = buscarEnemigoMasCercano(u);
            if (near_u == null)
            {
                Edificio e = buscarEdificioEnemigoMasCercano(u);
                if (e != null)
                {
                    return e;
                }
                else
                {
                    return null;
                }
            }
            return near_u;
        }

        public ResultadoBusqPath buscarRutaHastaEntidad(Unidad u, Entidad target)
        {
            GridPos posTarget = pos(target);
            return buscar(u, posTarget);
        }


        public ResultadoBusqPath buscarRutaHastaTarget(Unidad u, string target)
        {
            if (!entidades.ContainsKey(target)) return null;
            Entidad uTarget = entidades[target];
            return buscarRutaHastaTarget(u, target);
        }


        GridPos pos(Entidad u)
        {
            return new GridPos(u.posXr, u.posYr);
        }

        


        /*public ResultadoBusqPath[] buscarRutaHaciaEnemigosCercanos()
        {
            Dictionary<Unidad, GridPos> tmp = new Dictionary<Unidad, GridPos>();
            var unidades = unidadesPorJugador.Values.SelectMany(lst => lst);
            //JumpPointParam param = configurar();

            var res = new List<ResultadoBusqPath>();
            foreach (var u in unidades)
            {
                Entidad target = buscarMasCercano(u);
                if (target != null)
                {
                    u.target = target.id;
                    var r = buscarRutaHastaTarget(u, target);
                    res.Add(r);
                }
            }
            return res.ToArray();
        }*/

        public ResultadoBusqPath buscar(Unidad u, GridPos destino)
        {
            var r_path = buscarPath(u,  destino);
            var r = new ResultadoBusqPath() { path = r_path.ToArray() };
            return r;
        }

        private ResultadoBusqPath detener(Entidad u)
        {
            GridPos[] r = { new GridPos(u.posXr, u.posYr) };
            return new ResultadoBusqPath() { path = r };
        }


        public ResultadoBusqPath targetMasCercano(Unidad u)
        {
            Entidad destino = buscarMasCercano(u);
            if (destino != null)
            {
                u.target = destino.id;
                var r_path = buscarRutaHastaEntidad(u, destino); 
                return r_path;
            }
            else
            {
                u.target = null;
                return detener(u);
            }            
        }

        


        JumpPointParam configurar()
        {
            BaseGrid grilla = actualizarTableroFP();
            JumpPointParam param = parametrosBusqueda(grilla);
            return param;
        }

        bool estoyEnRango(Entidad ataq)
        {
            if (ataq.target == null) return false;
            Entidad def = entidades[ataq.target];
            return ataq.enRango(def);
        }

        bool atacarEntidad(Entidad ataq, Entidad def, long deltaT)
        {
            if (ataq.enRango(def))
            {
                detener((Unidad)ataq);
                float daño = (deltaT / 1000.0f) * 10 * ataq.ataque / (float)def.defensa;
                def.hp -= daño;
                if (def.hp < 0) {
                    Console.WriteLine(ataq.id + " mato " + def.id);
                    def.target = null;
                    ataq.target = null;
                }
                AccionMsg notif = new AccionMsg { Accion = "UpdateHP", IdUnidad = def.id ,ValorN = def.hp};
                this.Acciones.Add(notif);
                return true;
            }
            else
            {
                return false;
            }
        
        }

        

        string generarJson()
        {
            return JsonConvert.SerializeObject(this);
        }



        private float tiempo(float dx,float dy,float v)
        {
            double dist = euclides(dx, dy);
            return (float)(10000 * dist / v);
        }

        private List<PuntoRuta> fromPath(ResultadoBusqPath path,Unidad u,int puntosFuturo)
        {
            int i = path.idxActual;
            GridPos[] ruta = path.path;
            var res = new List<PuntoRuta>();
            res.Add(new PuntoRuta() { x = u.posXr, y = u.posYr, t = 0 });
            if (i < ruta.Length)
            {
                int eta = (int)(tiempo(ruta[i].x - u.posXr, ruta[i].y - u.posYr, u.velocidad));
                res.Add(new PuntoRuta() { x = ruta[i].x, y = ruta[i].y, t = eta });
                int puntosElegidos = Math.Min(ruta.Length, puntosFuturo + 1);
                for (int j = i + 1; j < puntosElegidos; j++)
                {
                    eta = (int)tiempo(ruta[j].x - ruta[j - 1].x, ruta[j].y - ruta[j - 1].y, u.velocidad);
                    res.Add(new PuntoRuta() { x = ruta[j].x, y = ruta[j].y, t = eta });
                }
            }
            return res;
        }
        

        public void tickTiempo()
        {
            Acciones.Clear();
            //ResultadoBusqPath[] res = buscarRutaHaciaEnemigosCercanos();
            // mover unidades
            long deltaT = sw.ElapsedMilliseconds - nanosPrevio;
            nanosPrevio += deltaT;

            var muertas = new List<Entidad>();

            foreach (Unidad u in entidades.Values)
            {
                if (u.estaViva)
                {
                    long t_restante = deltaT;

                    // buscar target mas cercano
                    if (!estoyEnRango(u))//(!paths.ContainsKey(u.id) || u.target == null)
                    {
                        string t_ant = u.target;
                        var p = targetMasCercano(u);
                        if (!String.Equals(u.target, t_ant))
                        {
                            Console.WriteLine("Cambio target " + t_ant + " -> " + u.target);
                        }
                        paths[u.id] = p;
                    }

                    // simular movimiento hacia el target
                    if (paths.ContainsKey(u.id))
                    {
                        ResultadoBusqPath p = paths[u.id];
                        // actualizo las posiciones de las unidades en funcion de sus movientos
                        t_restante = simularMovimiento(deltaT, u, p);

                        // detener si estoy en rango
                        if (!u.puedeDispararEnMovimiento && estoyEnRango(u))
                        {
                            p = detener(u);
                        }

                        PuntoRuta[] ruta = fromPath(p, u, 1).ToArray();
                        var accM = new AccionMoverUnidad() { IdUnidad = u.id, Accion = "MoveUnit", PosX = u.posXr, PosY = u.posYr, Path = ruta, Target = u.target };
                        Acciones.Add(accM);
                        //var acc = new AccionMsg() { Accion = "PosUnit", IdUnidad = u.id, PosX = u.posXr, PosY = u.posYr };
                        //Acciones.Add(acc);
                    }

                    // atacar
                    if (u.target != null)
                    {
                        if (!entidades.ContainsKey(u.target))
                        {
                            Console.WriteLine(u.target + " no existe");
                        }
                        else if (entidades[u.target].estaViva)
                        {
                            Entidad target = entidades[u.target];
                            atacarEntidad(u, target, t_restante);
                        }
                        else 
                        {
                            Console.WriteLine(u.target + " no esta viva");
                            u.target = null;
                        }

                    }
                    // actualizo posiciones unidades para pathfinding
                    this.param = configurar();
                }
                else
                {
                    if (Turno % 10 == 0) // notificamos de la muerte por si se perdio la notificacion en la red
                    {
                        AccionMsg notif = new AccionMsg { Accion = "UpdateHP", IdUnidad = u.id, ValorN = u.hp };
                        Acciones.Add(notif);
                    }
                }

            }
            Turno++;
        }

        

       
        private long simularMovimiento(long deltaT, Unidad u, ResultadoBusqPath p)
        {
            GridPos[] path = p.path;
            if (p.idxActual == 0) p.idxActual++; // ignoro el primero que es la posicion actual (me da problemas si la velocidad es muy lenta)
            int idx = p.idxActual;
            double t_restante = deltaT / 1000.0;
            double epsAvance = 0.001;
            if (p.idxActual == p.path.Length) return deltaT; // salgo rapido para facilitar debugging
            int contador = 0; // contador para evitar bucle infinito por error no detectado
            while (t_restante > 0 && p.idxActual < path.Length && contador < 10)
            {
                if (estoyEnRango(u) && !u.puedeDispararEnMovimiento) break;

                GridPos prox = path[p.idxActual];
                float avance = 1;
                float dif_x = prox.x - u.posX;
                float dif_y = prox.y - u.posY;
                double prox_dist = euclides(dif_x, dif_y);
                if (prox_dist > 0)
                {
                    // si la distancia es 0 las operaciones no estan definidas
                    double prox_eta = 10 * prox_dist / u.velocidad;
                    avance = (float)Math.Min(t_restante / prox_eta, 1);
                    u.posX += avance * dif_x;
                    u.posY += avance * dif_y;
                    t_restante -= prox_eta;
                }
                if (avance > 1 - epsAvance)
                {
                    p.idxActual++;
                }
                contador++;
            }
            return (long)Math.Round(t_restante * 1000);
        }

        public ResultadoBusqPath ordenMoverUnidad(string unidadId, int destinoX, int destinoY)
        {
            GridPos d = new GridPos(destinoX, destinoY);
            Unidad u = entidades[unidadId] as Unidad;
            if (u == null) return null;
            return buscar(u, d);
        }

        public Dictionary<int,int> UnidadesSobrevivientes(Jugador j)
        {

            var unidadesJ = unidadesPorJugador.GetValueOrDefault(j.Id);
            var res = new Dictionary<int, int>();
            if (unidadesJ != null)
            {

                var cantVivos = unidadesJ.Where(u => u.estaViva).
                    GroupBy(u => u.tipo_id).
                    Select(grupo => new { Id = grupo.Key, Cantidad = grupo.Count() });
                foreach(var c in cantVivos)
                {
                    res.Add(c.Id, c.Cantidad);
                }
            }
            return res;
        }



        public Entidad GetEntidadDesplegada(string id)
        {
            if (entidades.ContainsKey(id))
            {
                return entidades[id];
            }
            return null;
        }

        private void DeployAutomatico ( IEnumerable<Unidad> unidades,int centroX,int centroY)
        {
            Random rpos = new Random();

            int posX = centroX;
            int posY = centroY;
            int iter = unidades.Count() * 100;
            foreach (Unidad u in unidades)
            {
                while (!matrixUnidades[posX][posY] && iter > 0)
                {
                    posX += rpos.Next(2);
                    if (posX >= sizeX) posX = sizeX - 1;
                    else if (posX < 0) posX = 0;

                    posY += rpos.Next(2);
                    if (posY >= sizeY) posY = sizeY - 1;
                    else if (posY < 0) posY = 0;

                    iter--; // para evitar bucle infinito en caso extremo
                }
                u.posX = posX;
                u.posY = posY;
                matrixUnidades[posX][posY] = false;
            }
        }

        public void DeployUnidadesAutomatico(Jugador jugador, IEnumerable<Unidad> unidades)
        {
            actualizarTableroFP();

            DeployAutomatico(unidades, sizeX / 2, sizeY / 2);
        }
    }
}
