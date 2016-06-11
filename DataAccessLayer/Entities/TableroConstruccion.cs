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
        public string idUsuario { get; set; }
        public List<InfoCelda> lstInfoCelda = new List<InfoCelda>();

        public TableroConstruccion(string idUsuario)
        {
            this.idUsuario = idUsuario;
        }

    }
}
