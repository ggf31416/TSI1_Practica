using Microsoft.VisualStudio.TestTools.UnitTesting;
using BusinessLogicLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.Entities;

namespace BusinessLogicLayer
{
    [TestClass()]
    public class BLJuegoTests
    {
        [TestMethod()]
        public void InicializarEstadoTec()
        {
            Juego juego = new Juego();

            BLJuego bl = new BLJuego(null);
            BLTecnologia tec = new BLTecnologia(bl);
            juego.TipoEdificios = new List<TipoEdificio>();
            juego.TipoRecurso = new List<TipoRecurso>();
            juego.TipoRecurso.Add(new TipoRecurso() { Id = 5, Nombre = "Oro" });
            juego.TipoEdificios.Add(new TipoEdificio() { Ataque = 10, Defensa = 10, Vida = 10, RecursosAsociados = new List<RecursoAsociado>() });
            juego.Tecnologias = new List<Tecnologia>();
            juego.Tecnologias.Add(new Tecnologia() { Id = 1 });
            juego.DataJugador = new DataActual();
            Tecnologia dep = new Tecnologia() { Id = 2, TecnologiaDependencias = new List<TecnologiaDependencia>() };
            dep.TecnologiaDependencias.Add(new TecnologiaDependencia() { IdTecnologiaDepende = 2, IdTecnologia = dep.Id });
            juego.Tecnologias.Add(dep);
            tec.CompletarTecnologiasTerminadasSinGuardar(juego);
            var estado = juego.DataJugador.EstadoTecnologias;
            Assert.AreEqual(EstadoData.EstadoEnum.Puedo,estado["1"].Estado ,"Tec 1 deberia poder desarrollarse");
            Assert.AreEqual(EstadoData.EstadoEnum.NoPuedo,estado["2"].Estado, "Tec 2 todavia no deberia poder desarrollarse");


        }
    }
}