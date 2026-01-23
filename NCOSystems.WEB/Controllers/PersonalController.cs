using GridMvc.Server;
using GridShared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NCOSystems.Entity.Parametro;
using NCOSystems.Entity.Personal;
using NCOSystems.WEB.Models;

namespace NCOSystems.WEB.Controllers
{
    public class PersonalController : Controller
    {
        private readonly IConfiguration _configuration;

        public PersonalController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            // Obtener modelo de la memoria
            PersonalViewModel model = TempData.Get<PersonalViewModel>("PersonalData");
            BLL.Parametro parametro = new BLL.Parametro();

            // Chequea si el modelo existe
            if (model == null)
            {
                model = new PersonalViewModel();
            }

            model.regionEntities = parametro.ListarRegion(_configuration);
            model.tipoLicenciaEntities = parametro.ListarTipoLicencia(_configuration);
            model.tipoDocumentoEntities = parametro.ListarTipoDocumento(_configuration);
            model.FechaLicenciaClaseB = DateTime.Today.ToShortDateString();

            model.personalTipoLicenciaEntities = new List<PersonalTipoLicencia>();

            foreach(var item in model.tipoLicenciaEntities)
            {
                model.personalTipoLicenciaEntities.Add(new PersonalTipoLicencia
                {
                    IdPersonal = 0,
                    IdTipoLicencia = item.IdTipoLicencia,
                    NombreClaseLicencia = item.NombreTipo,
                    FechaVctoLicencia = null
                });
            }

            ViewBag.ListaComuna = new List<SelectListItem>();

            TempData.Put("PersonalData", model);

            return View(model);
        }

        [HttpGet]
        public JsonResult GetComuna(int idRegion)
        {
            BLL.Parametro parametro = new BLL.Parametro();
            
            var listadoComuna = parametro.ListarComuna(idRegion, _configuration);

            return Json(listadoComuna);
        }

        [HttpPost]
        public JsonResult AgregarLicencia(string fecVctoLicencia, string tipoLicencia)
        {
            // Obtener modelo de la memoria
            PersonalViewModel model = TempData.Get<PersonalViewModel>("PersonalData");
            int idFila = 0;
            string? nombreTipoLicencia = string.Empty;

            // Chequea si el modelo existe
            if (model == null)
            {
                model = new PersonalViewModel();
            }

            if(model.personalTipoLicenciaEntities == null)
            {
                model.personalTipoLicenciaEntities = new List<PersonalTipoLicencia>();
            }

            if(model.personalTipoLicenciaEntities.Count > 0)
            {
                idFila = model.personalTipoLicenciaEntities.Max(x => x.IdPersonalTipoLicencia) + 1;
            }
            else
            {
                idFila = 0;
            }

            nombreTipoLicencia = model.tipoLicenciaEntities.Where(x => x.IdTipoLicencia == Convert.ToInt32(tipoLicencia)).Select(x => x.NombreTipo).FirstOrDefault();

            model.personalTipoLicenciaEntities.Add(new PersonalTipoLicencia
            {
                IdTipoLicencia = Convert.ToInt32(tipoLicencia),
                FechaVctoLicencia = Convert.ToDateTime(fecVctoLicencia),
                IdPersonalTipoLicencia = idFila,
                NombreClaseLicencia = nombreTipoLicencia
            });

            TempData.Put("PersonalData", model);

            return Json(model);
        }

        [HttpPost]
        public JsonResult EliminarLicencia(string idPersonalTipoLicencia)
        {
            // Obtener modelo de la memoria
            PersonalViewModel model = TempData.Get<PersonalViewModel>("PersonalData");

            model.personalTipoLicenciaEntities.RemoveAll(x => x.IdPersonalTipoLicencia == Convert.ToInt32(idPersonalTipoLicencia));

            TempData.Put("PersonalData", model);

            return Json(model);
        }

        [HttpPost]
        public IActionResult Create(IFormCollection formulario)
        {
            PersonalViewModel model = TempData.Get<PersonalViewModel>("PersonalData");

            return View();
        }
    }
}