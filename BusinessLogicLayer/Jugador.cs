using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.Entities;

namespace BusinessLogicLayer
{
    public class Jugador
    {
        public string Id { get; set; }
        public string Clan { get; set; }
        public List<ConjuntoUnidades> Unidades { get; set; } = new List<ConjuntoUnidades>();
        public List<Edificio> Edificios { get; set; } = new List<Edificio>();
        private Dictionary<int, CantidadRecurso> Recursos { get; set; }  // clave Recurso.ID
        private DateTime ultimaActualizacionRecursos;



        public Jugador()
        {
            // TODO: falta inicializar para cada recurso
            Recursos = new Dictionary<int, CantidadRecurso>();

            ultimaActualizacionRecursos = DateTime.Now;

        }

        private void actualizarRecursosPorSegundo()
        {
            foreach (var cant in Recursos.Values)
            {
                cant.porSegundo = 0;
            }
            foreach (Edificio e in Edificios)
            {
                foreach (var prod in e.produccion)
                {
                    Recursos[prod.Key.Id].porSegundo += prod.Value;
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

        public bool RecursosConstruir(Edificio e)
        {
            actualizarRecursos(DateTime.Now);
            // tengo la cantidad necesaria para todos los recursos:
            bool puedoConstruir = e.costo.All(costoRec => Recursos[costoRec.Key.Id].acumulado >= costoRec.Value);
            if (puedoConstruir)
            {
                foreach (var costoRec in e.costo)
                {
                    Recursos[costoRec.Key.Id].acumulado -= costoRec.Value;
                }
                actualizarRecursosPorSegundo();
            }
            return puedoConstruir;
        }


    }


    public class CantidadRecurso
    {
        public double porSegundo { get; set; }
        public double acumulado { get; set; }
    }

    public class ConjuntoUnidades
    {
        public int UnidadId { get; set; }
        public int Cantidad { get; set; }
    }
}

