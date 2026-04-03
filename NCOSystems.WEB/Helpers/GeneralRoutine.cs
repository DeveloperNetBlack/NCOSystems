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
    }
}
