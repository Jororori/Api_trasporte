using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text;

namespace CapaEntidades
{
    public class Ciudades
    {
        public int IdCiudad { get; set; } 
        public string NombreCiudad { get; set; }
        public string Codigo { get; set; }

        public int IdUbigeo { get; set; }
    }
}
