using System;
using System.Collections.Generic;
using System.Text;

namespace CapaEntidades
{
    public class DetalleProgramacion
    {
        public int IdDetalleProgramacion { get; set; } 
        public int IdOrigen { get; set; }
        public string PuntoOrigen { get; set; }
        public int IdDestino { get; set; }
        public string PuntoDestino { get; set; }
        public string ValorAsiento { get; set; }
        public int NumeroFila { get; set; }
        public int NumeroColumna { get; set; }
        public int NumeroPiso { get; set; }
        public int Estado { get; set; }
        public decimal PrecioPiso1 { get; set; }
        public decimal PrecioPiso2 { get; set; }
        public List<PuntoIntemedio> PuntosIntermedios { get; set; } = new();

    }
}
