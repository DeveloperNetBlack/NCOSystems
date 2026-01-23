using Microsoft.Extensions.Configuration;
using NCOSystems.Entity.Parametro;
using System;
using System.Collections.Generic;
using System.Text;

namespace NCOSystems.BLL
{
    public class Documento
    {
        public void Insertar(DocumentoEntity documentoEntity, IConfiguration configuration)
        {
            DAL.Documento documento = new DAL.Documento();
            documento.Insertar(documentoEntity, configuration);
        }

        public List<DocumentoEntity> ListarDocumento(int idPersona, IConfiguration configuration)
        {
            DAL.Documento documento = new DAL.Documento();
            return documento.ListarDocumento(idPersona, configuration);
        }
    }
}
