using System;
using System.Collections.Generic;
using System.Text;

namespace CapaEntidades
{
    public class Programaciones
    { 
        public int IdProgramacion { get; set; }
        public DateTime FechaProgramacion { get; set; } 
        public string Origen { get; set; }
        public string Destino { get; set; }
        public string MarcaBus { get; set; }
        public string ModeloBus { get; set; }
        public string placaBus { get; set; }
        public int IdConductor { get; set; } 
        public string Conductor { get; set; }
        public decimal PrecioPiso1 { get; set; }
        public decimal PrecioPiso2 { get; set; }
        public int Estado { get; set; }
    }
}
