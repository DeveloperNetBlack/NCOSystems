using GridMvc.Server;
using GridShared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient.DataClassification;
using NCOSystems.Entity.Parametro;
using NCOSystems.Entity.Personal;
using NCOSystems.WEB.Helpers;
using NCOSystems.WEB.Models;
using System.Text.Json;

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
            //model.FechaLicenciaClaseB = DateTime.Today.ToShortDateString();

            model.personalTipoLicenciaEntities = new List<PersonalTipoLicenciaEntity>();

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
        public JsonResult EliminarLicencia(string idPersonalTipoLicencia)
        {
            // Obtener modelo de la memoria
            PersonalViewModel model = TempData.Get<PersonalViewModel>("PersonalData");

            model.personalTipoLicenciaEntities.RemoveAll(x => x.IdPersonalTipoLicencia == Convert.ToInt32(idPersonalTipoLicencia));

            TempData.Put("PersonalData", model);

            return Json(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string personalData, string datoPersonalTipoLicencia, string datoPersonalHijo, [FromForm] List<TipoDocumentoEntity> documentos)
        {
            int idPersonal = 0;
            string rutPersonal = string.Empty;  
            BLL.Documento documentoBLL = new BLL.Documento();

            try
            {

                idPersonal = Grabar(personalData, datoPersonalTipoLicencia, datoPersonalHijo, out rutPersonal);

                var archivosGuardados = new List<object>();
                var errores = new List<string>();

                foreach (var doc in documentos)
                {

                    var carpeta = Path.Combine("wwwroot", @"Documento\" + rutPersonal.Replace(".", "").Replace("-", ""));

                    if (!Directory.Exists(carpeta))
                        Directory.CreateDirectory(carpeta);

                    var rutaCompleta = Path.Combine(carpeta, doc.Archivo!.FileName);

                    using (var stream = new FileStream(rutaCompleta, FileMode.Create))
                    {
                        await doc.Archivo.CopyToAsync(stream);
                    }

                    // Aquí puedes guardar en BD:
                    documentoBLL.Insertar(new DocumentoEntity
                    {
                        IdPersona = idPersonal,
                        IdTipoDocumento = doc.IdTipoDocumento,
                        NombreDocumento = doc.Archivo.FileName,
                        IdUsuario = "ADMIN"
                    }, _configuration);
                }

            }
            catch (Exception ex) 
            {
                return Json(new { isError = true, mensaje = ex.Message, url = "/Personal" });
            }

            return Json(new { isError = false, mensaje = "Datos grabados exitosamente", url = "/Personal" });
        }

        private int Grabar(string personalData, string personalTipoLicencia, string personalHijo, out string rutPersonal)
        {
            int idPersonal = 0;
            PersonalEntity personalEntity = new PersonalEntity();
            BLL.Personal personalBLL = new BLL.Personal();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                Converters =
                        {
                            new StringToIntConverter(),
                            new StringToDateTimeConverter()
                        }
            };

            var persona = JsonSerializer.Deserialize<PersonalViewModel>(personalData, options);

            personalEntity.IdComuna = persona!.IdComuna;
            personalEntity.RutPersonal = persona.RutPersonal!.Replace(".","").ToUpper();
            personalEntity.NombrePersonal = persona.NombrePersonal!.ToUpper();
            personalEntity.ApellidoPaternoPersonal = persona.ApellidoPaternoPersonal!.ToUpper();
            personalEntity.ApellidoMaternoPersonal = persona.ApellidoMaternoPersonal!.ToUpper();
            personalEntity.TelefonoPersonal = persona.TelefonoPersonal;
            personalEntity.CorreoElectronico = persona.CorreoElectronico;
            personalEntity.FecLicenciaB = persona.FecLicenciaB;
            personalEntity.IndVigencia = 1;
            personalEntity.IdUsuario = "ADMIN";

            idPersonal = personalBLL.Insertar(personalEntity, _configuration);

            var tipoLicencia = JsonSerializer.Deserialize<List<PersonalTipoLicenciaEntity>>(personalTipoLicencia, options);

            personalBLL.InsertarPersonalTipoLicencia(tipoLicencia!, idPersonal, _configuration);

            var hijoPersonal = JsonSerializer.Deserialize<List<PersonalHijoEntity>>(personalHijo, options);

            personalBLL.InsertarHijo(hijoPersonal!, idPersonal, _configuration);

            rutPersonal = personalEntity.RutPersonal;

            return idPersonal;
        }

        public JsonResult ValidarRut(string rutPersonal)
        {
            BLL.Personal personal = new BLL.Personal();
            bool existe = false;

            var listadoPersonal = personal.ListarPersonal(rutPersonal.Replace(".",""), string.Empty, _configuration);

            if(listadoPersonal.Count > 0)
            {
                existe = true;
            }

            return Json(new { existe });
        }
    }
}