using CompileIT.NET9.DB.SQLServer;
using Microsoft.Extensions.Configuration;
using NCOSystems.Entity.Parametro;
using NCOSystems.Entity.Personal;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

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
            parameters.addParameters("@PI_RUT_PERSONAL", TypeData.DataType.Varchar, 12, ParameterDirection.Input, personalEntity.RutPersonal!);
            parameters.addParameters("@PI_NOMBRE_PERSONAL", TypeData.DataType.Varchar, 80, ParameterDirection.Input, personalEntity.NombrePersonal!);
            parameters.addParameters("@PI_AP_PATERNO_PERSONAL", TypeData.DataType.Varchar, 80, ParameterDirection.Input, personalEntity.ApellidoPaternoPersonal!);
            parameters.addParameters("@PI_AP_MATERNO_PERSONAL", TypeData.DataType.Varchar, 80, ParameterDirection.Input, personalEntity.ApellidoMaternoPersonal!);
            parameters.addParameters("@PI_TELEFONO_PERSONAL", TypeData.DataType.Varchar, 50, ParameterDirection.Input, personalEntity.TelefonoPersonal!);
            parameters.addParameters("@PI_CORREO_ELECTRONICO", TypeData.DataType.Varchar, 90, ParameterDirection.Input, personalEntity.CorreoElectronico!);
            parameters.addParameters("@PI_FEC_LICENCIA_B", TypeData.DataType.Date, 0, ParameterDirection.Input, Convert.ToDateTime(personalEntity.FecLicenciaB));
            parameters.addParameters("@PI_IND_VIGENCIA", TypeData.DataType.Int, 0, ParameterDirection.Input, personalEntity.IndVigencia);
            parameters.addParameters("@PI_ID_USUARIO", TypeData.DataType.Varchar, 30, ParameterDirection.Input, personalEntity.IdUsuario!);

            conn.ExecuteSQL(parameters);

            if(conn.ReturnScale != null)
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
            parameters.addParameters("@PI_ID_COMUNA", TypeData.DataType.Int, 0, ParameterDirection.Input, personalEntity.IdComuna!);
            parameters.addParameters("@PI_RUT_PERSONAL", TypeData.DataType.Varchar, 12, ParameterDirection.Input, personalEntity.RutPersonal!);
            parameters.addParameters("@PI_NOMBRE_PERSONAL", TypeData.DataType.Varchar, 80, ParameterDirection.Input, personalEntity.NombrePersonal!);
            parameters.addParameters("@PI_AP_PATERNO_PERSONAL", TypeData.DataType.Varchar, 80, ParameterDirection.Input, personalEntity.ApellidoPaternoPersonal!);
            parameters.addParameters("@PI_AP_MATERNO_PERSONAL", TypeData.DataType.Varchar, 80, ParameterDirection.Input, personalEntity.ApellidoMaternoPersonal!);
            parameters.addParameters("@PI_TELEFONO_PERSONAL", TypeData.DataType.Varchar, 50, ParameterDirection.Input, personalEntity.TelefonoPersonal!);
            parameters.addParameters("@PI_CORREO_ELECTRONICO", TypeData.DataType.Varchar, 90, ParameterDirection.Input, personalEntity.CorreoElectronico!);
            parameters.addParameters("@PI_FEC_LICENCIA_B", TypeData.DataType.Date, 0, ParameterDirection.Input, Convert.ToDateTime(personalEntity.FecLicenciaB));
            parameters.addParameters("@PI_IND_VIGENCIA", TypeData.DataType.Int, 0, ParameterDirection.Input, personalEntity.IndVigencia);
            parameters.addParameters("@PI_ID_USUARIO", TypeData.DataType.Varchar, 30, ParameterDirection.Input, personalEntity.IdUsuario!);

            conn.ExecuteSQL(parameters);
        }

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

        public void InsertarPersonalTipoLicencia(PersonalTipoLicenciaEntity personalTipoLicenciaEntity, IConfiguration configuration)
        {
            Connection<PersonalHijoEntity> conn = new(configuration);
            Parameters parameters = new Parameters();

            conn.Devolution = TypeRefund.Register.None;

            parameters.NameProcedure = "SP_INS_PERSONAL_TIPO_LICENCIA";

            parameters.addParameters("@PI_ID_PERSONAL", TypeData.DataType.Int, 0, ParameterDirection.Input, personalTipoLicenciaEntity.IdPersonal);
            parameters.addParameters("@PI_ID_TIPO_LICENCIA", TypeData.DataType.Int, 0, ParameterDirection.Input, personalTipoLicenciaEntity.IdTipoLicencia!);
            parameters.addParameters("@PI_FEC_VCTO_LICENCIA", TypeData.DataType.Date, 0, ParameterDirection.Input, Convert.ToDateTime(personalTipoLicenciaEntity.FechaVctoLicencia));
            parameters.addParameters("@PI_ID_USUARIO", TypeData.DataType.Varchar, 30, ParameterDirection.Input, personalTipoLicenciaEntity.IdUsuario!);

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
