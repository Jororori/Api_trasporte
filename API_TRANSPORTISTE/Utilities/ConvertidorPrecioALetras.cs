namespace API_TRANSPORTISTE.Utilities
{
    /// <summary>
    /// Utilidad para convertir números a letras (Moneda Peruana - Soles)
    /// </summary>
    public static class ConvertidorPrecioALetras
    {
        private static readonly string[] Unidades = { "", "UNO", "DOS", "TRES", "CUATRO", "CINCO", "SEIS", "SIETE", "OCHO", "NUEVE" };
        private static readonly string[] Decenas = { "", "DIEZ", "VEINTE", "TREINTA", "CUARENTA", "CINCUENTA", "SESENTA", "SETENTA", "OCHENTA", "NOVENTA" };
        private static readonly string[] Centenas = { "", "CIENTO", "DOSCIENTOS", "TRESCIENTOS", "CUATROCIENTOS", "QUINIENTOS", "SEISCIENTOS", "SETECIENTOS", "OCHOCIENTOS", "NOVECIENTOS" };
        private static readonly string[] Especiales = { "DIEZ", "ONCE", "DOCE", "TRECE", "CATORCE", "QUINCE", "DIECISÉIS", "DIECISIETE", "DIECIOCHO", "DIECINUEVE" };

        /// <summary>
        /// Convierte un precio string a letras en formato: "CIEN Y 00/100 SOLES"
        /// </summary>
        /// <param name="precioStr">Precio en formato string (ej: "100.50" o "100,50")</param>
        /// <returns>Precio convertido a letras</returns>
        public static string ConvertirPrecioALetras(string precioStr)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(precioStr))
                    return "CERO Y 00/100 SOLES";

                // Normalizar: reemplazar coma por punto
                precioStr = precioStr.Replace(",", ".");

                if (!decimal.TryParse(precioStr, out decimal precio) || precio < 0)
                    return "CERO Y 00/100 SOLES";

                // Separar soles y centavos
                long soles = (long)Math.Floor(precio);
                int centavos = (int)Math.Round((precio - soles) * 100);

                // Validar centavos (máximo 99)
                if (centavos > 99)
                {
                    soles += 1;
                    centavos = 0;
                }

                // Convertir soles a letras
                string solesEnLetras = ConvertirNumeroALetras(soles);

                // Formatear centavos con dos dígitos
                string centavosFormato = centavos.ToString("D2");

                return $"{solesEnLetras} Y {centavosFormato}/100 SOLES";
            }
            catch
            {
                return "CERO Y 00/100 SOLES";
            }
        }

        /// <summary>
        /// Convierte un número a su representación en letras
        /// </summary>
        private static string ConvertirNumeroALetras(long numero)
        {
            if (numero == 0)
                return "CERO";

            if (numero < 0)
                return "MENOS " + ConvertirNumeroALetras(-numero);

            string resultado = "";

            // Millones
            if (numero >= 1000000)
            {
                long millones = numero / 1000000;
                resultado += ConvertirGrupoTresDigitos(millones);
                resultado += millones == 1 ? " MILLÓN" : " MILLONES";
                numero %= 1000000;

                if (numero > 0)
                    resultado += " ";
            }

            // Miles
            if (numero >= 1000)
            {
                long miles = numero / 1000;
                resultado += ConvertirGrupoTresDigitos(miles);
                resultado += miles == 1 ? " MIL" : " MIL";
                numero %= 1000;

                if (numero > 0)
                    resultado += " ";
            }

            // Cientos
            if (numero > 0)
            {
                resultado += ConvertirGrupoTresDigitos(numero);
            }

            return resultado.Trim();
        }

        /// <summary>
        /// Convierte un número de hasta 3 dígitos a letras
        /// </summary>
        private static string ConvertirGrupoTresDigitos(long numero)
        {
            if (numero == 0)
                return "";

            string resultado = "";

            // Centenas
            long centena = numero / 100;
            if (centena > 0)
            {
                resultado += Centenas[centena];
                numero %= 100;
                if (numero > 0)
                    resultado += " ";
            }

            // Decenas y unidades
            if (numero >= 10 && numero < 20)
            {
                resultado += Especiales[numero - 10];
            }
            else
            {
                long decena = numero / 10;
                if (decena > 0)
                {
                    resultado += Decenas[decena];
                    numero %= 10;
                    if (numero > 0)
                        resultado += " Y ";
                }

                if (numero > 0)
                {
                    resultado += Unidades[numero];
                }
            }

            return resultado.Trim();
        }
    }
}
