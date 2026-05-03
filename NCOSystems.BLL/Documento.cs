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

        public void Actualizar(DocumentoEntity documentoEntity, IConfiguration configuration)
        {
            DAL.Documento documento = new DAL.Documento();
            documento.Actualizar(documentoEntity, configuration);
        }

        public void Eliminar(int idDocumento, IConfiguration configuration)
        {
            DAL.Documento documento = new DAL.Documento();
            documento.Eliminar(idDocumento, configuration);
        }

        public List<DocumentoEntity> ListarDocumento(int idPersona, IConfiguration configuration)
        {
            DAL.Documento documento = new DAL.Documento();
            return documento.ListarDocumento(idPersona, configuration);
        }

        public DocumentoEntity ObtenerDocumento(int idDocumento, IConfiguration configuration)
        {
            DAL.Documento documento = new DAL.Documento();
            return documento.ObtenerDocumento(idDocumento, configuration);
        }
    }
}
