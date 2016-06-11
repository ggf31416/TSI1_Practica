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
            dep.TecnologiaDependencias.Add(new TecnologiaDependencia() { IdTecnologiaDepende = 1, IdTecnologia = dep.Id });
            juego.Tecnologias.Add(dep);
            tec.CompletarTecnologiasTerminadasSinGuardar(juego);
            var estado = juego.DataJugador.EstadoTecnologias;
            Assert.AreEqual(EstadoData.EstadoEnum.Puedo,estado["1"].Estado ,"Tec 1 deberia poder desarrollarse");
            Assert.AreEqual(EstadoData.EstadoEnum.NoPuedo,estado["2"].Estado, "Tec 2 todavia no deberia poder desarrollarse");
            tec.CompletarTecnologiasTerminadasSinGuardar(juego);
            Assert.AreEqual(EstadoData.EstadoEnum.Puedo, estado["1"].Estado, "Tec 1 deberia poder desarrollarse (2 pasos)");
            Assert.AreEqual(EstadoData.EstadoEnum.NoPuedo, estado["2"].Estado, "Tec 2 todavia no deberia poder desarrollarse (2 pasos)");
            juego.DataJugador.EstadoTecnologias["1"].Estado = EstadoData.EstadoEnum.C;
            juego.DataJugador.EstadoTecnologias["1"].Fin = DateTime.UtcNow.AddSeconds(1);
            tec.CompletarTecnologiasTerminadasSinGuardar(juego);
            Assert.AreEqual(EstadoData.EstadoEnum.C, estado["1"].Estado, "Tec 1 deberia estar construyendose");
            Assert.AreEqual(EstadoData.EstadoEnum.NoPuedo, estado["2"].Estado, "Tec 2 todavia no deberia poder desarrollarse)");
            juego.DataJugador.EstadoTecnologias["1"].Estado = EstadoData.EstadoEnum.A;
            tec.CompletarTecnologiasTerminadasSinGuardar(juego);
            Assert.AreEqual(EstadoData.EstadoEnum.A, estado["1"].Estado, "Tec 1 deberia estar construido");
            Assert.AreEqual(EstadoData.EstadoEnum.Puedo, estado["2"].Estado, "Tec 2  deberia poder desarrollarse ahora");
        }
    }
}