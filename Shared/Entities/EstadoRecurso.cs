using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Shared.Entities
{
    [DataContract]
    public class EstadoRecurso
    {
        // tiene que ser un float o double porque sino donde se calcule con un dT no entero no da bien
        [DataMember]
        public float Total { get; set; }
        [DataMember]
        public int Produccion { get; set; }
    }
}
