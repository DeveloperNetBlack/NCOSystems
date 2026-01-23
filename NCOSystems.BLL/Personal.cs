using Microsoft.Extensions.Configuration;
using NCOSystems.Entity.Personal;
using System;
using System.Collections.Generic;
using System.Text;

namespace NCOSystems.BLL
{
    public class Personal
    {
        public void Insertar(PersonalEntity personalEntity, IConfiguration configuration)
        {
            DAL.Personal personal = new DAL.Personal();
            personal.Insertar(personalEntity, configuration);
        }

        public void Actualizar(PersonalEntity personalEntity, IConfiguration configuration)
        {
            DAL.Personal personal = new DAL.Personal();
            personal.Actualizar(personalEntity, configuration);
        }

        public void InsertarHijo(PersonalHijoEntity personalHijoEntity, IConfiguration configuration)
        {
            DAL.Personal personal = new DAL.Personal();
            personal.InsertarHijo(personalHijoEntity, configuration);
        }

        public List<PersonalEntity> ListarPersonal(string rutPersonal, string nombrePersonal, IConfiguration configuration)
        {
            DAL.Personal personal = new DAL.Personal();
            return personal.ListarPersonal(rutPersonal, nombrePersonal, configuration);
        }

        public List<PersonalHijoEntity> ListarPersonalHijo(int idPersonal, IConfiguration configuration)
        {
            DAL.Personal personal = new DAL.Personal();
            return personal.ListarPersonalHijo(idPersonal, configuration);
        }
    }
}
