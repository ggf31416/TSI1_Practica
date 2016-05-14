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
        public Nullable<int> PosX { get; set; }
        public Nullable<int> PosY { get; set; }
    }
}
