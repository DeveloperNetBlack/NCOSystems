using CompileIT.NET9.DB.SQLServer;
using Microsoft.Extensions.Configuration;
using NCOSystems.Entity.Personal;
using System.Data;

namespace NCOSystems.DAL
{
    public class Personal
    {
        public int Insertar(PersonalEntity personalEntity, IConfiguration configuration)
        {
            Connection<PersonalEntity> conn = new(configuration);
            Parameters parameters = new Parameters();
            int retorno = 0;

            conn.Devolution = TypeRefund.Register.Scale;

            parameters.NameProcedure = "SP_INS_PERSONAL";

            parameters.addParameters("@PI_ID_COMUNA", TypeData.DataType.Int, 0, ParameterDirection.Input, Convert.ToInt32(personalEntity.IdComuna));
            parameters.addParameters("@PI_ID_ESTADO_CIVIL", TypeData.DataType.Int, 0, ParameterDirection.Input, Convert.ToInt32(personalEntity.IdEstadoCivil));
            parameters.addParameters("@PI_ID_ESTADO_LABORAL", TypeData.DataType.Int, 0, ParameterDirection.Input, Convert.ToInt32(personalEntity.IdEstadoLaboral));
            parameters.addParameters("@PI_ID_GENERO", TypeData.DataType.Int, 0, ParameterDirection.Input, Convert.ToInt32(personalEntity.IdGenero));
            parameters.addParameters("@PI_ID_PAIS", TypeData.DataType.Int, 0, ParameterDirection.Input, Convert.ToInt32(personalEntity.IdPais));
            parameters.addParameters("@PI_RUT_PERSONAL", TypeData.DataType.Varchar, 12, ParameterDirection.Input, personalEntity.RutPersonal!);
            parameters.addParameters("@PI_NOMBRE_PERSONAL", TypeData.DataType.Varchar, 80, ParameterDirection.Input, personalEntity.NombrePersonal!);
            parameters.addParameters("@PI_AP_PATERNO_PERSONAL", TypeData.DataType.Varchar, 80, ParameterDirection.Input, personalEntity.ApPaternoPersonal!);
            parameters.addParameters("@PI_AP_MATERNO_PERSONAL", TypeData.DataType.Varchar, 80, ParameterDirection.Input, personalEntity.ApMaternoPersonal!);
            parameters.addParameters("@PI_FEC_NACIMIENTO", TypeData.DataType.DateTime, 0, ParameterDirection.Input, personalEntity.FecNacimiento!);
            parameters.addParameters("@PI_DIRECCION", TypeData.DataType.Varchar, 100, ParameterDirection.Input, personalEntity.Direccion!);
            parameters.addParameters("@PI_TELEFONO_PERSONAL", TypeData.DataType.Varchar, 50, ParameterDirection.Input, personalEntity.TelefonoPersonal!);
            parameters.addParameters("@PI_CORREO_ELECTRONICO", TypeData.DataType.Varchar, 90, ParameterDirection.Input, personalEntity.CorreoElectronico!);
            parameters.addParameters("@PI_IND_VIGENCIA", TypeData.DataType.Int, 0, ParameterDirection.Input, personalEntity.IndVigencia);
            parameters.addParameters("@PI_ID_USUARIO", TypeData.DataType.Varchar, 30, ParameterDirection.Input, personalEntity.IdUsuario!);

            conn.ExecuteSQL(parameters);

            if (conn.ReturnScale != null)
            {
                retorno = Convert.ToInt32(conn.ReturnScale);
            }

            return retorno;

        }

        public void Actualizar(PersonalEntity personalEntity, IConfiguration configuration)
        {
            Connection<PersonalEntity> conn = new(configuration);
            Parameters parameters = new Parameters();

            conn.Devolution = TypeRefund.Register.None;

            parameters.NameProcedure = "SP_UPD_PERSONAL";

            parameters.addParameters("@PI_ID_PERSONAL", TypeData.DataType.Int, 0, ParameterDirection.Input, personalEntity.IdPersonal);
            parameters.addParameters("@PI_ID_COMUNA", TypeData.DataType.Int, 0, ParameterDirection.Input, Convert.ToInt32(personalEntity.IdComuna));
            parameters.addParameters("@PI_ID_ESTADO_CIVIL", TypeData.DataType.Int, 0, ParameterDirection.Input, Convert.ToInt32(personalEntity.IdEstadoCivil));
            parameters.addParameters("@PI_ID_ESTADO_LABORAL", TypeData.DataType.Int, 0, ParameterDirection.Input, Convert.ToInt32(personalEntity.IdEstadoLaboral));
            parameters.addParameters("@PI_ID_GENERO", TypeData.DataType.Int, 0, ParameterDirection.Input, Convert.ToInt32(personalEntity.IdGenero));
            parameters.addParameters("@PI_ID_PAIS", TypeData.DataType.Int, 0, ParameterDirection.Input, Convert.ToInt32(personalEntity.IdPais));
            parameters.addParameters("@PI_RUT_PERSONAL", TypeData.DataType.Varchar, 12, ParameterDirection.Input, personalEntity.RutPersonal!);
            parameters.addParameters("@PI_NOMBRE_PERSONAL", TypeData.DataType.Varchar, 80, ParameterDirection.Input, personalEntity.NombrePersonal!);
            parameters.addParameters("@PI_AP_PATERNO_PERSONAL", TypeData.DataType.Varchar, 80, ParameterDirection.Input, personalEntity.ApPaternoPersonal!);
            parameters.addParameters("@PI_AP_MATERNO_PERSONAL", TypeData.DataType.Varchar, 80, ParameterDirection.Input, personalEntity.ApMaternoPersonal!);
            parameters.addParameters("@PI_FEC_NACIMIENTO", TypeData.DataType.DateTime, 0, ParameterDirection.Input, personalEntity.FecNacimiento!);
            parameters.addParameters("@PI_DIRECCION", TypeData.DataType.Varchar, 100, ParameterDirection.Input, personalEntity.Direccion!);
            parameters.addParameters("@PI_TELEFONO_PERSONAL", TypeData.DataType.Varchar, 50, ParameterDirection.Input, personalEntity.TelefonoPersonal!);
            parameters.addParameters("@PI_CORREO_ELECTRONICO", TypeData.DataType.Varchar, 90, ParameterDirection.Input, personalEntity.CorreoElectronico!);
            parameters.addParameters("@PI_IND_VIGENCIA", TypeData.DataType.Int, 0, ParameterDirection.Input, personalEntity.IndVigencia);
            parameters.addParameters("@PI_ID_USUARIO", TypeData.DataType.Varchar, 30, ParameterDirection.Input, personalEntity.IdUsuario!);

            conn.ExecuteSQL(parameters);
        }

        public List<PersonalEntity> ListarPersonal(string rutPersonal, string nombrePersonal, IConfiguration configuration)
        {
            Connection<PersonalEntity> conn = new(configuration);
            Parameters parameters = new Parameters();

            conn.Devolution = TypeRefund.Register.Entity;

            parameters.NameProcedure = "SP_SEL_PERSONAL";

            parameters.addParameters("@PI_RUT_PERSONAL", TypeData.DataType.Varchar, 12, ParameterDirection.Input, rutPersonal);
            parameters.addParameters("@PI_NOMBRE_PERSONAL", TypeData.DataType.Varchar, 380, ParameterDirection.Input, nombrePersonal);

            conn.ExecuteSQL(parameters);

            if (conn.ReturnEntity != null)
            {
                return conn.ReturnEntity.ToList();
            }
            else
            {
                return new List<PersonalEntity>();
            }
        }
    }
}
