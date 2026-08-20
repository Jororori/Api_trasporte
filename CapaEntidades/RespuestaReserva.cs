using System;
using System.Collections.Generic;
using System.Text;

namespace CapaEntidades
{
    public class RespuestaReserva
    {
        public string IdReseva { get; set; }
        public List<Data> DatosReserva { get; set; }


    }

    public class Data
    {
        public int Asiento { get; set; } 
        public decimal NuevoMonto { get; set; }
        public decimal MontoExtra { get; set; }
    }
}
