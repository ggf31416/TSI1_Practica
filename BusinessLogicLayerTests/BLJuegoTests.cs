using Microsoft.VisualStudio.TestTools.UnitTesting;
using BusinessLogicLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.Entities;

namespace BusinessLogicLayer.Tests
{
    [TestClass()]
    public class BLJuegoTests
    {
        [TestMethod()]
        public void ActualizarJuegoSinGuardarTest()
        {
            Juego juego = new Juego();

            BLJuego bl = new BLJuego(null);
            BLTecnologia tec = new BLTecnologia(bl);
            juego.TipoEdificios = new List<TipoEdificio>();
            juego.TipoRecurso = new List<TipoRecurso>();
            juego.TipoRecurso.Add(new TipoRecurso() { Id = 5, Nombre = "Oro" });
            juego.TipoEdificios.Add(new TipoEdificio() { Ataque = 10, Defensa = 10, Vida = 10, RecursosAsociados = new List<RecursoAsociado>() });



        }
    }
}