using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FrontEnd.Models
{
    public class AllDataJuegoModel
    {
        public JuegoModel DataJuego { get; set; }
        public List<TipoEntidadModel> TipoEdificios { get; set; }
        public List<TipoEntidadModel> TipoUnidades { get; set; }
        public List<TipoRecursoModel> TipoRecursos { get; set; }
        public List<TecnologiaModel> Tecnologias { get; set; }
        public TableroModel Tablero { get; set; }
        public DataActualModel DataJugador { get; set; }
    }
}