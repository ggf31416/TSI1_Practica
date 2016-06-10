using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.Entities;
using Newtonsoft.Json;

namespace BusinessLogicLayer
{
    public class Jugador
    {
        public string Id { get; set; }
        public string Clan { get; set; }
        public Dictionary<int,ConjuntoUnidades> Unidades { get; set; } = new Dictionary<int, ConjuntoUnidades>();
        public List<Edificio> Edificios { get; set; } = new List<Edificio>();
        private Dictionary<int, CantidadRecurso> Recursos { get; set; }  // clave Recurso.ID
        public Dictionary<int, TipoEntidad> tipos = new Dictionary<int, TipoEntidad>();
        private DateTime ultimaActualizacionRecursos;

        public void Inicializar(Shared.Entities.Juego juego)
        {
            foreach(var entidad in juego.TipoEntidad)
            {
                this.tipos.Add(entidad.Id, entidad);
            }
            foreach(var recurso in juego.TipoRecurso)
            {
                this.Recursos.Add(recurso.Id,new CantidadRecurso() { acumulado = 0, porSegundo = 0 });
            }
            ultimaActualizacionRecursos = DateTime.UtcNow;
        }

        public void CargarEdificios(Tablero miBase)
        {
            var ocupadas = miBase.Celdas.Where(c => c.IdTipoEdificio.HasValue && c.IdTipoEdificio >= 0);
            foreach(TableroCelda tc in ocupadas)
            {
                TipoEdificio te = tipos.GetValueOrDefault(tc.IdTipoEdificio.Value) as TipoEdificio;
                if (te != null)
                {
                    Edificio e = new Edificio();
                    e.DesdeTipo(te);
                    e.jugador = this.Id;
                    e.posX = tc.PosColumna.Value * e.sizeX;
                    e.posY = tc.PosFila.Value * e.sizeY;
                }
            }
        }


        public Jugador()
        {
            // TODO: falta inicializar para cada recurso
            Recursos = new Dictionary<int, CantidadRecurso>();

            ultimaActualizacionRecursos = DateTime.UtcNow;

        }

        private void actualizarRecursosPorSegundo()
        {
            foreach (var cant in Recursos.Values)
            {
                cant.porSegundo = 0;
            }
            foreach (Edificio e in Edificios)
            {
                TipoEdificio te = tipos[e.tipo_id] as TipoEdificio;
                foreach (var prod in te.RecursosAsociados)
                {
                    Recursos[prod.IdRecurso].porSegundo += prod.Valor;
                }
            }
        }

        private void actualizarRecursos(DateTime now)
        {
            TimeSpan dif = now - ultimaActualizacionRecursos;
            foreach (var cant in Recursos.Values)
            {
                cant.acumulado += cant.porSegundo * dif.TotalSeconds;
            }
            ultimaActualizacionRecursos = now;
        }

        public bool Construir(TipoEdificio e)
        {
            actualizarRecursos(DateTime.UtcNow);
            // tengo la cantidad necesaria para todos los recursos:
            bool puedoConstruir = e.Costos.All(costoRec => Recursos[costoRec.IdRecurso].acumulado >= costoRec.Valor);
            if (puedoConstruir)
            {
                foreach (var costoRec in e.Costos)
                {
                    Recursos[costoRec.IdRecurso].acumulado -= costoRec.Valor;
                }
                actualizarRecursosPorSegundo();
            }
            return puedoConstruir;
        }


        public String GenerarJson(bool incluirEdificios,bool incluirRecursos)
        {
            InfoJugador copia = new InfoJugador();
            copia.Id = this.Id;
            copia.Clan = this.Clan;
            copia.Unidades = new List<ConjuntoUnidades>(this.Unidades.Values);
            if (incluirEdificios)
            {
                copia.Edificios = this.Edificios;
            }
            if (incluirRecursos)
            {
                copia.Recursos = this.Recursos;
            }
            string json  =JsonConvert.SerializeObject(copia);
            return json;
        }

        public void AgregarUnidad(int tipoId)
        {
            if (!Unidades.ContainsKey(tipoId))
            {
                Unidades.Add(tipoId, new ConjuntoUnidades() { UnidadId = tipoId, Cantidad = 1 });
            }
            else
            {
                Unidades[tipoId].Cantidad += 1;
            }
        }

        private double aplicarPorcentaje(double valor, double porc)
        {
            return valor * (1 + 0.01 * porc);
        }

        public void AplicarTecnologia(Tecnologia tec)
        {
            foreach (Accion a in tec.AccionesAsociadas)
            {
                AplicarAccion(a);
            }
        }

        public void AplicarAccion(Accion accion)
        {
            if (accion.IdEntidad.HasValue)
            {
                TipoEntidad te = tipos.GetValueOrDefault(accion.IdEntidad.Value);
                string atr = accion.NombreAtributo.ToLowerInvariant();
                if (atr.Equals("ataque"))
                {
                    te.Ataque += accion.Valor;
                }
                else if (atr.Equals("defensa"))
                {
                    te.Defensa += accion.Valor;
                }
                else if (atr.Equals("vida"))
                {
                    te.Vida += accion.Valor;
                }
            }
        }
    }


    public class InfoJugador
    {
        public string Id { get; set; }
        public string Clan { get; set; }
        public List< ConjuntoUnidades> Unidades { get; set; } = new List<ConjuntoUnidades>();
        public List<Edificio> Edificios { get; set; } = new List<Edificio>();
        public Dictionary<int, CantidadRecurso> Recursos { get; set; }  // clave Recurso.ID
    }




}

