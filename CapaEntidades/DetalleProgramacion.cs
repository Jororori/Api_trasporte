using System;
using System.Collections.Generic;
using System.Text;

namespace CapaEntidades
{
    public class DetalleProgramacion
    {
        public int IdDetalleProgramacion { get; set; } 
        public string ValorAsiento { get; set; }
        public int NumeroFila { get; set; }
        public int NumeroColumna { get; set; }
        public int NumeroPiso { get; set; }
        public int Estado { get; set; }
        public decimal PrecioPiso1 { get; set; }
        public decimal PrecioPiso2 { get; set; }

    }
}
