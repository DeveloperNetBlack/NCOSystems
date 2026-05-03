using CompileIT.NET9.DB.SQLServer;
using Microsoft.Extensions.Configuration;
using NCOSystems.Entity.Personal;
using System.Data;

namespace NCOSystems.DAL
{
    public class PersonalTipoLicencia
    {
        public void InsertarPersonalTipoLicencia(PersonalTipoLicenciaEntity personalTipoLicenciaEntity, IConfiguration configuration)
        {
            Connection<PersonalTipoLicenciaEntity> conn = new(configuration);
            Parameters parameters = new Parameters();

            conn.Devolution = TypeRefund.Register.None;

            parameters.NameProcedure = "SP_INS_PERSONAL_TIPO_LICENCIA";

            parameters.addParameters("@PI_ID_PERSONAL", TypeData.DataType.Int, 0, ParameterDirection.Input, personalTipoLicenciaEntity.IdPersonal);
            parameters.addParameters("@PI_ID_TIPO_LICENCIA", TypeData.DataType.Int, 0, ParameterDirection.Input, personalTipoLicenciaEntity.IdTipoLicencia!);
            parameters.addParameters("@PI_FEC_VCTO_LICENCIA", TypeData.DataType.Date, 0, ParameterDirection.Input, personalTipoLicenciaEntity.FechaVctoLicencia);
            parameters.addParameters("@PI_FEC_OTORGAMIENTO", TypeData.DataType.Date, 0, ParameterDirection.Input, personalTipoLicenciaEntity.FechaOtorgamiento);
            parameters.addParameters("@PI_ID_USUARIO", TypeData.DataType.Varchar, 30, ParameterDirection.Input, personalTipoLicenciaEntity.IdUsuario!);

            conn.ExecuteSQL(parameters);
        }

        public void ActualizarPersonalTipoLicencia(PersonalTipoLicenciaEntity personalTipoLicenciaEntity, IConfiguration configuration)
        {
            Connection<PersonalTipoLicenciaEntity> conn = new(configuration);
            Parameters parameters = new Parameters();

            conn.Devolution = TypeRefund.Register.None;

            parameters.NameProcedure = "SP_UPD_PERSONAL_TIPO_LICENCIA";

            parameters.addParameters("@PI_ID_PERSONAL_TIPO_LICENCIA", TypeData.DataType.Int, 0, ParameterDirection.Input, personalTipoLicenciaEntity.IdPersonalTipoLicencia);
            parameters.addParameters("@PI_ID_PERSONAL", TypeData.DataType.Int, 0, ParameterDirection.Input, personalTipoLicenciaEntity.IdPersonal);
            parameters.addParameters("@PI_ID_TIPO_LICENCIA", TypeData.DataType.Int, 0, ParameterDirection.Input, personalTipoLicenciaEntity.IdTipoLicencia!);
            parameters.addParameters("@PI_FEC_VCTO_LICENCIA", TypeData.DataType.Date, 0, ParameterDirection.Input, personalTipoLicenciaEntity.FechaVctoLicencia);
            parameters.addParameters("@PI_FEC_OTORGAMIENTO", TypeData.DataType.Date, 0, ParameterDirection.Input, personalTipoLicenciaEntity.FechaOtorgamiento);
            parameters.addParameters("@PI_ID_USUARIO", TypeData.DataType.Varchar, 30, ParameterDirection.Input, personalTipoLicenciaEntity.IdUsuario!);

            conn.ExecuteSQL(parameters);
        }

        public void EliminarPersonalTipoLicencia(int idPersonalTipoLicencia, IConfiguration configuration)
        {
            Connection<PersonalTipoLicenciaEntity> conn = new(configuration);
            Parameters parameters = new Parameters();

            conn.Devolution = TypeRefund.Register.None;

            parameters.NameProcedure = "SP_DEL_PERSONAL_TIPO_LICENCIA";

            parameters.addParameters("@PI_ID_PERSONAL_TIPO_LICENCIA", TypeData.DataType.Int, 0, ParameterDirection.Input, idPersonalTipoLicencia);

            conn.ExecuteSQL(parameters);
        }

        public List<PersonalTipoLicenciaEntity> ListarPersonalTipoLicencia(int idPersonal, IConfiguration configuration)
        {
            Connection<PersonalTipoLicenciaEntity> conn = new(configuration);
            Parameters parameters = new Parameters();

            conn.Devolution = TypeRefund.Register.Entity;

            parameters.NameProcedure = "SP_SEL_PERSONAL_TIPO_LICENCIA";

            parameters.addParameters("@PI_ID_PERSONAL", TypeData.DataType.Int, 0, ParameterDirection.Input, idPersonal);

            conn.ExecuteSQL(parameters);

            if (conn.ReturnEntity != null)
            {
                return conn.ReturnEntity.ToList();
            }
            else
            {
                return new List<PersonalTipoLicenciaEntity>();
            }
        }
    }
}
