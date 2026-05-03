using Microsoft.Extensions.Configuration;
using NCOSystems.Entity.Personal;
using System;
using System.Collections.Generic;
using System.Text;

namespace NCOSystems.BLL
{
    public class PersonalTipoLicencia
    {
        public void InsertarPersonalTipoLicencia(List<PersonalTipoLicenciaEntity> personalTipoLicenciaEntity, int idPersonal, IConfiguration configuration)
        {
            DAL.PersonalTipoLicencia personal = new DAL.PersonalTipoLicencia();

            foreach (var item in personalTipoLicenciaEntity)
            {
                item.IdPersonal = idPersonal;
                item.IdUsuario = "ADMIN";
                personal.InsertarPersonalTipoLicencia(item, configuration);
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
