using System;
using System.Collections.Generic;
using System.Text;

namespace CapaEntidades
{
    public class Rutas
    {
        public int IdRuta { get; set; }
        public string Origen { get; set; } 
        public string Destino { get; set; }
        public int Distancia { get; set; }
        public string Duracion { get; set; }
    }
}
