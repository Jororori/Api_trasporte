using System;
using System.Collections.Generic;
using System.Text;

namespace CapaEntidades
{
    public class Reprogramacion
    {
        public string NumeroBoleto { get; set; }
        public int IdProgramacion { get; set; }
        public string Telefono { get; set; }
        public string Email { get; set; }

        public List<PasajerosReprog> Pasajeros { get; set; }
    }

    public class PasajerosReprog
    {
        public int Asiento { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public int TipoDocumento { get; set; }
        public string NroDocumento { get; set; }
        public DateTime? FechaNacimiento { get; set; }
        public string Sexo { get; set; }
        public string SerieBoleto { get; set; }

    }
}
