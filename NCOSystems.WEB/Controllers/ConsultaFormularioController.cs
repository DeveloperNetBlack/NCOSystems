using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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

        [HttpGet]
        public JsonResult GetComuna(int idRegion)
        {
            BLL.Parametro parametro = new BLL.Parametro();

            var listadoComuna = parametro.ListarComuna(idRegion, _configuration);

            return Json(listadoComuna);
        }

        [HttpPost]
        public IActionResult Listar(string rutPersona, string nombrePersonaCompleto)
        {
            PersonalViewModel personalViewModel = new();
            BLL.Personal personal = new();

            try
            {
                rutPersona = rutPersona == null ? string.Empty : rutPersona.Replace(".", "");
                nombrePersonaCompleto = nombrePersonaCompleto ?? string.Empty;

                var personalEntities = personal.ListarPersonal(rutPersona, nombrePersonaCompleto, _configuration);
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
                        IdEstadoCivil = item.IdEstadoCivil,
                        IdEstadoLaboral = item.IdEstadoLaboral,
                        IdGenero = item.IdGenero,
                        IdPersonal = item.IdPersonal,
                        Correlativo = item.Correlativo
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
            BLL.Parametro parametro = new BLL.Parametro();

            ViewBag.ListaComuna = new List<SelectListItem>();

            personalViewModel.regionEntities = parametro.ListarRegion(_configuration);
            personalViewModel.estadoCivilEntities = parametro.ListarEstadoCivil(_configuration);
            personalViewModel.estadoLaboralEntities = parametro.ListarEstadoLaboral(_configuration);
            personalViewModel.generoEntities = parametro.ListarGenero(_configuration);

            try
            {
                rutPersonal = rutPersonal == null ? string.Empty : rutPersonal.Replace(".", "");

                var personalEntities = personal.ListarPersonal(rutPersonal, string.Empty, _configuration);
                foreach (var item in personalEntities)
                {

                    personalViewModel.personalHijoEntities = personal.ListarPersonalHijo(item.IdPersonal, _configuration);
                    personalViewModel.personalTipoLicenciaEntities = personal.ListarPersonalTipoLicencia(item.IdPersonal, _configuration);
                    personalViewModel.documentoEntities = personalDocumento.ListarDocumento(item.IdPersonal, _configuration);

                    foreach (DocumentoEntity documentoEntity in personalViewModel.documentoEntities)
                    {
                        documentoEntity.RutaDocumento = _configuration["FTP:RutaBaseVer"] + "/" + rutPersonal.Replace(".", "").Replace("-", "") + "/";
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
                    personalViewModel.IdEstadoCivil = item.IdEstadoCivil;
                    personalViewModel.IdEstadoLaboral = item.IdEstadoLaboral;
                    personalViewModel.IdGenero = item.IdGenero;
                }

            }
            catch (Exception ex)
            {
                return PartialView("Mensajeria", new MensajeriaViewModel { IsError = true, Mensaje = ex.Message, Url = "/ConsultaFormulario" });
            }

            return PartialView("_FormularioIngreso", personalViewModel);
        }

        public JsonResult ValidarRut(string rutPersonal)
        {
            BLL.Personal personal = new BLL.Personal();
            bool existe = false;

            var listadoPersonal = personal.ListarPersonal(rutPersonal.Replace(".", ""), string.Empty, _configuration);

            if (listadoPersonal.Count > 0)
            {
                existe = true;
            }

            return Json(new { existe });
        }

        [HttpGet]
        public async Task<IActionResult> ExportarExcel(string rutPersonal, string nombrePersonal)
        {
            BLL.Personal personal = new BLL.Personal();

            var rut = rutPersonal ?? string.Empty;
            var nombre = nombrePersonal ?? string.Empty;

            // Obtén la misma lista que usas en el Grid
            var personalEntities = personal.ListarPersonal(rut.Replace(".", ""), nombre, _configuration);

            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Personal");

            // Encabezados
            worksheet.Cell(1, 1).Value = "N°";
            worksheet.Cell(1, 2).Value = "RUT";
            worksheet.Cell(1, 3).Value = "Nombre";
            worksheet.Cell(1, 4).Value = "Comuna";
            worksheet.Cell(1, 5).Value = "Teléfono";

            // Estilo encabezados
            var headerRange = worksheet.Range("A1:E1");
            headerRange.Style.Font.Bold = true;
            headerRange.Style.Fill.BackgroundColor = XLColor.FromHtml("#2E75B6");
            headerRange.Style.Font.FontColor = XLColor.White;
            headerRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            // Datos
            int fila = 2;
            foreach (var persona in personalEntities)
            {
                worksheet.Cell(fila, 1).Value = persona.Correlativo;
                worksheet.Cell(fila, 2).Value = persona.RutPersonal;
                worksheet.Cell(fila, 3).Value = persona.NombreCompletoPersonal;
                worksheet.Cell(fila, 4).Value = persona.NombreComuna;
                worksheet.Cell(fila, 5).Value = persona.TelefonoPersonal;
                fila++;
            }

            // Ajustar ancho de columnas automáticamente
            worksheet.Columns().AdjustToContents();

            // Retornar el archivo
            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            stream.Position = 0;

            string fileName = $"Personal_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";
            return File(stream.ToArray(),
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        fileName);
        }
    }
}
