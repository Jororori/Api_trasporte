using System;
using System.Collections.Generic;
using System.Text;

namespace CapaEntidades
{
    public class DocumentoElectronicoResponse
    {
        public int Asiento { get; set; }
        public string Numero { get; set; }
        public byte[] Pdf { get; set; }
    }
}
