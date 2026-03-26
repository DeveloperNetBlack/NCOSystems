using System;
using System.Collections.Generic;
using System.Text;

namespace NCOSystems.Entity.Personal
{
    public class PersonalEntity
    {
        public int IdPersonal { get; set; }

        public int IdComuna { get; set; }

        public string? RutPersonal { get; set; }

        public string? NombrePersonal { get; set; }

        public string? ApellidoPaternoPersonal { get; set; }

        public string? ApellidoMaternoPersonal { get; set; }

        public string? TelefonoPersonal { get; set; }

        public string? CorreoElectronico { get; set; }

        public DateTime? FecLicenciaB { get; set; }

        public int IndVigencia { get; set; }

        public string? IdUsuario { get; set; }

    }
}
