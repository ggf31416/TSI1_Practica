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

            retJuegoModel.Id = (int)juego.Id;
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

        [HttpGet]
        public ActionResult GetEntidadesActualizadas(string tenant)
        {
            try
            {
                string idJugador = Request.Cookies["clienteId"].Value;
                ServiceTableroClient client = new ServiceTableroClient();

                ListasEntidades listasEntidades = client.GetEntidadesActualizadas(tenant, idJugador);

                Models.ListasEntidadesModel ret = new Models.ListasEntidadesModel();

                //Datos TipoEdificios
                ret.TipoEdificios = new List<Models.TipoEntidadModel>();

                foreach (var tE in listasEntidades.TipoEdificios)
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
                        foreach (var c in tE.RecursosAsociados)
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
                        foreach (var c in tE.Costos)
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

                foreach (var tU in listasEntidades.TipoUnidades)
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
                        foreach (var c in tU.Costos)
                        {
                            Models.Costo costo = new Models.Costo();
                            costo.IdRecurso = c.IdRecurso;
                            costo.Value = c.Valor;
                            newTipoEntidad.Costos.Add(costo);
                        }
                    }

                    ret.TipoUnidades.Add(newTipoEntidad);
                }

                return Json(new { success = true, ret = ret }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult GetJuegoUsuario(string tenant)
        {
            try
            {
                
                ServiceTableroClient client = new ServiceTableroClient();

                Models.AllDataJuegoModel ret = new Models.AllDataJuegoModel();

                Juego juego = client.GetJuegoUsuario(tenant, Request.Cookies["clienteId"].Value);

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

                    Models.EstadoDataModel edm = new Models.EstadoDataModel();
                    edm.Estado = tc.Estado.Estado;
                    edm.Id = tc.Estado.Id;
                    edm.Cantidad = tc.Estado.Cantidad;
                    edm.Fin = tc.Estado.Fin;
                    edm.Faltante = tc.Estado.Faltante;

                    celda.EstadoData = edm;
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

                //Datos DataActual
                ret.DataJugador = new Models.DataActualModel();
                ret.DataJugador.Clan = juego.DataJugador.Clan;
                ret.DataJugador.EstadoRecursos = new Dictionary<string, Models.EstadoRecursoModel>();
                foreach (var eR in juego.DataJugador.EstadoRecursos)
                {
                    Models.EstadoRecursoModel eRM = new Models.EstadoRecursoModel();
                    //eRM.Id = int.Parse(eR.Key.Split('#')[0]);
                    eRM.Id = int.Parse(eR.Key);// TODO: Cambiar por la linea de arrib acuando cambie la clave
                    eRM.Total = (int)eR.Value.Total;
                    eRM.Produccion = eR.Value.Produccion;

                    ret.DataJugador.EstadoRecursos.Add(eR.Key, eRM);
                }

                ret.DataJugador.EstadoTecnologias = new Dictionary<string, Models.EstadoDataModel>();
                foreach (var eT in juego.DataJugador.EstadoTecnologias)
                {
                    Models.EstadoDataModel eDM = new Models.EstadoDataModel();
                    eDM.Id = int.Parse(eT.Key);// TODO: Modificar cuando cambien las claves
                    eDM.Estado = eT.Value.Estado;
                    eDM.Fin = eT.Value.Fin;
                    eDM.Faltante = eT.Value.Faltante;
                    eDM.Cantidad = eT.Value.Cantidad;

                    ret.DataJugador.EstadoTecnologias.Add(eT.Key, eDM);
                }

                ret.DataJugador.EstadoUnidades = new Dictionary<string, Models.EstadoDataModel>();
                foreach (var eU in juego.DataJugador.EstadoUnidades)
                {
                    if (eU.Value.Estado == EstadoData.EstadoEnum.A)
                    {
                        Models.EstadoDataModel eDM = new Models.EstadoDataModel();
                        eDM.Id = eU.Value.Id;// TODO: Modificar cuando cambien las claves
                        eDM.Estado = eU.Value.Estado;
                        eDM.Fin = eU.Value.Fin;
                        eDM.Faltante = eU.Value.Faltante;
                        eDM.Cantidad = eU.Value.Cantidad;

                        ret.DataJugador.EstadoUnidades.Add(eDM.Id.ToString(), eDM);
                    }
                   
                }
                ret.DataJugador.UltimaActualizacion = juego.DataJugador.UltimaActualizacion;

                return Json(new { success = true, responseText = "Juego: ", ret = ret }, JsonRequestBehavior.AllowGet);
            }
            catch(Exception e)
            {

                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }

        //SOCIALES
        [HttpGet]
        public ActionResult GetJugadoresAtacables(string tenant)
        {
            try
            {
                ServiceTableroClient client = new ServiceTableroClient();

                List<ClienteJuego> jugadores = client.GetJugadoresAtacables(tenant, Request.Cookies["clienteId"].Value).ToList();

                List<Models.ClienteJuego> ret = new List<Models.ClienteJuego>();

                foreach(var cJ in jugadores)
                {
                    if(cJ.clienteId != Request.Cookies["clienteId"].Value)
                    {
                        Models.ClienteJuego mCJ = new Models.ClienteJuego();
                        mCJ.idJuego = cJ.idJuego;
                        mCJ.nombre = cJ.nombre;
                        mCJ.apellido = cJ.apellido;
                        mCJ.token = cJ.token;
                        mCJ.clienteId = cJ.clienteId;
                        mCJ.username = cJ.username;
                        ret.Add(mCJ);
                    }
                }

                return Json(new { success = true, ret = ret }, JsonRequestBehavior.AllowGet);
            }
            catch(Exception e)
            {
                return Json(new { success = false}, JsonRequestBehavior.AllowGet);
            }
        }

        //CLANES
        [HttpPost]
        public ActionResult CrearClan(string tenant, Models.NombreClanModel NombreClanModel)
        {
            try
            {
                ServiceTableroClient client = new ServiceTableroClient();

                client.CrearClan(NombreClanModel.NombreClan, tenant, Request.Cookies["clienteId"].Value);

                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult AbandonarClan(string tenant)
        {
            try
            {
                ServiceTableroClient client = new ServiceTableroClient();

                bool ret = client.AbandonarClan(tenant, Request.Cookies["clienteId"].Value);

                return Json(new { success = true, ret = ret }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult GetJugadoresSinClan(string tenant)
        {
            try
            {
                ServiceTableroClient client = new ServiceTableroClient();

                List<Models.ClienteJuego> ret = new List<Models.ClienteJuego>();

                foreach (var cj in client.GetJugadoresSinClan(tenant, Request.Cookies["clienteId"].Value))
                {
                    Models.ClienteJuego newCJ = new Models.ClienteJuego();
                    newCJ.clienteId = cj.clienteId;
                    newCJ.idJuego = cj.idJuego;
                    newCJ.nombre = cj.nombre;
                    newCJ.apellido = cj.apellido;
                    newCJ.clan = cj.clan;
                    newCJ.username = cj.username;
                    newCJ.token = cj.token;
                    ret.Add(newCJ);
                }

                return Json(new { success = true, ret = ret }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult AgregarJugadorClan(string tenant, Models.ClienteJuego JugadorModel)
        {
            try
            {
                ServiceTableroClient client = new ServiceTableroClient();

                ClienteJuego cj = new ClienteJuego();
                cj.clienteId = JugadorModel.clienteId;
                cj.idJuego = JugadorModel.idJuego;
                cj.nombre = JugadorModel.nombre;
                cj.apellido = JugadorModel.apellido;
                cj.clan = JugadorModel.clan;
                cj.username = JugadorModel.username;
                cj.token = JugadorModel.token;
                bool ret = client.AgregarJugadorClan(cj,tenant, Request.Cookies["clienteId"].Value);

                return Json(new { success = true, ret = ret}, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { success = false}, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult GetJugadoresEnElClan(string tenant)
        {
            try
            {
                ServiceTableroClient client = new ServiceTableroClient();

                List<Models.ClienteJuego> ret = new List<Models.ClienteJuego>();

                foreach (var cj in client.GetJugadoresEnElClan(tenant, Request.Cookies["clienteId"].Value))
                {
                    Models.ClienteJuego newCJ = new Models.ClienteJuego();
                    newCJ.clienteId = cj.clienteId;
                    newCJ.idJuego = cj.idJuego;
                    newCJ.nombre = cj.nombre;
                    newCJ.apellido = cj.apellido;
                    newCJ.clan = cj.clan;
                    newCJ.username = cj.username;
                    newCJ.token = cj.token;
                    ret.Add(newCJ);
                }

                return Json(new { success = true, ret = ret }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult SoyAdministrador(string tenant)
        {
            try
            {
                ServiceTableroClient client = new ServiceTableroClient();

                bool ret = client.SoyAdministrador(tenant, Request.Cookies["clienteId"].Value);

                return Json(new { success = true, ret = ret}, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { success = false}, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult EnviarRecursos(string tenant, Models.TributoModel tributoModel)
        {
            try
            {
                ServiceTableroClient client = new ServiceTableroClient();

                List<RecursoAsociado> recursosAsociados = new List<RecursoAsociado>();
                foreach(var ram in tributoModel.Valores)
                {
                    RecursoAsociado ra = new RecursoAsociado();
                    ra.IdRecurso = ram.IdRecurso;
                    ra.Valor = ram.Value;
                    recursosAsociados.Add(ra);
                }
                int ret = client.EnviarRecursos(recursosAsociados.ToArray(), tributoModel.IdJugadorDestino, tenant, Request.Cookies["clienteId"].Value);

                return Json(new { success = true, ret = ret}, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }

    }

}