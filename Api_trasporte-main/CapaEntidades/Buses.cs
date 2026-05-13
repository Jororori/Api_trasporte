using System;
using System.Collections.Generic;
using System.Text;

namespace CapaEntidades
{
    public class Buses
    {
        public int IdBus { get; set; }
        public string Modelo { get; set; }
        public string Placa { get; set; } 
        public int Tipo { get; set; }

        public int Capacidad { get; set; }
        public List<Asientos> Asiento { get; set; }
    }
}
