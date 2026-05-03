using CompileIT.NET9.DB.SQLServer;
using Microsoft.Extensions.Configuration;
using NCOSystems.Entity.Personal;
using System.Data;

namespace NCOSystems.DAL
{
    public class PersonalHijo
    {
        public void InsertarHijo(PersonalHijoEntity personalHijoEntity, IConfiguration configuration)
        {
            Connection<PersonalHijoEntity> conn = new(configuration);
            Parameters parameters = new Parameters();

            conn.Devolution = TypeRefund.Register.None;

            parameters.NameProcedure = "SP_INS_PERSONAL_HIJO";

            parameters.addParameters("@PI_ID_PERSONAL", TypeData.DataType.Int, 0, ParameterDirection.Input, personalHijoEntity.IdPersonal);
            parameters.addParameters("@PI_NOMBRE_HIJO", TypeData.DataType.Varchar, 80, ParameterDirection.Input, personalHijoEntity.NombreHijo!.ToUpper());
            parameters.addParameters("@PI_EDAD_HIJO", TypeData.DataType.Int, 0, ParameterDirection.Input, personalHijoEntity.EdadHijo!);
            parameters.addParameters("@PI_ID_USUARIO", TypeData.DataType.Varchar, 30, ParameterDirection.Input, personalHijoEntity.IdUsuario!);

            conn.ExecuteSQL(parameters);
        }

        public void ActualizarHijo(PersonalHijoEntity personalHijoEntity, IConfiguration configuration)
        {
            Connection<PersonalHijoEntity> conn = new(configuration);
            Parameters parameters = new Parameters();

            conn.Devolution = TypeRefund.Register.None;

            parameters.NameProcedure = "SP_UPD_PERSONAL_HIJO";

            parameters.addParameters("@PI_ID_PERSONAL_HIJO", TypeData.DataType.Int, 0, ParameterDirection.Input, personalHijoEntity.IdPersonalHijo);
            parameters.addParameters("@PI_ID_PERSONAL", TypeData.DataType.Int, 0, ParameterDirection.Input, personalHijoEntity.IdPersonal);
            parameters.addParameters("@PI_NOMBRE_HIJO", TypeData.DataType.Varchar, 80, ParameterDirection.Input, personalHijoEntity.NombreHijo!.ToUpper());
            parameters.addParameters("@PI_EDAD_HIJO", TypeData.DataType.Int, 0, ParameterDirection.Input, personalHijoEntity.EdadHijo!);
            parameters.addParameters("@PI_ID_USUARIO", TypeData.DataType.Varchar, 30, ParameterDirection.Input, personalHijoEntity.IdUsuario!);

            conn.ExecuteSQL(parameters);
        }

        public void EliminarHijo(int idPersonalHijo, IConfiguration configuration)
        {
            Connection<PersonalHijoEntity> conn = new(configuration);
            Parameters parameters = new Parameters();

            conn.Devolution = TypeRefund.Register.None;

            parameters.NameProcedure = "SP_DEL_PERSONAL_HIJO";

            parameters.addParameters("@PI_ID_PERSONAL_HIJO", TypeData.DataType.Int, 0, ParameterDirection.Input, idPersonalHijo);

            conn.ExecuteSQL(parameters);
        }

        public List<PersonalHijoEntity> ListarPersonalHijo(int idPersonal, IConfiguration configuration)
        {
            Connection<PersonalHijoEntity> conn = new(configuration);
            Parameters parameters = new Parameters();

            conn.Devolution = TypeRefund.Register.Entity;

            parameters.NameProcedure = "SP_SEL_PERSONAL_HIJO";

            parameters.addParameters("@PI_ID_PERSONAL", TypeData.DataType.Int, 0, ParameterDirection.Input, idPersonal);

            conn.ExecuteSQL(parameters);

            if (conn.ReturnEntity != null)
            {
                return conn.ReturnEntity.ToList();
            }
            else
            {
                return new List<PersonalHijoEntity>();
            }
        }

    }
}
