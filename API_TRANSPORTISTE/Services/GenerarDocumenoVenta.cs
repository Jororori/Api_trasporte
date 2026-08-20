using CapaEntidades;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Globalization;
using System.Net;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Xml;

namespace API_TRANSPORTISTE.Services
{
    public static class GenerarDocumenoVenta
    {
        public class ResultadoFactura
        {
            public string NumeroDocumento { get; set; }
            public byte[] PdfBytes { get; set; }
        }

        public static ResultadoFactura GenerarFactBol(DetalleDocVenta Detalles, int IncluirIGV, string DniLogin, string RucLogin, string PassLogin, string SerieDoc, int TipoDocumento, string NroDocumento, string Pasajero, DateTime? FechaNacimiento, int Edad, string Sexo, string Ruc, string RazonSocial, string Direccion, int TipoDocVenta, DateTime? FechaEmision, int IdAgenciaOrigen, int IdAgenciaDestino, string FormaDePago, string MedioPago, string Tarjeta, DateTime? FechaVencimiento, double Adelanto, string Observaciones, int IdUsuario, int Estado, int IdDocumento, int IdDetalleProgramacion, string precio, string PrecioLetra, string PrecioReprog, string HoraSalida, string Menor, int Embarque, string Telefono, string PlacaBus, string MedioDePago, int IdEstablecimiento)
        {


            var invoice = new Datosinvoice
            {
                Ruc_emisor = RucLogin,
                Dni_usuario = DniLogin,
                Pass = PassLogin,
                Tipo = TipoDocVenta.ToString(),
                Serie = SerieDoc,
                Numero = "",
                Ruc = Ruc,
                Tipo_de_documento = TipoDocumento.ToString(), // ejemplo: DNI
                Razon_social = RazonSocial,
                Direccion = Direccion,
                Email = "",
                Telefono = Telefono,
                Fecha_de_emision = FechaEmision?.ToString("dd-MM-yyyy"),
                Fecha_de_vencimiento = FechaVencimiento?.ToString("dd-MM-yyyy"),
                Moneda = "PEN",
                Tipo_de_cambio = "0.00",
                Descuento_global = "0.00",
                Nro_bolsas = "0",
                Total = precio,
                Detraccion = false,
                Observaciones = Observaciones,
                Documento_que_se_modifica_tipo = "",
                Documento_que_se_modifica_serie = "",
                Documento_que_se_modifica_numero = "",
                Concepto_de_nota__de_credito = "",
                Motivo_de_nota__de_credito = "",
                Tipo_de_nota_de_debito = "",
                Enviar_automaticamente_a_la_sunat = false,
                Enviar_automaticamente_al_cliente = true,
                Periodo = FechaEmision?.ToString("MM-yyyy"),
                Cancelado = false,
                Codigo_unico = "",
                IncluirIgv = IncluirIGV,
                Bus = 1,
                Placa = PlacaBus,
                IdDetalleBus = IdDetalleProgramacion,
                IdEstablecimiento = IdEstablecimiento,
                MedioDePago = MedioDePago,


                Invoice_lines = new List<DatosInvoice_lines>
                {
                    new DatosInvoice_lines
                    {
                        Unit_code = Detalles.Codigo,
                        Cantidad = "1.000",
                        Tipo_de_igv = "1",
                        Precio_unitario = precio,
                        Descripcion = Detalles.Descripcion,
                        IdProducto = Detalles.IdProducto
                    }
                }
            };

            var encabezado = new Encabezado
            {
                Type = TipoDocVenta == 1 ? "FACTURA" : "BOLETA",
                Invoice = invoice
            };

            string valores = JsonConvert.SerializeObject(encabezado);

            string soapEnvelope = $@"<?xml version=""1.0"" encoding=""utf-8""?>
            <soap:Envelope xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance""
                           xmlns:xsd=""http://www.w3.org/2001/XMLSchema""
                           xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"">
              <soap:Body>
                <GetDocumento xmlns=""http://webservice.org/"">
                  <valores><![CDATA[{valores}]]></valores>
                </GetDocumento>
              </soap:Body>
            </soap:Envelope>";

            var request = (HttpWebRequest)WebRequest.Create("https://factura-2.pe/WS/WebServiceFactura-2.asmx");

            request.Method = "POST";
            request.ContentType = "text/xml; charset=utf-8";
            request.Headers.Add("SOAPAction", "\"http://webservice.org/GetDocumento\"");

            byte[] bytes = Encoding.UTF8.GetBytes(soapEnvelope);
            request.ContentLength = bytes.Length;

            using (var stream = request.GetRequestStream())
            {
                stream.Write(bytes, 0, bytes.Length);
            }

            string resultado;

            using (var response = (HttpWebResponse)request.GetResponse())
            using (var reader = new StreamReader(response.GetResponseStream()))
            {
                resultado = reader.ReadToEnd();
            }

            return ObtenerDatosFactura(resultado);

        }

        public static string GenerarNC(DocumentoVenta documento)
        {
            var encabezado = new Encabezado2
            {
                Type = "NOTA_DE_CREDITO",
                Invoice = documento
            };

            string valores = JsonConvert.SerializeObject(encabezado);

            string soapEnvelope = $@"<?xml version=""1.0"" encoding=""utf-8""?>
            <soap:Envelope xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance""
                           xmlns:xsd=""http://www.w3.org/2001/XMLSchema""
                           xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"">
              <soap:Body>
                <GetDocumento xmlns=""http://webservice.org/"">
                  <valores><![CDATA[{valores}]]></valores>
                </GetDocumento>
              </soap:Body>
            </soap:Envelope>";

            var request = (HttpWebRequest)WebRequest.Create("https://factura-2.pe/WS/WebServiceFactura-2.asmx");

            request.Method = "POST";
            request.ContentType = "text/xml; charset=utf-8";
            request.Headers.Add("SOAPAction", "\"http://webservice.org/GetDocumento\"");

            byte[] bytes = Encoding.UTF8.GetBytes(soapEnvelope);
            request.ContentLength = bytes.Length;

            using (var stream = request.GetRequestStream())
            {
                stream.Write(bytes, 0, bytes.Length);
            }

            string resultado;

            using (var response = (HttpWebResponse)request.GetResponse())
            using (var reader = new StreamReader(response.GetResponseStream()))
            {
                resultado = reader.ReadToEnd();
            }
            return resultado;
        }

        public static ResultadoFactura RegenerarPdf(int IdDocumentoVenta, int Tipo, int IdDetalleBus)
        {
            string soapEnvelope = $@"<?xml version=""1.0"" encoding=""utf-8""?>
            <soap:Envelope xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance""
                           xmlns:xsd=""http://www.w3.org/2001/XMLSchema""
                           xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"">
              <soap:Body>
                <RegenerarPdf xmlns=""http://webservice.org/"">
                  <IdDocumentoVenta><![CDATA[{IdDocumentoVenta}]]></IdDocumentoVenta>
                  <Tipo><![CDATA[{Tipo}]]></Tipo>
                  <IdDetalleBus><![CDATA[{IdDetalleBus}]]></IdDetalleBus>
                </RegenerarPdf>
              </soap:Body>
            </soap:Envelope>";

            var request = (HttpWebRequest)WebRequest.Create("https://factura-2.pe/WS/WebServiceFactura-2.asmx");

            request.Method = "POST";
            request.ContentType = "text/xml; charset=utf-8";
            request.Headers.Add("SOAPAction", "\"http://webservice.org/RegenerarPdf\"");

            byte[] bytes = Encoding.UTF8.GetBytes(soapEnvelope);
            request.ContentLength = bytes.Length;

            using (var stream = request.GetRequestStream())
            {
                stream.Write(bytes, 0, bytes.Length);
            }

            string resultado;

            using (var response = (HttpWebResponse)request.GetResponse())
            using (var reader = new StreamReader(response.GetResponseStream()))
            {
                resultado = reader.ReadToEnd();
            }

            return ObtenerDatosRegFactura(resultado);
        }

        private static ResultadoFactura ObtenerDatosFactura(string respuestaSOAP)
        {
            // 1. Extraer el JSON del XML
            string patron = @"<GetDocumentoResult>(.*?)</GetDocumentoResult>";
            Match match = Regex.Match(respuestaSOAP, patron, RegexOptions.Singleline);

            if (!match.Success)
                throw new Exception("No se encontró GetDocumentoResult");

            string json = match.Groups[1].Value;

            // 2. Parsear el JSON
            JObject obj = JObject.Parse(json);

            // 3. Obtener el número de documento
            string numeroDocumento = obj["NumeroDocumento"]?.ToString();

            // 4. Obtener el PDF en Base64
            string pdfBase64 = obj["ArchivoPDF"]?.ToString();

            if (string.IsNullOrEmpty(pdfBase64))
                throw new Exception("No se encontró ArchivoPDF");

            // 5. Convertir Base64 a bytes
            byte[] pdfBytes = Convert.FromBase64String(pdfBase64);

            return new ResultadoFactura
            {
                NumeroDocumento = numeroDocumento,
                PdfBytes = pdfBytes
            };
        }

        private static ResultadoFactura ObtenerDatosRegFactura(string respuestaSOAP)
        {
            // 1. Extraer el JSON del XML
            string patron = @"<RegenerarPdfResult>(.*?)</RegenerarPdfResult>";
            Match match = Regex.Match(respuestaSOAP, patron, RegexOptions.Singleline);

            if (!match.Success)
                throw new Exception("No se encontró RegenerarPdfResult");

            string json = match.Groups[1].Value;

            // 2. Parsear el JSON
            JObject obj = JObject.Parse(json);

            // 3. Obtener el número de documento
            string numeroDocumento = obj["NumeroDocumento"]?.ToString();

            // 4. Obtener el PDF en Base64
            string pdfBase64 = obj["ArchivoPDF"]?.ToString();

            if (string.IsNullOrEmpty(pdfBase64))
                throw new Exception("No se encontró ArchivoPDF");

            // 5. Convertir Base64 a bytes
            byte[] pdfBytes = Convert.FromBase64String(pdfBase64);

            return new ResultadoFactura
            {
                NumeroDocumento = numeroDocumento,
                PdfBytes = pdfBytes
            };
        }
    }
}
