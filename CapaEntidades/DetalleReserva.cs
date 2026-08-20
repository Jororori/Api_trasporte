using System;
using System.Collections.Generic;
using System.Text;

namespace CapaEntidades
{
    public class Boleto { 
        public int IdProgramacion { get; set; }
        public List<DetalleReserva> venta { get; set; }
    }

    public class DetalleReserva
    {
        public int IdDetalleProgramacion { get; set; }
        public int Asiento { get; set; }
        public int TipoDocumento { get; set; }

        public string? NroDocumento { get; set; }
        public string? Pasajero { get; set; }
        public string? Telefono { get; set; }
        public string? Ruc { get; set; }
        public string? RazonSocial { get; set; } 
        public string? Direccion { get; set; }

        public DateTime? FechaNacimiento { get; set; }

        public int Edad { get; set; }

        public string? Sexo { get; set; }

        public int IdAgenciaOrigen { get; set; }
        public int PuntoEmbarque { get; set; }

        public int IdAgenciaDestino { get; set; }

        public string? MedioDePago { get; set; }
        public string? Tarjeta { get; set; }

        public string? HoraSalida { get; set; }
        public string? Observacion { get; set; }
        public string? Menor { get; set; }
        public decimal Precio { get; set; }
        public string? PrecioLetra { get; set; }
        public int IncluidoIGV { get; set; }
        public string? PlacaBus { get; set; }

    }
}
