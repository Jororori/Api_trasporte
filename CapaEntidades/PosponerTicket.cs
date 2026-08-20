using System;
using System.Collections.Generic;
using System.Text;

namespace CapaEntidades
{
    public class PosponerTicket
    {
        public string NumeroBoleto { get; set; }
        public int IdProgramacion { get; set; }
        public string? Telefono { get; set; }
        public string? MedioDePago { get; set; }
        public string? Tarjeta { get; set; }
        public List<Pasajeros> Pasajeros { get; set; }
        public int PuntoEmbarque { get; set; }
        public int IdDestino { get; set; }
        public int IdOrigen { get; set; }
    }
        
    public class Pasajeros
    {
        public int Asiento { get; set; }
        public string Pasajero { get; set; }
        public int TipoDocumento { get; set; } 
        public string NroDocumento { get; set; }
        public DateTime? FechaNacimiento { get; set; }
        public string? Sexo { get; set; }
        public string SerieBoleto { get; set; }
        public string? Ruc { get; set; }
        public string? RazonSocial { get; set; }
        public string? Direccion { get; set; }
        public decimal MontoAnteriorP { get; set; }
        public int IdDetalleProgAnterior { get; set; }
        public int IdDocVenta { get; set; }
    }
}
