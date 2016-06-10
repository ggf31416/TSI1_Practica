using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Core;
using MongoDB.Driver.Linq;
using MongoDB.Bson.Serialization;
using DataAccessLayer.Entities;
using DataAccessLayer.Exceptions;

namespace DataAccessLayer
{
    public class DALConstruccion : IDALConstruccion
    {
        const string connectionstring = "mongodb://40.84.2.155";
        private static IMongoClient client = new MongoClient(connectionstring);
        private IMongoDatabase database;
        private IMongoCollection<Shared.Entities.Juego> collection;
        private string nombreJuego = "AOE";
        private string idUsuario = "10209545762984761";

        public DALConstruccion(){
            database = client.GetDatabase(nombreJuego);
            collection = database.GetCollection<Shared.Entities.Juego>("juego_usuario");
        }

        public DALConstruccion(string nombreJuego, string idUsuario)
        {
            this.nombreJuego = nombreJuego;
            this.idUsuario = idUsuario;
            database = client.GetDatabase(nombreJuego);
            collection = database.GetCollection<Shared.Entities.Juego>("juego_usuario");
        }

        public void InicializarConstruccion(string idUsuario, string nombreJuego)
        {
            IDALJuego iDALJuego = new DALJuego();
            Shared.Entities.Juego juego = iDALJuego.GetJuego(this.nombreJuego);
            juego.IdJugador = idUsuario;
            juego.DataJugador = new Shared.Entities.DataActual();
            juego.DataJugador.UltimaActualizacion = DateTime.Now;
            juego.DataJugador.EstadoRecursos = new Dictionary<string, Shared.Entities.EstadoRecurso>();
            foreach (var recurso in juego.TipoRecurso)
            {
                Shared.Entities.EstadoRecurso EstadoRecurso = new Shared.Entities.EstadoRecurso();
                EstadoRecurso.Total = 99999;
                EstadoRecurso.Produccion = 12345;
                juego.DataJugador.EstadoRecursos.Add(recurso.Id.ToString(), EstadoRecurso);
            }
            collection.InsertOne(juego);
        }

        //public TableroConstruccion getTableroConstruccion(string idUsuario)
        //{
        //    var query = from tablero in collection.AsQueryable<TableroConstruccion>()
        //                where tablero.idUsuario == idUsuario
        //                select tablero;
        //    if (query.Count() > 0)
        //        return query.First();
        //    else
        //        throw new DALConstruccionException("ERROR:No existe el usuario " + idUsuario);
        //}

        //private InfoCelda getInfoCelda(TableroConstruccion tableroConstruccion, int posX, int posY) {
        //    foreach (var infoCelda in tableroConstruccion.lstInfoCelda)
        //    {
        //        if (infoCelda.PosX == posX && infoCelda.PosY == posY)
        //            return infoCelda;
        //    }
        //    return null;
        //}

        //public void AddInfoCelda(string idUsuario, InfoCelda infoCelda)
        //{
        //    TableroConstruccion tableroConstruccion = null;
        //    try
        //    {
        //        tableroConstruccion = getTableroConstruccion(idUsuario);
        //    } catch (DALConstruccionException ex)
        //    {
        //        throw new DALConstruccionException("ERROR:Imposible insertar edificio " + infoCelda.Id +
        //            "en la posicion(" + infoCelda.PosX + "," + infoCelda.PosY +
        //            "). No existe el usuario " + idUsuario);
        //    }
        //    InfoCelda celda = getInfoCelda(tableroConstruccion, infoCelda.PosX, infoCelda.PosY);
        //    if (celda != null)
        //        throw new DALConstruccionException("ERROR:Imposible insertar edificio " + infoCelda.Id + 
        //            "en la posicion(" + infoCelda.PosX + "," + infoCelda.PosY + 
        //            "). Ya existe un edificio de id " + celda.Id + " en dicho lugar");
        //    tableroConstruccion.lstInfoCelda.Add(infoCelda);
        //    collection.ReplaceOne(tablero => tablero.idUsuario == tableroConstruccion.idUsuario, tableroConstruccion);
        //}

        //public void DeleteInfoCelda(string idUsuario, InfoCelda infoCelda)
        //{
        //    TableroConstruccion tableroConstruccion = getTableroConstruccion(idUsuario);
        //    if (tableroConstruccion == null)
        //        throw new DALConstruccionException("ERROR:Imposible insertar edificio " + infoCelda.Id +
        //            "en la posicion(" + infoCelda.PosX + "," + infoCelda.PosY +
        //            "). No existe el usuario " + idUsuario);
        //    InfoCelda celda = getInfoCelda(tableroConstruccion, infoCelda.PosX, infoCelda.PosY);
        //    if (celda != null)
        //        throw new DALConstruccionException("ERROR:Imposible insertar edificio " + infoCelda.Id +
        //            "en la posicion(" + infoCelda.PosX + "," + infoCelda.PosY +
        //            "). Ya existe un edificio de id " + celda.Id + " en dicho lugar");
        //    tableroConstruccion.lstInfoCelda.Remove(infoCelda);
        //    collection.ReplaceOne(tablero => tablero.idUsuario == tableroConstruccion.idUsuario, tableroConstruccion);
        //}

        //public void Refresh(string idUsuario, Shared.Entities.Juego juego)
        //{
        //    TableroConstruccion tableroConstruccion = getTableroConstruccion(idUsuario);
        //    if (tableroConstruccion == null)
        //        throw new DALConstruccionException("ERROR:No existe el usuario " + idUsuario);
        //    DateTime now = new DateTime();
        //    bool necesitaUpdate = false;
        //    var dicEntidades = juego.TipoEntidad.ToDictionary(e => e.Id);
        //    foreach (var infoCelda in tableroConstruccion.lstInfoCelda)
        //    {
        //        if (!infoCelda.terminado)
        //        {
        //            int segundosConstruyendo = now.Subtract(infoCelda.FechaCreacion).Seconds;
        //            //foreach (Shared.Entities.TipoEntidad tipoEntidad in juego.tipo_entidad)
        //            if (!dicEntidades.ContainsKey(infoCelda.Id))
        //            {
        //                throw new Exception(infoCelda.Id + " no es un id de entidad en la coleccion de entidades del juego!!!");
        //            }
        //            var tipoEntidad = dicEntidades[infoCelda.Id];

        //            if (segundosConstruyendo >= tipoEntidad.TiempoConstruccion)
        //            {
        //                infoCelda.terminado = true;
        //                necesitaUpdate = true;
        //            }
        //        }
        //    }
        //    if (necesitaUpdate)
        //        collection.ReplaceOne(tablero => tablero.idUsuario == tableroConstruccion.idUsuario, tableroConstruccion);
        //}


        //SERVICIOS
        public Shared.Entities.ValidarConstruccion ConstruirEdificio(int IdEdificio, string Tenant, string NombreJugador)
        {
            var query = from juego in collection.AsQueryable<Shared.Entities.Juego>()
                        where juego.IdJugador == idUsuario
                        select juego;

            if (query.Count() > 0)
            {
                Shared.Entities.Juego juego = query.First();
                Shared.Entities.ValidarConstruccion validarConstruccion = new Shared.Entities.ValidarConstruccion();
                validarConstruccion.Tablero = juego.Tablero;
                foreach(var tipoEdi in juego.TipoEdificios)
                {
                    if (tipoEdi.Id == IdEdificio)
                    {
                        validarConstruccion.TipoEdificio = tipoEdi;
                        break;
                    }
                }
                validarConstruccion.Recursos = new Dictionary<string, int>();
                foreach(var recur in juego.DataJugador.EstadoRecursos)
                {
                    validarConstruccion.Recursos.Add(recur.Key, (int)recur.Value.Total);
                }
                return validarConstruccion;
            }
            else
            {
                return null;
            }
        }

        public bool PersistirEdificio(Shared.Entities.CEInputData ceid, string Tenant, string NombreJugador)
        {
            var query = from juego in collection.AsQueryable<Shared.Entities.Juego>()
                        where juego.IdJugador == idUsuario
                        select juego;

            if (query.Count() > 0)
            {
                Shared.Entities.Juego juego = query.First();
                Shared.Entities.TipoEdificio TipoEdificio = new Shared.Entities.TipoEdificio();
                foreach (var tipoEdi in juego.TipoEdificios)
                {
                    if (tipoEdi.Id == ceid.IdTipoEdificio)
                    {
                        TipoEdificio = tipoEdi;
                        break;
                    }
                }
                Shared.Entities.TableroCelda tableroCelda = new Shared.Entities.TableroCelda();
                tableroCelda.IdTipoEdificio = ceid.IdTipoEdificio;
                tableroCelda.PosColumna = ceid.PosColumna;
                tableroCelda.PosFila = ceid.PosFila;
                tableroCelda.Estado = new Shared.Entities.EstadoData();
                tableroCelda.Estado.Estado = Shared.Entities.EstadoData.EstadoEnum.C;
                tableroCelda.Estado.Fin = DateTime.Now.AddSeconds((int)TipoEdificio.TiempoConstruccion);
                juego.Tablero.Celdas.Add(tableroCelda);
                Shared.Entities.EstadoRecurso EstRec = new Shared.Entities.EstadoRecurso();
                foreach (var costo in TipoEdificio.Costos)
                {
                    juego.DataJugador.EstadoRecursos.TryGetValue(costo.IdRecurso.ToString(), out EstRec);
                    EstRec.Total = EstRec.Total - costo.Valor;
                    juego.DataJugador.EstadoRecursos[costo.IdRecurso.ToString()] = EstRec;
                }
                var result = collection.ReplaceOne(j => j.IdJugador == juego.IdJugador, juego, new UpdateOptions { IsUpsert = true });
                return result.ModifiedCount == 1;
            }
            else
            {
                return false;
            }
        }

        public Shared.Entities.ValidarUnidad EntrenarUnidad(int IdUnidad, string Tenant, string NombreJugador)
        {
            var query = from juego in collection.AsQueryable<Shared.Entities.Juego>()
                        where juego.IdJugador == idUsuario
                        select juego;

            if (query.Count() > 0)
            {
                Shared.Entities.Juego juego = query.First();
                Shared.Entities.ValidarUnidad validarUnidad = new Shared.Entities.ValidarUnidad();
                foreach (var tipoUni in juego.TipoUnidades)
                {
                    if (tipoUni.Id == IdUnidad)
                    {
                        validarUnidad.TipoUnidad = tipoUni;
                        break;
                    }
                }
                validarUnidad.Recursos = new Dictionary<string, int>();
                foreach (var recur in juego.DataJugador.EstadoRecursos)
                {
                    validarUnidad.Recursos.Add(recur.Key, (int)recur.Value.Total);
                }
                return validarUnidad;
            }
            else
            {
                return null;
            }
        }

        public bool PersistirUnidades(Shared.Entities.EUInputData euid, string Tenant, string NombreJugador)
        {
            var query = from juego in collection.AsQueryable<Shared.Entities.Juego>()
                        where juego.IdJugador == idUsuario
                        select juego;

            if (query.Count() > 0)
            {
                Shared.Entities.Juego juego = query.First();
                Shared.Entities.TipoUnidad TipoUnidad = new Shared.Entities.TipoUnidad();
                foreach (var tipoUni in juego.TipoUnidades)
                {
                    if (tipoUni.Id == euid.IdTipoUnidad)
                    {
                        TipoUnidad = tipoUni;
                        break;
                    }
                }
                Shared.Entities.EstadoData EstadoData = new Shared.Entities.EstadoData();
                EstadoData.Cantidad = euid.Cantidad;
                EstadoData.Estado = Shared.Entities.EstadoData.EstadoEnum.C;
                EstadoData.Fin = DateTime.Now.AddHours((int)TipoUnidad.TiempoConstruccion);
                juego.DataJugador.EstadoUnidades = new Dictionary<string, Shared.Entities.EstadoData>();
                juego.DataJugador.EstadoUnidades.Add(TipoUnidad.Id.ToString(), EstadoData);
                Shared.Entities.EstadoRecurso EstRec = new Shared.Entities.EstadoRecurso();
                foreach (var costo in TipoUnidad.Costos)
                {
                    juego.DataJugador.EstadoRecursos.TryGetValue(costo.IdRecurso.ToString(), out EstRec);
                    EstRec.Total = EstRec.Total - costo.Valor;

                    juego.DataJugador.EstadoRecursos[costo.IdRecurso.ToString()] = EstRec;
                }
                var result = collection.ReplaceOne(j => j.IdJugador == juego.IdJugador, juego, new UpdateOptions { IsUpsert = true });
                return result.ModifiedCount == 1;
            }
            else
            {
                return false;
            }
        }

        //public void AddPrueba(Prueba prueba)
        //{
        //    collection.InsertOne(prueba);
        //}

        //public void DeletePrueba(String nombre)
        //{
        //    collection.DeleteOne(prueba => prueba.Nombre == nombre);
        //}

        //public void UpdatePrueba(Prueba pruebaUpdate)
        //{
        //    collection.ReplaceOne(prueba => prueba.Nombre == pruebaUpdate.Nombre, pruebaUpdate);
        //}


        //public List<Prueba> GetAllPrueba()
        //{
        //    var collection = _database.GetCollection<BsonDocument>("construccion");
        //    List<Prueba> result = new List<Prueba>();
        //    List<BsonDocument> mongoResult = collection.AsQueryable<BsonDocument>().ToList();
        //    foreach (var prueba in mongoResult)
        //    {
        //        try
        //        {
        //            result.Add(BsonSerializer.Deserialize<Prueba>(prueba));
        //        }
        //        catch (Exception ex)
        //        {
        //            //ignore
        //        }
        //    }
        //    return result;
        //}

        //public Prueba GetPrueba(String nombre)
        //{
        //    var prueba = from p in collection.AsQueryable<Prueba>()
        //                             where p.Nombre == nombre
        //                             select p;
        //    return prueba.First();
        //}

    }
}
