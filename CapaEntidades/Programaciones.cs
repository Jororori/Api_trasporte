using System;
using System.Collections.Generic;
using System.Text;

namespace CapaEntidades
{
    public class Programaciones
    { 
        public int IdProgramacion { get; set; }
        public DateTime FechaProgramacion { get; set; } 
        public DateTime FechaLlegada { get; set; }
        public int IdOrigen { get; set; }
        public string Origen { get; set; }
        public string DireccionOrigen { get; set; }
        public List<ZonasEmbarque> PuntoEmbarque { get; set; } = new();
        public int IdDestino { get; set; }
        public string Destino { get; set; }
        public string DireccionDestino { get; set; }
        public string MarcaBus { get; set; }
        public string ModeloBus { get; set; }
        public string placaBus { get; set; }
        public int IdConductor { get; set; } 
        public string Conductor { get; set; }
        public decimal PrecioPiso1 { get; set; }
        public decimal PrecioPiso2 { get; set; }
        public int Estado { get; set; }
        public TimeSpan HoraSalida { get; set; }
        public TimeSpan HoraLlegada { get; set; }

        public List<PuntoIntemedio> PuntosIntermedios { get; set; } = new();
    }

    public class ZonasEmbarque
    {
        public int ID { get; set; }
        public string NombrePuntoIntermedio { get; set; }
        public string Direccion { get; set; }
        public TimeSpan HoraSalida { get; set; }
    }

}
