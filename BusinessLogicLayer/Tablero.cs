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
    public class Tablero
    {

        Random r = new Random();
        
        private List<Edificio> edificios = new List<Edificio>();
        private string jugadorDefensor;
        private Dictionary<String, List<Unidad>> unidadesPorJugador = new Dictionary<string, List<Unidad>>();
        private Dictionary<string, Unidad> unidades = new Dictionary<string, Unidad>();
        private Dictionary<string, ResultadoBusqPath> paths = new Dictionary<string, ResultadoBusqPath>();
        private static int edificio_size = 4;
        private static int tablero_size = 10;
        private int sizeX = tablero_size * edificio_size;
        private int sizeY = tablero_size * edificio_size;
        private float velocidad = 5;
        JumpPointParam param = null;
        private Stopwatch sw;
        private long nanosPrevio;
        public List<AccionMsg> Acciones { get; private set; } = new List<AccionMsg>();


        private int turno = 0;

        public Tablero()
        {
            param = configurar();
            sw = Stopwatch.StartNew();
            nanosPrevio = sw.ElapsedMilliseconds;
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
            //while (unidades.Keys.Contains(id)) id = r.Next();
            //u.id = id;
            u.jugador = jugador;
            unidades[id] = u;
            unidadesPorJugador[jugador].Add(u);
            //return 
        }

        public void agregarUnidades(String jugador, IEnumerable<Unidad> unidades)
        {
            if (!unidadesPorJugador.ContainsKey(jugador))
            {
                unidadesPorJugador.Add(jugador, new List<Unidad>());
            }

            unidadesPorJugador[jugador].AddRange(unidades);

        }

        

        

  
     

        BaseGrid crearTableroPF()
        {
            bool[][] matrix = new bool[sizeX][];
            for (int i = 0; i < sizeX; i++)
            {
                matrix[i] = new bool[sizeY];
                for (int j = 0; j < sizeY; j++)
                {
                    matrix[i][j] = true;
                }
            }

            foreach (var e in edificios)
            {
                for (int i = 0; i < e.sizeX; i++)
                {
                    for (int j = 0; j < e.sizeY; j++)
                    {
                        matrix[e.posXr + i][e.posYr + j] = false;
                    }
                }
            }
            BaseGrid grilla = new StaticGrid(sizeX, sizeY, matrix);
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
            JumpPointParam param = new JumpPointParam(grilla, new GridPos(0, 0), new GridPos(0, 1), true, cruzarJuntoObstaculo, cruzarPorDiagonal, heuristica_distancia);
            return param;
        }

        public List<GridPos> buscarPath(Unidad u, GridPos dest)
        {
            GridPos start = new GridPos((int)Math.Round(u.posX), (int)Math.Round(u.posY));
            this.param = new JumpPointParam(param.SearchGrid, start, dest, param.AllowEndNodeUnWalkable, param.CrossCorner, param.CrossAdjacentPoint, HeuristicMode.MIXTA15);
            List<GridPos> res = JumpPointFinder.FindPath(param);
            List<Node> nodosUnwalk = ((StaticGrid)this.param.SearchGrid).buscarUnwalkables();
            if (!param.SearchGrid.IsWalkableAt(dest.x, dest.y))
            {
                Console.WriteLine("Is walkable at " + dest.x + "," + dest.y + "? : " + false);
            }

            param.SearchGrid.Reset();
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
            double  distancia = 0;
            Unidad nearest = null;
            foreach (String j in this.unidadesPorJugador.Keys)
            {
                if (!j.Equals(u.jugador))
                {
                    var enemigos = this.unidadesPorJugador[j];
                    foreach (Unidad e in enemigos)
                    {
                        double d = euclides2(u.posX - e.posX, u.posY - e.posY);
                        if (d < distancia)
                        {
                            d = distancia;
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
            if (!u.jugador.Equals(jugadorDefensor))
            {
                foreach (Edificio ed in this.edificios)
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

        GridPos buscarMasCercano(Unidad u)
        {
            Unidad near_u = buscarEnemigoMasCercano(u);
            if (near_u == null)
            {
                Edificio e = buscarEdificioEnemigoMasCercano(u);
                if (e != null)
                {
                    return pos(e);
                }
                else
                {
                    return pos(u);
                }
            }
            return pos(near_u);
        }


        GridPos pos(Entidad u)
        {
            return new GridPos(u.posXr, u.posYr);
        }


        public ResultadoBusqPath[] buscarRutaHaciaEnemigosCercanos()
        {
            Dictionary<Unidad, GridPos> tmp = new Dictionary<Unidad, GridPos>();
            var unidades = unidadesPorJugador.Values.SelectMany(lst => lst);
            //JumpPointParam param = configurar();

            var res = new List<ResultadoBusqPath>();
            foreach (var u in unidades)
            {
                GridPos pos = buscarMasCercano(u);
                var r = buscar(u, pos);
                res.Add(r);
            }
            return res.ToArray();
        }

        public ResultadoBusqPath buscar(Unidad u, GridPos destino)
        {
            var r_path = buscarPath(u,  destino);
            var r = new ResultadoBusqPath() { path = r_path.ToArray() };
            paths.Add(u.id, r);
            return r;
        }


        public ResultadoBusqPath rutaEnemigoMasCercano(Unidad u)
        {
            GridPos destino = buscarMasCercano(u);
            var r_path = buscarPath(u, destino);
            var r = new ResultadoBusqPath() { path = r_path.ToArray() };
            paths.Add(u.id, r);
            return r;
        }


        JumpPointParam configurar()
        {
            BaseGrid grilla = crearTableroPF();
            JumpPointParam param = parametrosBusqueda(grilla);
            return param;
        }

        bool atacarEntidad(Entidad ataq, Entidad def, long deltaT)
        {
            if (ataq.enRango(def))
            {
                float daño = (deltaT / 1000.0f) * 10 * ataq.ataque / (float)def.defensa;
                def.vida -= daño;
                if (def.vida < 0) {
                    matar(def);
                    def.target = null;
                }
                AccionMsg notif = new AccionMsg { Accion = "UpdateHP", IdUnidad = def.id ,ValorN = def.vida};
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

        private void matar(Entidad def)
        {
            unidades.Remove(def.id);
            if (def is Unidad)
            {
                unidadesPorJugador[def.jugador].Remove((Unidad)def);
            }
        }
        

        public void tickTiempo()
        {
            Acciones.Clear();
            //ResultadoBusqPath[] res = buscarRutaHaciaEnemigosCercanos();
            // mover unidades
            long deltaT = sw.ElapsedMilliseconds - nanosPrevio;
            nanosPrevio += deltaT;

            foreach (Unidad u in unidades.Values)
            {
                if (!paths.ContainsKey(u.id))
                {
                    var p = rutaEnemigoMasCercano(u);
                    paths.Add(u.id, p);
                    var accM = new AccionMoverUnidad() { IdUnidad = u.id ,Accion="MoveUnit", PosX = u.posXr, PosY = u.posYr, Path = p.path };
                    Acciones.Add(accM);

                }
                if (paths.ContainsKey(u.id))
                {
                    var p = paths[u.id];
                    // actualizo las posiciones de las unidades en funcion de sus movientos
                    simularMovimiento(deltaT, u,p);
                }
              
                // ya mandamos los paths, no es necesario mandar los valores de x,y actuales
                if (turno % 5 == 0)
                {
                    // buscar target mas cercano
                }
                
                if (u.target != null) {
                    Entidad target = unidades[u.target];
                    atacarEntidad(u, target, deltaT);
                }


            }



        }

        private void simularMovimiento(long deltaT, Unidad u, ResultadoBusqPath p)
        {
            GridPos[] path = p.path;
            int idx = p.idxActual;
            double t_restante = deltaT;
            while (t_restante > 0 && p.idxActual < path.Length)
            {

                GridPos prox = path[idx];
                float dif_x = prox.x - u.posX;
                float dif_y = prox.x - u.posY;
                double prox_dist = euclides(dif_x, dif_y);
                double prox_eta = prox_dist / u.velocidad;
                float avance = (float)Math.Min(t_restante / prox_eta, 1);
                u.posX += avance * dif_x;
                u.posY += avance * dif_y;
                t_restante -= prox_eta;
            }
        }

        public ResultadoBusqPath ordenMoverUnidad(string unidadId, int destinoX, int destinoY)
        {
            GridPos d = new GridPos(destinoX, destinoY);
            Unidad u = unidades[unidadId];
            return buscar(u, d);
        }
    }
}
