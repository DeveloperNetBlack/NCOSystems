using System;
using System.Collections.Generic;
using System.Text;

namespace NCOSystems.Entity.Personal
{
    public class PersonalTipoLicencia
    {
        public int IdPersonalTipoLicencia { get; set; }
        public int IdPersonal { get; set; }
        public int IdTipoLicencia { get; set; }
        public string? NombreClaseLicencia { get; set; }
        public DateTime? FechaVctoLicencia { get; set; }
        public string? IdUsuario { get; set; }
    }
}
