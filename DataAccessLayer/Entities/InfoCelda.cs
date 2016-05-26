using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.Attributes;

namespace DataAccessLayer.Entities
{
    public class InfoCelda
    {
        public int Id { get; set; }
        public int PosX { get; set; }
        public int PosY { get; set; }
        public DateTime FechaCreacion { get; set; }
        public bool terminado { get; set; }
    }
}
