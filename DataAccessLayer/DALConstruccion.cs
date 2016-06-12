﻿using System;
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
        private IMongoCollection<TableroConstruccion> collection;
        private int idJuego;

        public DALConstruccion(int idJuego)
        {
            this.idJuego = idJuego;
            database = client.GetDatabase(idJuego.ToString());
            collection = database.GetCollection<TableroConstruccion>("construccion");
        }

        public void InicializarConstruccion(string idUsuario)
        {
            TableroConstruccion tableroConstruccionInicial = new TableroConstruccion(idUsuario);
            collection.InsertOne(tableroConstruccionInicial);
        }

        public TableroConstruccion getTableroConstruccion(string idUsuario)
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

        public void AddInfoCelda(string idUsuario, InfoCelda infoCelda)
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

        public void DeleteInfoCelda(string idUsuario, InfoCelda infoCelda)
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

        public void Refresh(string idUsuario, Shared.Entities.Juego juego)
        {
            TableroConstruccion tableroConstruccion = getTableroConstruccion(idUsuario);
            if (tableroConstruccion == null)
                throw new DALConstruccionException("ERROR:No existe el usuario " + idUsuario);
            DateTime now = new DateTime();
            bool necesitaUpdate = false;
            var dicEntidades = juego.TipoEntidad.ToDictionary(e => e.Id);
            foreach (var infoCelda in tableroConstruccion.lstInfoCelda)
            {
                if (!infoCelda.terminado)
                {
                    int segundosConstruyendo = now.Subtract(infoCelda.FechaCreacion).Seconds;
                    //foreach (Shared.Entities.TipoEntidad tipoEntidad in juego.tipo_entidad)
                    if (!dicEntidades.ContainsKey(infoCelda.Id))
                    {
                        throw new Exception(infoCelda.Id + " no es un id de entidad en la coleccion de entidades del juego!!!");
                    }
                    var tipoEntidad = dicEntidades[infoCelda.Id];

                    if (segundosConstruyendo >= tipoEntidad.TiempoConstruccion)
                    {
                        infoCelda.terminado = true;
                        necesitaUpdate = true;
                    }
                }
            }
            if (necesitaUpdate)
                collection.ReplaceOne(tablero => tablero.idUsuario == tableroConstruccion.idUsuario, tableroConstruccion);
        }

        
        //SERVICIOS
        public Shared.Entities.ValidarConstruccion ConstruirEdificio(int IdEdificio)
        {
            return null;
        }

        public void PersistirEdificio(Shared.Entities.CEInputData ceid)
        {

        }

        public Shared.Entities.ValidarUnidad EntrenarUnidad(int IdUnidad)
        {
            return null;
        }

        public void PersistirUnidades(Shared.Entities.EUInputData ceid)
        {

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
