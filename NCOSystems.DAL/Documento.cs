using CompileIT.NET9.DB.SQLServer;
using Microsoft.Extensions.Configuration;
using NCOSystems.Entity.Parametro;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

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

            parameters.addParameters("@PI_ID_PERSONAL", TypeData.DataType.Int, 0, ParameterDirection.Input, documentoEntity.IdPersona);
            parameters.addParameters("@PI_ID_TIPO_DOCUMENTO", TypeData.DataType.Int, 0, ParameterDirection.Input, documentoEntity.IdTipoDocumento);
            parameters.addParameters("@PI_NOMBRE_DOCUMENTO", TypeData.DataType.Varchar, 80, ParameterDirection.Input, documentoEntity.NombreDocumento!);
            parameters.addParameters("@PI_ID_USUARIO", TypeData.DataType.Varchar, 30, ParameterDirection.Input, documentoEntity.IdUsuario);

            conn.ExecuteSQL(parameters);

        }

        public List<DocumentoEntity> ListarDocumento(int idPersona, IConfiguration configuration)
        {
            Connection<DocumentoEntity> conn = new(configuration);
            Parameters parameters = new Parameters();

            conn.Devolution = TypeRefund.Register.Entity;

            parameters.NameProcedure = "SP_SEL_DOCUMENTO";

            parameters.addParameters("@PI_ID_PERSONAL", TypeData.DataType.Int, 0, ParameterDirection.Input, idPersona);

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
    }
}
