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

namespace BusinessLogicLayer
{
    public class Tablero
    {

        Random r = new Random();
        
        private List<Edificio> edificios = new List<Edificio>();
        private string jugadorDefensor;
        private Dictionary<String, List<Unidad>> unidadesPorJugador = new Dictionary<string, List<Unidad>>();
        private Dictionary<string, Unidad> unidades = new Dictionary<string, Unidad>();
        private static int edificio_size = 4;
        private static int tablero_size = 10;
        private int sizeX = tablero_size * edificio_size;
        private int sizeY = tablero_size * edificio_size;
        private float velocidad = 5;
        JumpPointParam param = null;

        public Tablero()
        {
            param = configurar();
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
                        matrix[e.posX + i][e.posY + j] = false;
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
                   this.param.SearchGrid.SetWalkableAt(e.posX + i,e.posY + j, false);
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
            GridPos start = new GridPos(u.posX, u.posY);
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
            public string id_unidad { get; set; }
            public GridPos[] path { get; set; }


        }

        private int euclides2(int dx, int dy)
        {
            return dx * dx + dy * dy;
        }

        public Unidad buscarEnemigoMasCercano(Unidad u)
        {
            int distancia = 0;
            Unidad nearest = null;
            foreach (String j in this.unidadesPorJugador.Keys)
            {
                if (!j.Equals(u.jugador))
                {
                    var enemigos = this.unidadesPorJugador[j];
                    foreach (Unidad e in enemigos)
                    {
                        int d = euclides2(u.posX - e.posX, u.posY - e.posY);
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
            int distancia = 0;
            Edificio nearest = null;
            if (!u.jugador.Equals(jugadorDefensor))
            {
                foreach (Edificio ed in this.edificios)
                {
                    if (!ed.jugador.Equals(u.jugador))
                    {
                        int d = euclides2(u.posX - ed.posX, u.posY - ed.posY);
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
                    return new GridPos(e.posX, e.posY);
                }
                else
                {
                    return new GridPos(u.posX, u.posY);
                }
            }
            return new GridPos(near_u.posX, near_u.posY);
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
            var r = new ResultadoBusqPath() { id_unidad = u.id, path = r_path.ToArray() };
            return r;
        }

        JumpPointParam configurar()
        {
            BaseGrid grilla = crearTableroPF();
            JumpPointParam param = parametrosBusqueda(grilla);
            return param;
        }

        void atacarUnidad(Unidad ataq, Unidad def)
        {
            if (ataq.distancia(def) < ataq.rango)
            {
                float daño = 10.0f * ataq.ataque / def.defensa;
                ataq.vida -= daño;
                if (ataq.vida < 0) { matar(def); }
            }
        }

        string generarJson()
        {
            return JsonConvert.SerializeObject(this);
        }

        private void matar(Unidad def)
        {
            unidadesPorJugador[def.jugador].Remove(def);
        }

        void tickTiempo()
        {
            ResultadoBusqPath[] res = buscarRutaHaciaEnemigosCercanos();
        }

        public ResultadoBusqPath ordenMoverUnidad(string unidadId, int destinoX, int destinoY)
        {
            GridPos d = new GridPos(destinoX, destinoY);
            Unidad u = unidades[unidadId];
            return buscar(u, d);
        }
    }
}
