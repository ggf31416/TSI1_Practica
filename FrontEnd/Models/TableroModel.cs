using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FrontEnd.Models
{
    public class TableroModel
    {
        public int Id { get; set; }
        public int IdJuego { get; set; }
        public string ImagenTerreno { get; set; }
        public string ImagenFondo { get; set; }
        public List<ColumnaModel> Columnas { get; set; }
    }
}