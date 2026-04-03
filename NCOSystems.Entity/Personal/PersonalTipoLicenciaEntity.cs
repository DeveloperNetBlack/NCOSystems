namespace NCOSystems.Entity.Personal
{
    public class PersonalTipoLicenciaEntity
    {
        public int IdPersonalTipoLicencia { get; set; }
        public int IdPersonal { get; set; }
        public int IdTipoLicencia { get; set; }
        public string? NombreClaseLicencia { get; set; }
        public DateTime? FecVctoLicencia { get; set; }
        public DateTime? FecOtorgamiento { get; set; }
        public string? IdUsuario { get; set; }
    }
}
