using System;
using System.Collections.Generic;
using System.Text;
using CompileIT.NET9.DB.SQLServer;
using System.Data;
using NCOSystems.Entity.Parametro;
using Microsoft.Extensions.Configuration;

namespace NCOSystems.DAL
{
    public class Parametro
    {
        public List<ComunaEntity> ListarComuna(int idRegion, IConfiguration configuration)
        {
            Connection<ComunaEntity> conn = new(configuration);
            Parameters parameters = new Parameters();

            conn.Devolution = TypeRefund.Register.Entity;

            parameters.NameProcedure = "SP_SEL_COMUNA";

            parameters.addParameters("@PI_ID_REGION", TypeData.DataType.Int, 0, ParameterDirection.Input, idRegion);

            conn.ExecuteSQL(parameters);

            if (conn.ReturnEntity != null)
            {
                return conn.ReturnEntity.ToList();
            }
            else
            {
                return new List<ComunaEntity>();
            }
        }

        public List<RegionEntity> ListarRegion(IConfiguration configuration)
        {
            Connection<RegionEntity> conn = new(configuration);
            Parameters parameters = new Parameters();

            conn.Devolution = TypeRefund.Register.Entity;

            parameters.NameProcedure = "SP_SEL_REGION";

            conn.ExecuteSQL(parameters);

            if (conn.ReturnEntity != null)
            {
                return conn.ReturnEntity.ToList();
            }
            else
            {
                return new List<RegionEntity>();
            }
        }

        public List<TipoDocumentoEntity> ListarTipoDocumento(IConfiguration configuration)
        {
            Connection<TipoDocumentoEntity> conn = new(configuration);
            Parameters parameters = new Parameters();

            conn.Devolution = TypeRefund.Register.Entity;

            parameters.NameProcedure = "SP_SEL_TIPO_DOCUMENTO";

            conn.ExecuteSQL(parameters);

            if (conn.ReturnEntity != null)
            {
                return conn.ReturnEntity.ToList();
            }
            else
            {
                return new List<TipoDocumentoEntity>();
            }
        }

        public List<TipoLicenciaEntity> ListarTipoLicencia(IConfiguration configuration)
        {
            Connection<TipoLicenciaEntity> conn = new(configuration);
            Parameters parameters = new Parameters();

            conn.Devolution = TypeRefund.Register.Entity;
            
            parameters.NameProcedure = "SP_SEL_TIPO_LICENCIA";
            
            conn.ExecuteSQL(parameters);
            
            if (conn.ReturnEntity != null)
            {
                return conn.ReturnEntity.ToList();
            }
            else
            {
                return new List<TipoLicenciaEntity>();
            }
        }

        public List<EstadoCivilEntity> ListarEstadoCivil(IConfiguration configuration)
        {
            Connection<EstadoCivilEntity> conn = new(configuration);
            Parameters parameters = new Parameters();

            conn.Devolution = TypeRefund.Register.Entity;

            parameters.NameProcedure = "SP_SEL_ESTADO_CIVIL";

            conn.ExecuteSQL(parameters);

            if (conn.ReturnEntity != null)
            {
                return conn.ReturnEntity.ToList();
            }
            else
            {
                return new List<EstadoCivilEntity>();
            }
        }

        public List<EstadoLaboralEntity> ListarEstadoLaboral(IConfiguration configuration)
        {
            Connection<EstadoLaboralEntity> conn = new(configuration);
            Parameters parameters = new Parameters();

            conn.Devolution = TypeRefund.Register.Entity;

            parameters.NameProcedure = "SP_SEL_ESTADO_LABORAL";

            conn.ExecuteSQL(parameters);

            if (conn.ReturnEntity != null)
            {
                return conn.ReturnEntity.ToList();
            }
            else
            {
                return new List<EstadoLaboralEntity>();
            }
        }
    }
}
