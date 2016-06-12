using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using MongoDB.Bson.Serialization.Attributes;

namespace Shared.Entities
{
    [DataContract]
    public class ListasEntidades
    {
        [DataMember]
        public List<TipoEdificio> TipoEdificios { get; set; }
        [DataMember]
        public List<TipoUnidad> TipoUnidades { get; set; }
    }
}
