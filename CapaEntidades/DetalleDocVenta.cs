using System;
using System.Collections.Generic;
using System.Text;

namespace CapaEntidades
{
    public class DetalleDocVenta
    {
        public int IdProducto { get; set; }
        public string Codigo { get; set; }
        public string Descripcion { get; set; }
        public string Medida { get; set; }
        public string Precio { get; set; }
        public string Descuento { get; set; }
        public string Moneda { get; set; }
        public string Equivalencia { get; set; }
        public string  NombreCompleto { get; set; }
        public string TipoProducto { get; set; }
        public int Exonerado { get; set; }
        public string LogoProd { get; set; }
        public string Peso { get; set; }
        public int IdMedida { get; set; }
        public int NroBolsas { get; set; } 

    }
}
