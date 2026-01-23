using System;
using System.Collections.Generic;
using System.Text;

namespace NCOSystems.Entity.Parametro
{
    public class DocumentoEntity
    {
        public int IdDocumento { get; set; }
        public int IdPersona { get; set; }
        public int IdTipoDocumento { get; set; }
        public string? NombreDocumento { get; set; }
        public string? IdUsuario { get; set; }
    }
}
