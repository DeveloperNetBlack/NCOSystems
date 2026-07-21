using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace NCOSystems.WEB.Helpers
{
    public class GeneralRoutine
    {
        public static string FormatearRut(string rut)
        {
            // 1. Limpiar el RUT (quitar puntos, guiones y espacios)
            string rutLimpio = rut.Replace(".", "").Replace("-", "").Trim();

            if (rutLimpio.Length < 2) return rut; // RUT inválido o muy corto

            // 2. Separar cuerpo y dígito verificador
            string cuerpo = rutLimpio.Substring(0, rutLimpio.Length - 1);
            string dv = rutLimpio.Substring(rutLimpio.Length - 1);

            // 3. Dar formato con puntos de miles y guion
            return string.Format("{0:N0}-{1}", double.Parse(cuerpo), dv).Replace(",", ".");
        }

        public static string SanitizeFileName(string fileName)
        {
            // 1. Separar nombre y extensión para no perder el .pdf, .docx, etc.
            string extension = Path.GetExtension(fileName);
            string nameOnly = Path.GetFileNameWithoutExtension(fileName);

            // 2. Normalizar y quitar tildes/diacríticos (á->a, ñ->n, ü->u, etc.)
            nameOnly = RemoveDiacritics(nameOnly);
            extension = RemoveDiacritics(extension);

            // 3. Reemplazar cualquier caracter que NO sea letra, número, espacio, guion o guion bajo
            nameOnly = Regex.Replace(nameOnly, @"[^a-zA-Z0-9 _-]", "");

            // 4. Colapsar espacios múltiples y espacios alrededor de guiones
            nameOnly = Regex.Replace(nameOnly, @"\s{2,}", " ").Trim();

            // 5. Colapsar puntos/guiones repetidos que pudieran quedar (por si acaso)
            nameOnly = Regex.Replace(nameOnly, @"\.{2,}", ".");

            // 6. Si el nombre quedó vacío (raro, pero por seguridad), poner algo por defecto
            if (string.IsNullOrWhiteSpace(nameOnly))
                nameOnly = "archivo";

            // 7. Limpiar la extensión también (por si venía con caracteres raros)
            extension = Regex.Replace(extension, @"[^a-zA-Z0-9.]", "");

            return nameOnly + extension;
        }

        public static string RemoveDiacritics(string text)
        {
            var normalized = text.Normalize(NormalizationForm.FormD);
            var sb = new StringBuilder();

            foreach (var c in normalized)
            {
                var category = CharUnicodeInfo.GetUnicodeCategory(c);
                if (category != UnicodeCategory.NonSpacingMark)
                    sb.Append(c);
            }

            return sb.ToString().Normalize(NormalizationForm.FormC);
        }

    }
}
