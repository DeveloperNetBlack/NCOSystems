using Microsoft.Extensions.Configuration;
using NCOSystems.Entity.Personal;

namespace NCOSystems.BLL
{
    public class PersonalHijo
    {
        public void InsertarHijo(List<PersonalHijoEntity> personalHijoEntity, int idPersonal, IConfiguration configuration)
        {
            DAL.PersonalHijo personal = new DAL.PersonalHijo();

            foreach (var item in personalHijoEntity)
            {
                item.IdPersonal = idPersonal;
                item.IdUsuario = "ADMIN";
                personal.InsertarHijo(item, configuration);
            }
        }

        public void InsertarHijo(PersonalHijoEntity personalHijoEntity, IConfiguration configuration)
        {
            DAL.PersonalHijo personal = new DAL.PersonalHijo();

            personal.InsertarHijo(personalHijoEntity, configuration);
        }

        public void ActualizarHijo(PersonalHijoEntity personalHijoEntity, IConfiguration configuration)
        {
            DAL.PersonalHijo personal = new DAL.PersonalHijo();

            personal.ActualizarHijo(personalHijoEntity, configuration);
        }

        public void EliminarHijo(int idPersonalHijo, IConfiguration configuration)
        {
            DAL.PersonalHijo personal = new DAL.PersonalHijo();

            personal.EliminarHijo(idPersonalHijo, configuration);
        }

        public List<PersonalHijoEntity> ListarPersonalHijo(int idPersonal, IConfiguration configuration)
        {
            DAL.PersonalHijo personal = new DAL.PersonalHijo();
            return personal.ListarPersonalHijo(idPersonal, configuration);
        }
    }
}
