using Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DataAccessLayer.Relacional
{
    public class DALEntidades : IDALEntidadesRO
    {
        

        public List<TipoEdificio> GetAllTipoEdificios()
        {
            using (var context = new Model.frontofficedbEntities())
            {
                IQueryable<Model.TipoEntidad> listaTipoEntidades = context.TipoEntidades.OfType<DataAccessLayer.Relacional.Model.TipoEdificio>();
                   // from row in context.TipoEntidades.OfType<TipoEdificio>
                   // join ed in context.TipoEdificios on row.Id equals ed.Id
                   // select row;

                List<TipoEdificio> ret = new List<TipoEdificio>();
                try
                {
                    foreach (var tipoEntidad in listaTipoEntidades)
                    {
                        TipoEdificio result = new TipoEdificio() { };

                        result.Id = tipoEntidad.Id;
                        result.Nombre = tipoEntidad.Nombre;
                        result.Vida = tipoEntidad.Vida;
                        result.Defensa = tipoEntidad.Defensa;
                        result.Imagen = tipoEntidad.Imagen;
                        result.Ataque = tipoEntidad.Ataque;
                        result.TiempoConstruccion = tipoEntidad.TiempoConstruccion;

                        ret.Add(result);
                    }
                }
                catch (Exception e)
                {
                    var a = e;
                }

                return ret;
            };

        }

        public List<TipoUnidad> GetAllTipoUnidades()
        {
            using (var context = new Model.frontofficedbEntities())
            {
                IQueryable<Model.TipoEntidad> listaTipoEntidades = context.TipoEntidades.OfType<DataAccessLayer.Relacional.Model.TipoUnidad>();
                    //from row in context.TipoEntidades
                    //join ed in context.TipoUnidades on row.Id equals ed.Id
                    //select row;

                List<TipoUnidad> ret = new List<TipoUnidad>();
                try
                {
                    foreach (var tipoEntidad in listaTipoEntidades)
                    {
                        TipoUnidad result = new TipoUnidad() { };

                        result.Id = tipoEntidad.Id;
                        result.Nombre = tipoEntidad.Nombre;
                        result.Vida = tipoEntidad.Vida;
                        result.Defensa = tipoEntidad.Defensa;
                        result.Imagen = tipoEntidad.Imagen;
                        result.Ataque = tipoEntidad.Ataque;
                        result.TiempoConstruccion = tipoEntidad.TiempoConstruccion;

                        
                        ret.Add(result);
                    }
                }
                catch (Exception e)
                {
                    var a = e;
                }

                return ret;
            }
        }
        
    }
}
