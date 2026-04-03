using Microsoft.AspNetCore.Mvc;
using NCOSystems.Entity.Parametro;
using NCOSystems.Entity.Personal;
using NCOSystems.WEB.Helpers;
using NCOSystems.WEB.Models;

namespace NCOSystems.WEB.Controllers
{
    public class ConsultaFormularioController : Controller
    {

        private readonly IConfiguration _configuration;

        public ConsultaFormularioController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public IActionResult Index()
        {
            PersonalViewModel model = new PersonalViewModel();
            BLL.Personal personal = new BLL.Personal();
            
            model.ListaPersonal = personal.ListarPersonal(string.Empty, string.Empty, _configuration);

            return View(model);
        }

        [HttpPost]
        public IActionResult Listar(string rutPersonal, string nombrePersonal)
        {
            PersonalViewModel personalViewModel = new();
            BLL.Personal personal = new();

            try
            {
                rutPersonal = rutPersonal == null ? string.Empty : rutPersonal.Replace(".", "");
                nombrePersonal = nombrePersonal ?? string.Empty;

                    var personalEntities = personal.ListarPersonal(rutPersonal, nombrePersonal, _configuration);
                    foreach (var item in personalEntities)
                    {
                        personalViewModel.ListaPersonal.Add(new PersonalEntity
                        {
                            RutPersonal = GeneralRoutine.FormatearRut(item.RutPersonal!),
                            ApMaternoPersonal = item.ApMaternoPersonal,
                            ApPaternoPersonal = item.ApPaternoPersonal,
                            NombrePersonal = item.NombrePersonal,
                            IdComuna = item.IdComuna,
                            IdRegion = item.IdRegion,
                            NombreComuna = item.NombreComuna,
                            NombreRegion = item.NombreRegion,
                            TelefonoPersonal = item.TelefonoPersonal,
                            IndVigencia = item.IndVigencia,
                            CorreoElectronico = item.CorreoElectronico,
                            IdPersonal = item.IdPersonal
                        });
                    }
               
            }
            catch (Exception ex)
            {
                return PartialView("Mensajeria", new MensajeriaViewModel { IsError = true, Mensaje = ex.Message, Url = "/ConsultaFormulario" });
            }

            return View("Index", personalViewModel);
        }

        public IActionResult GetPersonal(string rutPersonal)
        {
            PersonalViewModel personalViewModel = new();
            BLL.Personal personal = new();
            BLL.Documento personalDocumento = new();

            try
            {
                rutPersonal = rutPersonal == null ? string.Empty : rutPersonal.Replace(".", "");

                var personalEntities = personal.ListarPersonal(rutPersonal, string.Empty, _configuration);
                foreach (var item in personalEntities)
                {
                    
                    personalViewModel.personalHijoEntities = personal.ListarPersonalHijo(item.IdPersonal, _configuration);
                    personalViewModel.personalTipoLicenciaEntities = personal.ListarPersonalTipoLicencia(item.IdPersonal, _configuration);
                    personalViewModel.documentoEntities = personalDocumento.ListarDocumento(item.IdPersonal, _configuration);

                    foreach(DocumentoEntity documentoEntity in personalViewModel.documentoEntities)
                    {
                        documentoEntity.RutaDocumento = Path.Combine("wwwroot", @"Documento\" +  documentoEntity.RutPersonal!.Replace(".", "").Replace("-", ""));
                        documentoEntity.RutaDocumento = Path.Combine(documentoEntity.RutaDocumento, documentoEntity.NombreDocumento!);
                    }

                    personalViewModel.RutPersonal = GeneralRoutine.FormatearRut(item.RutPersonal!);
                    personalViewModel.ApMaternoPersonal = item.ApMaternoPersonal;
                    personalViewModel.ApPaternoPersonal = item.ApPaternoPersonal;
                    personalViewModel.NombrePersonal = item.NombrePersonal;
                    personalViewModel.IdComuna = item.IdComuna;
                    personalViewModel.IdRegion = item.IdRegion;
                    personalViewModel.NombreComuna = item.NombreComuna;
                    personalViewModel.NombreRegion = item.NombreRegion;
                    personalViewModel.TelefonoPersonal = item.TelefonoPersonal;
                    personalViewModel.IndVigencia = item.IndVigencia;
                    personalViewModel.CorreoElectronico = item.CorreoElectronico;
                    personalViewModel.IdPersonal = item.IdPersonal;
                }

            }
            catch (Exception ex)
            {
                return PartialView("Mensajeria", new MensajeriaViewModel { IsError = true, Mensaje = ex.Message, Url = "/ConsultaFormulario" });
            }

            return PartialView("_FormularioIngreso", personalViewModel);
        }
    }
}
