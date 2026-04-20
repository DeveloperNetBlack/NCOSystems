namespace NCOSystems.Entity.Personal
{
    public class PersonalTipoLicenciaEntity
    {
        public int IdPersonalTipoLicencia { get; set; }
        public int IdPersonal { get; set; }
        public int IdTipoLicencia { get; set; }
        public string? NombreClaseLicencia { get; set; }
        public string? FecVctoLicencia { get; set; }
        public string? FecOtorgamiento { get; set; }
        public DateTime FechaVctoLicencia => Convert.ToDateTime(FecVctoLicencia!.Substring(6, 4) + "-" + FecVctoLicencia.Substring(3, 2) + "-" + FecVctoLicencia.Substring(0, 2));
        public DateTime FechaOtorgamiento => Convert.ToDateTime(FecOtorgamiento!.Substring(6, 4) + "-" + FecOtorgamiento.Substring(3, 2) + "-" + FecOtorgamiento.Substring(0, 2));
        public string? IdUsuario { get; set; }
    }
}
