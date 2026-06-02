using Microsoft.Extensions.Configuration;
using NCOSystems.Entity.Parametro;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks.Dataflow;

namespace NCOSystems.BLL
{
    public class Parametro
    {
        public List<ComunaEntity> ListarComuna(int idRegion, IConfiguration configuration)
        {
            DAL.Parametro parametro = new DAL.Parametro();

            return parametro.ListarComuna(idRegion, configuration);
        }

        public List<RegionEntity> ListarRegion(IConfiguration configuration)
        {
            DAL.Parametro parametro = new DAL.Parametro();
            return parametro.ListarRegion(configuration);
        }

        public List<TipoDocumentoEntity> ListarTipoDocumento(IConfiguration configuration)
        {
            DAL.Parametro parametro = new DAL.Parametro();
            return parametro.ListarTipoDocumento(configuration);
        }

        public List<TipoLicenciaEntity> ListarTipoLicencia(IConfiguration configuration)
        {
            DAL.Parametro parametro = new DAL.Parametro();

            return parametro.ListarTipoLicencia(configuration);
        }

        public List<EstadoCivilEntity> ListarEstadoCivil(IConfiguration configuration)
        {
            DAL.Parametro parametro = new DAL.Parametro();

            return parametro.ListarEstadoCivil(configuration);
        }

        public List<EstadoLaboralEntity> ListarEstadoLaboral(IConfiguration configuration)
        {
            DAL.Parametro parametro = new DAL.Parametro();

            return parametro.ListarEstadoLaboral(configuration);
        }

        public List<GeneroEntity> ListarGenero(IConfiguration configuration)
        {
            DAL.Parametro parametro = new DAL.Parametro();

            return parametro.ListarGenero(configuration);
        }

        public List<PaisEntity> ListarPais(IConfiguration configuration)
        {
            DAL.Parametro parametro = new DAL.Parametro();

            return parametro.ListarPais(configuration);
        }
    }
}
