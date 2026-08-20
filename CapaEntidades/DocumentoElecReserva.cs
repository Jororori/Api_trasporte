using System;
using System.Collections.Generic;
using System.Text;

namespace CapaEntidades
{
    public class DocumentoElecReserva
    {
        public int Asiento { get; set; }
        public string Numero { get; set; }
        public byte[] Pdf { get; set; }
        public decimal NuevoMonto { get; set; }
        public decimal MontoExtra { get; set; }
    }
}
