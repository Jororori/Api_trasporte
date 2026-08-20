using System;
using System.Collections.Generic;
using System.Text;

namespace CapaEntidades
{
    public class Encabezado
    {
        public string Type { get; set; }
        public Datosinvoice Invoice { get; set; }
    }

    public class Datosinvoice
    {
        public string Ruc_emisor { get; set; }/*usado*/ /*ruc del emisor */ /* preguntar sobre forma de pago (por mientras contado y efectivo)*/
        public string Dni_usuario { get; set; }/*usado*/ /*dni usuario en el sistema*/
        public string Pass { get; set; }/*usado*/ /*clave del usuario en el sistema*/
        public string Tipo { get; set; }/*usado*/ /* 1 factura 3 boleta 7 nota de crédito .....*/
        public string Serie { get; set; }/*usado*/  /*indicas la serie de la cual se va ha generar el correlativo (B001 o F001)*/ /* por el momento hay titketera? */
        public string Numero { get; set; } /* vacio */
        public string Ruc { get; set; } /* ruc o dni de la persona */ /*usado*/
        public string Tipo_de_documento { get; set; }/*usado*/ /*1 dni, 6 ruc, 0 otros*/
        public string Razon_social { get; set; }/*usado*/ /* razon social o nombre de la persona */
        public string Direccion { get; set; }/*usado*/
        public string Email { get; set; }/*usado*/
        public string Telefono { get; set; }/*usado*/ /* este campo no esta */
        public string Fecha_de_emision { get; set; }/*usado*/ /*"26-08-2021", dia mesaño */
        public string Fecha_de_vencimiento { get; set; }/*usado*/ /*"26-08-2021", dia mesaño*/
        public string Moneda { get; set; }/*usado*/ /* PEN (soles) o USD (dolares) */
        public string Tipo_de_cambio { get; set; }/*usado*/ /*formato 0.00*/
        public string Descuento_global { get; set; }/*usado*/ /* formato 0.00 */
        public string Nro_bolsas { get; set; }/*usado*/ /* formato 0 */
        public string Total { get; set; }/*usado*/ /* formato 0.00 */
        public bool Detraccion { get; set; }/*vacio*/
        public string Observaciones { get; set; }/*usado*/
        public string Documento_que_se_modifica_tipo { get; set; }/*usado*/ /* este dato obligatorio cuando es nota de credito */
        public string Documento_que_se_modifica_serie { get; set; }/*usado*/ /* este dato obligatorio cuando es nota de credito */
        public string Documento_que_se_modifica_numero { get; set; }/*usado*/ /* este dato obligatorio cuando es nota de credito */
        public string Concepto_de_nota__de_credito { get; set; }/*usado*/ /* 1 2 3 .... 10 */ /* preguntar q es tipo nota de credito? */
        public string Motivo_de_nota__de_credito { get; set; }/*usado*/ /* descipcion del motivo de la nota de credito */
        public string Tipo_de_nota_de_debito { get; set; }
        public bool Enviar_automaticamente_a_la_sunat { get; set; }/*vacio*/ /* esto aun no funcionaria */
        public bool Enviar_automaticamente_al_cliente { get; set; }/*usado*/ /*siempre envia si hay email*/
        public string Periodo { get; set; }/*"08-2021", mes año */
        public bool Cancelado { get; set; }/*vacio*/
        public string Codigo_unico { get; set; }/*vacio*/
        public int IncluirIgv { get; set; }
        public int Bus { get; set; }
        public string Placa { get; set; }
        public int IdDetalleBus { get; set; }
        public int IdEstablecimiento { get; set; }
        public string MedioDePago { get; set; }
        public List<DatosInvoice_lines> Invoice_lines { get; set; }
    }

    public class DatosInvoice_lines
    {
        public string Unit_code { get; set; }/*usado*/ /* codigo del producto */ /* la mmedida sera la principal? */ /* preguntar que sucede cuando es nota de credito NC00 */
        public string Cantidad { get; set; }/*usado*/ /* formato 0.000 */
        public string Tipo_de_igv { get; set; }/*usado*/ /* 0 gravado 1 exonerado 2 gratuito 3 inafecto */
        public string Precio_unitario { get; set; }/*usado*/ /* formato 0.000 */
        public string Descripcion { get; set; }/*usado*/
        public int IdProducto { get; set; } /* solo se usa dentro del web service */
    }
}
