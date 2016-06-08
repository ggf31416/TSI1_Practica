using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Entities
{
    public class TableroConstruccion
    {

        [BsonId]
        public int idUsuario { get; set; }
        public List<InfoCelda> lstInfoCelda = new List<InfoCelda>();

        public TableroConstruccion(int idUsuario)
        {
            this.idUsuario = idUsuario;
        }

    }
}
