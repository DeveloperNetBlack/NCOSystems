using System;
using System.Collections.Generic;
using System.Text;

namespace NCOSystems.Entity.Personal
{
    public class PersonalHijoEntity
    {
        public int IdPersonalHijo { get; set; }
        public int IdPersonal { get; set; }
        public string? NombreHijo { get; set; }
        public int EdadHijo { get; set; }
        public string? IdUsuario { get; set; }
    }
}
