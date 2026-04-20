using NCOSystems.Entity.Parametro;
using NCOSystems.Entity.Personal;

namespace NCOSystems.WEB.Models
{
    public class PersonalViewModel : PersonalEntity
    {
        public List<PersonalEntity> ListaPersonal { get; set; } = new();

        public PersonalEntity personalEntity { get; set; } = new PersonalEntity();
        
        public List<RegionEntity> regionEntities { get; set; } = new();
        
        public List<ComunaEntity> comunaEntities { get; set; } = new();
        
        public List<TipoLicenciaEntity> tipoLicenciaEntities { get; set; } = new();
        
        public List<PersonalTipoLicenciaEntity> personalTipoLicenciaEntities { get; set; } = new();

        public List<PersonalHijoEntity> personalHijoEntities { get; set; } = new();

        public List<TipoDocumentoEntity> tipoDocumentoEntities { get; set; } = new();

        public List<DocumentoEntity> documentoEntities { get; set; } = new();

        public List<EstadoCivilEntity> estadoCivilEntities { get; set; } = new();

        public List<EstadoLaboralEntity> estadoLaboralEntities { get; set; } = new();

        public List<GeneroEntity> generoEntities { get; set; } = new();

        public string? FechaVctoLicencia { get; set; }
        public string? FecOtorgamiento { get; set; }

        public string? NombreHijo { get; set; }
        public int? EdadHijo { get; set; }

        public string? NombrePersonaCompleto { get; set; }
        public string? RutPersona { get; set; }
    }
}
