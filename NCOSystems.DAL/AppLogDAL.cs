// AppLogDAL.cs
using CompileIT.NET9.DB.SQLServer;
using Microsoft.Extensions.Configuration;
using NCOSystems.Entity.Log;
using NCOSystems.Entity.Parametro;
using System.Data;

namespace NCOSystems.DAL
{
    public class AppLogDAL
    {
        public void Insertar(AppLogEntity log, IConfiguration configuration)
        {
            Connection<AppLogEntity> conn = new(configuration);
            Parameters parameters = new Parameters();

            conn.Devolution = TypeRefund.Register.Scale;
            parameters.NameProcedure = "SP_INS_APP_LOG";

            parameters.addParameters("@PI_LEVEL", TypeData.DataType.Varchar, 20, ParameterDirection.Input, log.Level);
            parameters.addParameters("@PI_CATEGORY", TypeData.DataType.Varchar, 100, ParameterDirection.Input, log.Category ?? (object)DBNull.Value);
            parameters.addParameters("@PI_EVENT_TYPE", TypeData.DataType.Varchar, 100, ParameterDirection.Input, log.EventType ?? (object)DBNull.Value);
            parameters.addParameters("@PI_MESSAGE", TypeData.DataType.Varchar, 4000, ParameterDirection.Input, log.Message);
            parameters.addParameters("@PI_EXCEPTION", TypeData.DataType.Varchar, 4000, ParameterDirection.Input, log.Exception ?? (object)DBNull.Value);
            parameters.addParameters("@PI_STACK_TRACE", TypeData.DataType.Varchar, 4000, ParameterDirection.Input, log.StackTrace ?? (object)DBNull.Value);
            parameters.addParameters("@PI_USER_NAME", TypeData.DataType.Varchar, 100, ParameterDirection.Input, log.UserName ?? (object)DBNull.Value);
            parameters.addParameters("@PI_IP_ADDRESS", TypeData.DataType.Varchar, 45, ParameterDirection.Input, log.IpAddress ?? (object)DBNull.Value);
            parameters.addParameters("@PI_REQUEST_PATH", TypeData.DataType.Varchar, 500, ParameterDirection.Input, log.RequestPath ?? (object)DBNull.Value);
            parameters.addParameters("@PI_PAYLOAD", TypeData.DataType.Varchar, 4000, ParameterDirection.Input, log.Payload ?? (object)DBNull.Value);
            parameters.addParameters("@PI_DURATION_MS", TypeData.DataType.Int, 0, ParameterDirection.Input, log.DurationMs ?? (object)DBNull.Value);

            conn.ExecuteSQL(parameters);
        }

        public List<AppLogEntity> Listar(LogFiltroEntity filtro, IConfiguration configuration)
        {
            Connection<AppLogEntity> conn = new(configuration);
            Parameters parameters = new Parameters();

            conn.Devolution = TypeRefund.Register.Entity;
            parameters.NameProcedure = "SP_SEL_APP_LOGS";

            parameters.addParameters("@PI_LEVEL", TypeData.DataType.Varchar, 20, ParameterDirection.Input, filtro.Level ?? (object)DBNull.Value);
            parameters.addParameters("@PI_CATEGORY", TypeData.DataType.Varchar, 100, ParameterDirection.Input, filtro.Category ?? (object)DBNull.Value);
            parameters.addParameters("@PI_EVENT_TYPE", TypeData.DataType.Varchar, 100, ParameterDirection.Input, filtro.EventType ?? (object)DBNull.Value);
            parameters.addParameters("@PI_FECHA_DESDE", TypeData.DataType.DateTime, 0, ParameterDirection.Input, filtro.FechaDesde ?? (object)DBNull.Value);
            parameters.addParameters("@PI_FECHA_HASTA", TypeData.DataType.DateTime, 0, ParameterDirection.Input, filtro.FechaHasta ?? (object)DBNull.Value);

            conn.ExecuteSQL(parameters);

            if (conn.ReturnEntity != null)
            {
                return conn.ReturnEntity.ToList();
            }
            else
            {
                return new List<AppLogEntity>();
            }

        }

        public int Purgar(int diasRetener, IConfiguration configuration)
        {
            Connection<AppLogEntity> conn = new(configuration);
            Parameters parameters = new Parameters();

            conn.Devolution = TypeRefund.Register.Scale;
            parameters.NameProcedure = "SP_PULGARLOGS";

            parameters.addParameters("@DiasRetener", TypeData.DataType.Int, 0, ParameterDirection.Input, diasRetener);

            conn.ExecuteSQL(parameters);

            return conn.ReturnScale != null ? Convert.ToInt32(conn.ReturnScale) : 0;
        }
    }
}