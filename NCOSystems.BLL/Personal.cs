using Microsoft.Extensions.Configuration;
using NCOSystems.Entity.Personal;
using System;
using System.Collections.Generic;
using System.Text;

namespace NCOSystems.BLL
{
    public class Personal
    {
        public int Insertar(PersonalEntity personalEntity, IConfiguration configuration)
        {
            DAL.Personal personal = new DAL.Personal();
            
            return personal.Insertar(personalEntity, configuration);
        }

        public void Actualizar(PersonalEntity personalEntity, IConfiguration configuration)
        {
            DAL.Personal personal = new DAL.Personal();
            personal.Actualizar(personalEntity, configuration);
        }



        public List<PersonalEntity> ListarPersonal(string rutPersonal, string nombrePersonal, IConfiguration configuration)
        {
            DAL.Personal personal = new DAL.Personal();
            return personal.ListarPersonal(rutPersonal, nombrePersonal, configuration);
        }

        public List<PersonalTipoLicenciaEntity> ListarPersonalTipoLicencia(int idPersonal, IConfiguration configuration)
        {
            DAL.PersonalTipoLicencia personal = new DAL.PersonalTipoLicencia();

            return personal.ListarPersonalTipoLicencia(idPersonal, configuration);
        }
    }
}
