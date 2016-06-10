using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Shared.Entities
{
    [DataContract]
    public class Tablero
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public Nullable<int> CantFilas { get; set; }
        [DataMember]
        public Nullable<int> CantColumnas { get; set; }
        [DataMember]
        public string ImagenTerreno { get; set; }
        [DataMember]
        public string ImagenFondo { get; set; }
        [DataMember]
        public Nullable<int> IdJuego { get; set; }
        [DataMember]
        public List<TableroCelda> Celdas { get; set; }
    }
}
