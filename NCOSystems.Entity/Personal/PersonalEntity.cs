using System.ComponentModel.DataAnnotations;

namespace NCOSystems.Entity.Personal
{
    public class PersonalEntity
    {
        public int IdPersonal { get; set; }

        [Required(ErrorMessage = "Debe seleccionar una comuna")]
        public int IdComuna { get; set; }

        [Required(ErrorMessage = "Debe seleccionar una región")]
        public int IdRegion { get; set; }

        [Required(ErrorMessage = "El RUT es obligatorio")]
        public string? RutPersonal { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        public string? NombrePersonal { get; set; }

        [Required(ErrorMessage = "El Apellido Paterno es obligatorio")]
        public string? ApPaternoPersonal { get; set; }

        public string? ApMaternoPersonal { get; set; }

        public string? NombreCompletoPersonal { get { return $"{NombrePersonal} {ApPaternoPersonal} {ApMaternoPersonal}"; } }

        [Required(ErrorMessage = "El Teléfono es obligatorio")]
        public string? TelefonoPersonal { get; set; }

        [Required(ErrorMessage = "El Correo Electrónico es obligatorio")]
        public string? CorreoElectronico { get; set; }

        public string? NombreComuna { get; set; }

        public string? NombreRegion { get; set; }

        public int IndVigencia { get; set; }

        public string? IdUsuario { get; set; }

    }
}
