namespace NCOSystems.Entity.Personal
{
    public class PersonalTipoLicenciaEntity
    {
        public int IdPersonalTipoLicencia { get; set; }
        public int IdPersonal { get; set; }
        public string? IdTipoLicencia { get; set; }
        public string? NombreClaseLicencia { get; set; }
        public DateTime? FechaVctoLicencia { get; set; }
        public string? IdUsuario { get; set; }
    }
}
