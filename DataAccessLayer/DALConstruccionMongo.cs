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
    public class DALConstruccionMongo : IDALConstruccion
    {
        const string connectionstring = "mongodb://40.84.2.155";
        private static IMongoClient client = new MongoClient(connectionstring);
        private IMongoDatabase database;
        private IMongoCollection<TableroConstruccion> collection;
        private string juego;

        public DALConstruccionMongo(string juego)
        {
            this.juego = juego;
            database = client.GetDatabase(juego);
            collection = database.GetCollection<TableroConstruccion>("construccion");
        }

        public void InicializarConstruccion(int idUsuario)
        {
            TableroConstruccion tableroConstruccionInicial = new TableroConstruccion(idUsuario);
            collection.InsertOne(tableroConstruccionInicial);
        }

        public TableroConstruccion getTableroConstruccion(int idUsuario)
        {
            var query = from tablero in collection.AsQueryable<TableroConstruccion>()
                        where tablero.idUsuario == idUsuario
                        select tablero;
            if (query.Count() > 0)
                return query.First();
            else
                throw new DALConstruccionException("ERROR:No existe el usuario " + idUsuario);
        }

        private InfoCelda getInfoCelda(TableroConstruccion tableroConstruccion, int posX, int posY) {
            foreach (var infoCelda in tableroConstruccion.lstInfoCelda)
            {
                if (infoCelda.PosX == posX && infoCelda.PosY == posY)
                    return infoCelda;
            }
            return null;
        }

        public void AddInfoCelda(int idUsuario, InfoCelda infoCelda)
        {
            TableroConstruccion tableroConstruccion = null;
            try
            {
                tableroConstruccion = getTableroConstruccion(idUsuario);
            } catch (DALConstruccionException ex)
            {
                throw new DALConstruccionException("ERROR:Imposible insertar edificio " + infoCelda.Id +
                    "en la posicion(" + infoCelda.PosX + "," + infoCelda.PosY +
                    "). No existe el usuario " + idUsuario);
            }
            InfoCelda celda = getInfoCelda(tableroConstruccion, infoCelda.PosX, infoCelda.PosY);
            if (celda != null)
                throw new DALConstruccionException("ERROR:Imposible insertar edificio " + infoCelda.Id + 
                    "en la posicion(" + infoCelda.PosX + "," + infoCelda.PosY + 
                    "). Ya existe un edificio de id " + celda.Id + " en dicho lugar");
            tableroConstruccion.lstInfoCelda.Add(infoCelda);
            collection.ReplaceOne(tablero => tablero.idUsuario == tableroConstruccion.idUsuario, tableroConstruccion);
        }

        public void DeleteInfoCelda(int idUsuario, InfoCelda infoCelda)
        {
            TableroConstruccion tableroConstruccion = getTableroConstruccion(idUsuario);
            if (tableroConstruccion == null)
                throw new DALConstruccionException("ERROR:Imposible insertar edificio " + infoCelda.Id +
                    "en la posicion(" + infoCelda.PosX + "," + infoCelda.PosY +
                    "). No existe el usuario " + idUsuario);
            InfoCelda celda = getInfoCelda(tableroConstruccion, infoCelda.PosX, infoCelda.PosY);
            if (celda != null)
                throw new DALConstruccionException("ERROR:Imposible insertar edificio " + infoCelda.Id +
                    "en la posicion(" + infoCelda.PosX + "," + infoCelda.PosY +
                    "). Ya existe un edificio de id " + celda.Id + " en dicho lugar");
            tableroConstruccion.lstInfoCelda.Remove(infoCelda);
            collection.ReplaceOne(tablero => tablero.idUsuario == tableroConstruccion.idUsuario, tableroConstruccion);
        }

        public void Refresh(int idUsuario, Shared.Entities.Juego juego)
        {
            TableroConstruccion tableroConstruccion = getTableroConstruccion(idUsuario);
            if (tableroConstruccion == null)
                throw new DALConstruccionException("ERROR:No existe el usuario " + idUsuario);
            DateTime now = new DateTime();
            bool necesitaUpdate = false;
            foreach (var infoCelda in tableroConstruccion.lstInfoCelda)
            {
                if (!infoCelda.terminado)
                {
                    int segundosConstruyendo = now.Subtract(infoCelda.FechaCreacion).Seconds;
                    //if (seungosConstruyendo >= juego.edificio.segundos)
                    //  infoCelda.terminado = true;
                    //  necesitaUpdate = true;
                }
            }
            if (necesitaUpdate)
                collection.ReplaceOne(tablero => tablero.idUsuario == tableroConstruccion.idUsuario, tableroConstruccion);
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
