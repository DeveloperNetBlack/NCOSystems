using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using NCOSystems.Entity.Personal;

namespace NCOSystems.BLL
{
    public class PersonalTipoLicencia
    {

        public void InsertarPersonalTipoLicencia(List<PersonalTipoLicenciaEntity> personalTipoLicenciaEntity, int idPersonal, IConfiguration configuration, AppLog _log)
        {
            DAL.PersonalTipoLicencia personal = new DAL.PersonalTipoLicencia();
           
            foreach (var item in personalTipoLicenciaEntity)
            {
                try
                {
                    item.IdPersonal = idPersonal;
                    item.IdUsuario = "ADMIN";
                    personal.InsertarPersonalTipoLicencia(item, configuration);
                }
                catch (Exception ex)
                {
                    _log.Error("Error al insertar tipos de licencia", ex,
                                eventType: "ERROR_INSERT_TIPO_LICENCIA",
                                category: "Grabar",
                                payload: new { idPersonal, item.IdPersonalTipoLicencia, item.FecVctoLicencia, item.FecOtorgamiento });
                }
            }
        }

        public void InsertarPersonalTipoLicencia(PersonalTipoLicenciaEntity personalTipoLicenciaEntity, IConfiguration configuration)
        {
            DAL.PersonalTipoLicencia personal = new DAL.PersonalTipoLicencia();

            personal.InsertarPersonalTipoLicencia(personalTipoLicenciaEntity, configuration);
        }

        public void ActualizarPersonalTipoLicencia(PersonalTipoLicenciaEntity personalTipoLicenciaEntity, IConfiguration configuration)
        {
            DAL.PersonalTipoLicencia personal = new DAL.PersonalTipoLicencia();

            personal.ActualizarPersonalTipoLicencia(personalTipoLicenciaEntity, configuration);
        }

        public void EliminarPersonalTipoLicencia(int idPersonalTipoLicencia, IConfiguration configuration)
        {
            DAL.PersonalTipoLicencia personal = new DAL.PersonalTipoLicencia();

            personal.EliminarPersonalTipoLicencia(idPersonalTipoLicencia, configuration);
        }

        public List<PersonalTipoLicenciaEntity> ListarPersonalTipoLicencia(int idPersonal, IConfiguration configuration)
        {
            DAL.PersonalTipoLicencia personal = new DAL.PersonalTipoLicencia();

            return personal.ListarPersonalTipoLicencia(idPersonal, configuration);
        }
    }
}
