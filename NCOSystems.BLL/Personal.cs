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

        public void InsertarHijo(List<PersonalHijoEntity> personalHijoEntity, int idPersonal, IConfiguration configuration)
        {
            DAL.Personal personal = new DAL.Personal();

            foreach (var item in personalHijoEntity)
            {
                item.IdPersonal = idPersonal;
                item.IdUsuario = "ADMIN";
                personal.InsertarHijo(item, configuration);
            }
        }

        public void InsertarPersonalTipoLicencia(List<PersonalTipoLicenciaEntity> personalTipoLicenciaEntity, int idPersonal,  IConfiguration configuration)
        {
            DAL.Personal personal = new DAL.Personal();

            foreach (var item in personalTipoLicenciaEntity)
            {
                item.IdPersonal = idPersonal;
                item.IdUsuario = "ADMIN";
                personal.InsertarPersonalTipoLicencia(item, configuration);
            }
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
