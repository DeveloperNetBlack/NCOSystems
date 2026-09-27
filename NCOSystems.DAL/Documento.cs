using Knotus.NET10.DB.SQLServer;
using Microsoft.Extensions.Configuration;
using NCOSystems.Entity.Parametro;
using System.Data;

namespace NCOSystems.DAL
{
    public class Documento
    {
        public void Insertar(DocumentoEntity documentoEntity, IConfiguration configuration)
        {
            Connection<DocumentoEntity> conn = new(configuration);
            Parameters parameters = new Parameters();

            conn.Devolution = TypeRefund.Register.None;

            parameters.NameProcedure = "SP_INS_DOCUMENTO";

            parameters.AddParameter("PI_ID_PERSONAL", TypeData.DataType.Int, 0, ParameterDirection.Input, documentoEntity.IdPersonal);
            parameters.AddParameter("PI_ID_TIPO_DOCUMENTO", TypeData.DataType.Int, 0, ParameterDirection.Input, documentoEntity.IdTipoDocumento);
            parameters.AddParameter("PI_NOMBRE_DOCUMENTO", TypeData.DataType.Varchar, 80, ParameterDirection.Input, documentoEntity.NombreDocumento!);
            parameters.AddParameter("PI_ID_USUARIO", TypeData.DataType.Varchar, 30, ParameterDirection.Input, documentoEntity.IdUsuario!);

            conn.ExecuteSQL(parameters);

        }

        public void Actualizar(DocumentoEntity documentoEntity, IConfiguration configuration)
        {
            Connection<DocumentoEntity> conn = new(configuration);
            Parameters parameters = new Parameters();

            conn.Devolution = TypeRefund.Register.None;

            parameters.NameProcedure = "SP_UPD_DOCUMENTO";

            parameters.AddParameter("PI_ID_DOCUMENTO", TypeData.DataType.Int, 0, ParameterDirection.Input, documentoEntity.IdDocumento);
            parameters.AddParameter("PI_ID_PERSONAL", TypeData.DataType.Int, 0, ParameterDirection.Input, documentoEntity.IdPersonal);
            parameters.AddParameter("PI_ID_TIPO_DOCUMENTO", TypeData.DataType.Int, 0, ParameterDirection.Input, documentoEntity.IdTipoDocumento);
            parameters.AddParameter("PI_NOMBRE_DOCUMENTO", TypeData.DataType.Varchar, 80, ParameterDirection.Input, documentoEntity.NombreDocumento!);
            parameters.AddParameter("PI_ID_USUARIO", TypeData.DataType.Varchar, 30, ParameterDirection.Input, documentoEntity.IdUsuario!);

            conn.ExecuteSQL(parameters);

        }

        public void Eliminar(int idDocumento, IConfiguration configuration)
        {
            Connection<DocumentoEntity> conn = new(configuration);
            Parameters parameters = new Parameters();

            conn.Devolution = TypeRefund.Register.None;

            parameters.NameProcedure = "SP_DEL_DOCUMENTO";

            parameters.AddParameter("PI_ID_DOCUMENTO", TypeData.DataType.Int, 0, ParameterDirection.Input, idDocumento);

            conn.ExecuteSQL(parameters);

        }

        public List<DocumentoEntity> ListarDocumento(int idPersona, IConfiguration configuration)
        {
            Connection<DocumentoEntity> conn = new(configuration);
            Parameters parameters = new Parameters();

            conn.Devolution = TypeRefund.Register.Entity;

            parameters.NameProcedure = "SP_SEL_DOCUMENTO";

            parameters.AddParameter("PI_ID_PERSONAL", TypeData.DataType.Int, 0, ParameterDirection.Input, idPersona);

            conn.ExecuteSQL(parameters);

            if (conn.ReturnEntity != null)
            {
                return conn.ReturnEntity.ToList();
            }
            else
            {
                return new List<DocumentoEntity>();
            }
        }

        public DocumentoEntity ObtenerDocumento(int idDocumento, IConfiguration configuration)
        {
            Connection<DocumentoEntity> conn = new(configuration);
            Parameters parameters = new Parameters();

            conn.Devolution = TypeRefund.Register.EntitySingle;

            parameters.NameProcedure = "SP_SEL_DOCUMENTO_ID";

            parameters.AddParameter("PI_ID_DOCUMENTO", TypeData.DataType.Int, 0, ParameterDirection.Input, idDocumento);

            conn.ExecuteSQL(parameters);

            if (conn.ReturnEntity != null)
            {
                return conn.ReturnEntitySingle!;
            }
            else
            {
                return new DocumentoEntity();
            }
        }
    }
}
