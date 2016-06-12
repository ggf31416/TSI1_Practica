using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ServiceStack.Redis;
using Microsoft.AspNet.SignalR;
using FrontEnd.ServiceTablero;
using Shared.Entities;

namespace FrontEnd.Controllers
{
    public class JuegoController : Controller
    {
        [HttpGet]
        public ActionResult GetAllDataJuego(string tenant)
        {
            ServiceTableroClient client = new ServiceTableroClient();

            Models.AllDataJuegoModel ret = new Models.AllDataJuegoModel();

            Juego juego = client.GetAllDataJuego(tenant);

            //Datos Juegos
            Models.JuegoModel retJuegoModel = new Models.JuegoModel();

            retJuegoModel.Id = juego.Id;
            retJuegoModel.IdJugador = juego.IdJugador;
            retJuegoModel.Nombre = juego.Nombre;
            retJuegoModel.Estado = juego.Estado;
            retJuegoModel.IdDisenador = juego.IdDisenador;
            retJuegoModel.Imagen = juego.Imagen;
            retJuegoModel.Url = juego.Url;

            ret.DataJuego = retJuegoModel;

            //Datos Tablero
            Models.TableroModel retTableroModel = new Models.TableroModel();

            retTableroModel.Id = juego.Tablero.Id;
            retTableroModel.IdJuego = juego.Tablero.Id;
            retTableroModel.ImagenFondo = juego.Tablero.ImagenFondo;
            retTableroModel.ImagenTerreno = juego.Tablero.ImagenTerreno;
            retTableroModel.Columnas = new List<Models.ColumnaModel>();

            for (int i = 0; i < juego.Tablero.CantFilas; i++)
            {
                Models.ColumnaModel col = new Models.ColumnaModel();
                col.Fila = new List<Models.IdModel>();
                retTableroModel.Columnas.Add(col);
            }


            foreach (Shared.Entities.TableroCelda tc in juego.Tablero.Celdas.OrderBy(c => c.PosColumna))
            {
                Models.IdModel celda = new Models.IdModel();
                celda.Id = tc.IdTipoEdificio == null ? -1 : (int)tc.IdTipoEdificio;
                retTableroModel.Columnas.ElementAt((int)tc.PosFila).Fila.Add(celda);
            }

            ret.Tablero = retTableroModel;

            //Datos Tecnologias
            ret.Tecnologias = new List<Models.TecnologiaModel>();

            foreach (Shared.Entities.Tecnologia t in juego.Tecnologias)
            {
                Models.TecnologiaModel newTecnologia = new Models.TecnologiaModel();

                newTecnologia.Id = t.Id;
                newTecnologia.Nombre = t.Nombre;
                newTecnologia.Imagen = t.Imagen;
                newTecnologia.IdJuego = t.IdJuego;
                newTecnologia.TiempoConstruccion = t.Tiempo;
                if (t.AccionesAsociadas != null)
                {
                    newTecnologia.AccionesAsociadas = new List<Models.AccionModel>();
                    foreach (Shared.Entities.Accion a in t.AccionesAsociadas)
                    {
                        Models.AccionModel accion = new Models.AccionModel();
                        accion.Id = a.Id;
                        accion.IdJuego = a.IdJuego == null ? -1 : a.IdJuego.GetValueOrDefault();
                        accion.IdTecnologia = a.IdTecnologia == null ? -1 : a.IdTecnologia.GetValueOrDefault();
                        accion.Nombre = a.Nombre;
                        accion.NombreAtributo = a.NombreAtributo;
                        accion.ValorPor = a.ValorPor;
                        accion.Valor = a.Valor;
                        accion.IdEntidad = a.IdEntidad;
                        newTecnologia.AccionesAsociadas.Add(accion);
                    }
                }
                if (t.TecnologiaRecursoCostos != null)
                {
                    newTecnologia.Costos = new List<Models.Costo>();
                    foreach (var c in t.TecnologiaRecursoCostos)
                    {
                        Models.Costo costo = new Models.Costo();
                        costo.IdRecurso = c.IdRecurso;
                        costo.Value = c.Costo;
                        newTecnologia.Costos.Add(costo);
                    }
                }
                if (t.TecnologiaDependencias != null)
                {
                    newTecnologia.TecnologiaDependencias = new List<Models.TecnologiaDependeModel>();
                    foreach (var c in t.TecnologiaDependencias)
                    {
                        Models.TecnologiaDependeModel dep = new Models.TecnologiaDependeModel();
                        dep.IdTecnologiaDepende = c.IdTecnologiaDepende;
                        newTecnologia.TecnologiaDependencias.Add(dep);
                    }
                }
                ret.Tecnologias.Add(newTecnologia);
            }

            //Datos Recursos
            ret.TipoRecursos = new List<Models.TipoRecursoModel>();

            foreach (Shared.Entities.TipoRecurso tE in juego.TipoRecurso)
            {
                Models.TipoRecursoModel newTipoRecursos = new Models.TipoRecursoModel();

                newTipoRecursos.Id = tE.Id;
                newTipoRecursos.IdJuego = tE.IdJuego;
                newTipoRecursos.Nombre = tE.Nombre;
                newTipoRecursos.Imagen = tE.Imagen;

                ret.TipoRecursos.Add(newTipoRecursos);
            }

            //Datos TipoEdificios
            ret.TipoEdificios = new List<Models.TipoEntidadModel>();

            foreach (var tE in juego.TipoEdificios)
            {
                Models.TipoEntidadModel newTipoEntidad = new Models.TipoEntidadModel();

                newTipoEntidad.IdJuego = tE.IdJuego;
                newTipoEntidad.Id = tE.Id;
                newTipoEntidad.Nombre = tE.Nombre;
                newTipoEntidad.Vida = tE.Vida;
                newTipoEntidad.Defensa = tE.Defensa;
                newTipoEntidad.Imagen = tE.Imagen;
                newTipoEntidad.Ataque = tE.Ataque;
                newTipoEntidad.TiempoConstruccion = tE.TiempoConstruccion;
                if (tE.UnidadesAsociadas != null)
                    newTipoEntidad.UnidadesAsociadas = tE.UnidadesAsociadas.ToList();
                else
                    newTipoEntidad.UnidadesAsociadas = new List<int>();
                newTipoEntidad.RecursosAsociados = new List<Models.RecursoAsociado>();
                if (tE.RecursosAsociados != null)
                {
                    foreach (Shared.Entities.RecursoAsociado c in tE.RecursosAsociados)
                    {
                        Models.RecursoAsociado recProd = new Models.RecursoAsociado();
                        recProd.IdRecurso = c.IdRecurso;
                        recProd.Value = c.Valor;
                        newTipoEntidad.RecursosAsociados.Add(recProd);
                    }
                }
                newTipoEntidad.Costos = new List<Models.Costo>();
                if (tE.Costos != null)
                {
                    foreach (Shared.Entities.Costo c in tE.Costos)
                    {
                        Models.Costo costo = new Models.Costo();
                        costo.IdRecurso = c.IdRecurso;
                        costo.Value = c.Valor;
                        newTipoEntidad.Costos.Add(costo);
                    }
                }
                ret.TipoEdificios.Add(newTipoEntidad);
            }

            //Datos TipoUnidades
            ret.TipoUnidades = new List<Models.TipoEntidadModel>();

            foreach (var tU in juego.TipoUnidades)
            {
                Models.TipoEntidadModel newTipoEntidad = new Models.TipoEntidadModel();

                newTipoEntidad.IdJuego = tU.IdJuego;
                newTipoEntidad.Id = tU.Id;
                newTipoEntidad.Nombre = tU.Nombre;
                newTipoEntidad.Vida = tU.Vida;
                newTipoEntidad.Defensa = tU.Defensa;
                newTipoEntidad.Imagen = tU.Imagen;
                newTipoEntidad.Ataque = tU.Ataque;
                newTipoEntidad.TiempoConstruccion = tU.TiempoConstruccion;

                newTipoEntidad.Costos = new List<Models.Costo>();
                if (tU.Costos != null)
                {
                    foreach (Shared.Entities.Costo c in tU.Costos)
                    {
                        Models.Costo costo = new Models.Costo();
                        costo.IdRecurso = c.IdRecurso;
                        costo.Value = c.Valor;
                        newTipoEntidad.Costos.Add(costo);
                    }
                }

                ret.TipoUnidades.Add(newTipoEntidad);
            }
            /*
            //Datos DataActual
            ret.DataJugador.EstadoRecursos = new Dictionary<int, Models.EstadoRecursoModel>();
            foreach(var eR in juego.DataJugador.EstadoRecursos)
            {
                Models.EstadoRecursoModel eRM = new Models.EstadoRecursoModel();
                eRM.Total = (int)eR.Value.Total;
                eRM.Produccion = eR.Value.Produccion;

                ret.DataJugador.EstadoRecursos.Add(eR.Key, eRM);
            }

            ret.DataJugador.EstadoTecnologias = new Dictionary<int, Models.EstadoDataModel>();
            foreach(var eT in juego.DataJugador.EstadoTecnologias)
            {
                Models.EstadoDataModel eDM = new Models.EstadoDataModel();
                eDM.Estado = eT.Value.Estado;
                eDM.Tiempo = (int)eT.Value.Faltante;
                eDM.Cantidad = eT.Value.Cantidad;

                ret.DataJugador.EstadoTecnologias.Add(eT.Key, eDM);
            }

            ret.DataJugador.EstadoUnidades = new Dictionary<int, Models.EstadoDataModel>();
            foreach (var eU in juego.DataJugador.EstadoUnidades)
            {
                Models.EstadoDataModel eDM = new Models.EstadoDataModel();
                eDM.Estado = eU.Value.Estado;
                eDM.Tiempo = (int)eU.Value.Faltante;
                eDM.Cantidad = eU.Value.Cantidad;

                ret.DataJugador.EstadoUnidades.Add(eU.Key, eDM);
            }
            ret.DataJugador.UltimaActualizacion = juego.DataJugador.UltimaActualizacion;
            */
            return Json(new { success = true, responseText = "Juego: ", ret = ret }, JsonRequestBehavior.AllowGet);
        }


    }

}