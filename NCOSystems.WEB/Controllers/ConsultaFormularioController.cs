using ClosedXML.Excel;
using FluentFTP;
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

        // ============================================================
        //  INDEX
        // ============================================================
        public IActionResult Index()
        {
            PersonalViewModel model = new PersonalViewModel();
            BLL.Personal personal = new BLL.Personal();

            model.ListaPersonal = personal.ListarPersonal(string.Empty, string.Empty, _configuration);

            return View(model);
        }

        // ============================================================
        //  LISTAR (POST)
        // ============================================================
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

        // ============================================================
        //  GET PERSONAL — carga el formulario principal (PartialView)
        // ============================================================
        public IActionResult GetPersonal(string rutPersonal)
        {
            PersonalViewModel personalViewModel = new();
            BLL.Personal personal = new();
            BLL.PersonalHijo personalHijo = new();
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
                    personalViewModel.personalHijoEntities = personalHijo.ListarPersonalHijo(item.IdPersonal, _configuration);
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

        // ============================================================
        //  AJAX — obtener comunas por región
        // ============================================================
        [HttpGet]
        public JsonResult GetComuna(int idRegion)
        {
            BLL.Parametro parametro = new BLL.Parametro();
            var listadoComuna = parametro.ListarComuna(idRegion, _configuration);
            return Json(listadoComuna);
        }

        // ============================================================
        //  AJAX — validar RUT
        // ============================================================
        public JsonResult ValidarRut(string rutPersonal)
        {
            BLL.Personal personal = new BLL.Personal();
            bool existe = false;

            var listadoPersonal = personal.ListarPersonal(rutPersonal.Replace(".", ""), string.Empty, _configuration);
            if (listadoPersonal.Count > 0)
                existe = true;

            return Json(new { existe });
        }

        // ============================================================
        //  EXPORTAR EXCEL
        // ============================================================
        [HttpGet]
        public async Task<IActionResult> ExportarExcel(string rutPersonal, string nombrePersonal)
        {
            BLL.Personal personal = new BLL.Personal();

            var rut = rutPersonal ?? string.Empty;
            var nombre = nombrePersonal ?? string.Empty;

            var personalEntities = personal.ListarPersonal(rut.Replace(".", ""), nombre, _configuration);

            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Personal");

            worksheet.Cell(1, 1).Value = "N°";
            worksheet.Cell(1, 2).Value = "RUT";
            worksheet.Cell(1, 3).Value = "Nombre";
            worksheet.Cell(1, 4).Value = "Comuna";
            worksheet.Cell(1, 5).Value = "Teléfono";
            worksheet.Cell(1, 6).Value = "Fecha Ingreso";

            var headerRange = worksheet.Range("A1:F1");
            headerRange.Style.Font.Bold = true;
            headerRange.Style.Fill.BackgroundColor = XLColor.FromHtml("#2E75B6");
            headerRange.Style.Font.FontColor = XLColor.White;
            headerRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            int fila = 2;
            foreach (var persona in personalEntities)
            {
                worksheet.Cell(fila, 1).Value = persona.Correlativo;
                worksheet.Cell(fila, 2).Value = persona.RutPersonal;
                worksheet.Cell(fila, 3).Value = persona.NombreCompletoPersonal;
                worksheet.Cell(fila, 4).Value = persona.NombreComuna;
                worksheet.Cell(fila, 5).Value = persona.TelefonoPersonal;
                worksheet.Cell(fila, 6).Value = persona.FecIngreso;
                fila++;
            }

            worksheet.Columns().AdjustToContents();

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            stream.Position = 0;

            string fileName = $"Personal_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";
            return File(stream.ToArray(),
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        fileName);
        }

        [HttpPost]
        public IActionResult EditFormulario(PersonalViewModel personalViewModel)
        {
            BLL.Personal personal = new BLL.Personal();

            try
            {
                PersonalEntity personalEntity = new PersonalEntity
                {
                    IdPersonal = personalViewModel.IdPersonal,
                    RutPersonal = personalViewModel.RutPersonal,
                    NombrePersonal = personalViewModel.NombrePersonal,
                    ApPaternoPersonal = personalViewModel.ApPaternoPersonal,
                    ApMaternoPersonal = personalViewModel.ApMaternoPersonal,
                    CorreoElectronico = personalViewModel.CorreoElectronico,
                    TelefonoPersonal = personalViewModel.TelefonoPersonal,
                    IdRegion = personalViewModel.IdRegion,
                    IdComuna = personalViewModel.IdComuna,
                    IdEstadoCivil = personalViewModel.IdEstadoCivil,
                    IdEstadoLaboral = personalViewModel.IdEstadoLaboral,
                    IdGenero = personalViewModel.IdGenero,
                    IdUsuario = "ADMIN"
                };

                personal.Actualizar(personalEntity, _configuration);
            }
            catch (Exception ex)
            {
                return PartialView("Mensajeria", new MensajeriaViewModel { IsError = true, Mensaje = ex.Message, Url = "/ConsultaFormulario" });
            }

            return PartialView("Mensajeria", new MensajeriaViewModel { IsError = false, Mensaje = "Se actualizo registro correctamente", Url = "/ConsultaFormulario" });

        }

        // ############################################################
        //
        //   H I J O S
        //
        // ############################################################

        // ---- Crear Hijo (GET) ----
        [HttpGet]
        public IActionResult CreateHijo(int idPersonal, string rutPersonal)
        {
            var model = new PersonalHijoEntity
            {
                IdPersonal = idPersonal,
            };
            return PartialView("_CreateHijoModal", model);
        }

        // ---- Crear Hijo (POST) ----
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateHijo(PersonalHijoEntity model)
        {
            if (!ModelState.IsValid) return View(model);

            try
            {
                BLL.PersonalHijo personalHijo = new BLL.PersonalHijo();
                model.IdUsuario = "ADMIN";
                personalHijo.InsertarHijo(model, _configuration);
                TempData["Mensaje"] = "Hijo agregado correctamente.";
            }
            catch (Exception ex)
            {
                return PartialView("Mensajeria", new MensajeriaViewModel { IsError = true, Mensaje = ex.Message, Url = "/ConsultaFormulario" });
            }

            return Json(new { success = true, mensaje = "Hijo guardado correctamente." });
        }

        // ---- Editar Hijo (GET) ----
        [HttpGet]
        public IActionResult EditHijo(int idPersonalHijo, int idPersonal)
        {
            try
            {
                BLL.PersonalHijo personalHijo = new BLL.PersonalHijo();
                var hijos = personalHijo.ListarPersonalHijo(idPersonal, _configuration);
                var hijo = hijos.FirstOrDefault(h => h.IdPersonalHijo == idPersonalHijo);

                if (hijo == null) hijo = new PersonalHijoEntity();

                return PartialView("_EditHijoModal", hijo); // <-- PartialView
            }
            catch (Exception ex)
            {
                return PartialView("Mensajeria", new MensajeriaViewModel
                {
                    IsError = true,
                    Mensaje = ex.Message,
                    Url = "/ConsultaFormulario"
                });
            }
        }

        // ---- Editar Hijo (POST) ----
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditHijo(PersonalHijoEntity model)
        {
            try
            {
                BLL.PersonalHijo personalHijo = new BLL.PersonalHijo();
                model.IdUsuario = "ADMIN";
                personalHijo.ActualizarHijo(model, _configuration); // ajusta al método que tengas
                return Json(new { success = true, mensaje = "Hijo actualizado correctamente." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // ---- Eliminar Hijo (AJAX POST) ----
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult DeleteHijo(int id)
        {
            try
            {
                BLL.PersonalHijo personalHijo = new BLL.PersonalHijo();
                personalHijo.EliminarHijo(id, _configuration);
                return Json(new { success = true, mensaje = "Registro eliminado correctamente." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }


        // ############################################################
        //
        //   L I C E N C I A S
        //
        // ############################################################

        // ---- Crear Licencia (GET) ----
        [HttpGet]
        public IActionResult CreateLicencia(int idPersonal)
        {
            var model = new PersonalTipoLicenciaEntity
            {
                IdPersonal = idPersonal,
            };

            BLL.Parametro parametro = new BLL.Parametro();

            model.tipoLicenciaEntities = parametro.ListarTipoLicencia(_configuration);

            return PartialView("_CreateLicenciaModal", model);
        }

        // ---- Crear Licencia (POST) ----
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateLicencia(PersonalTipoLicenciaEntity model)
        {
            if (!ModelState.IsValid) return View(model);

            try
            {
                BLL.PersonalTipoLicencia personal = new BLL.PersonalTipoLicencia();

                model.IdUsuario = "ADMIN";

                personal.InsertarPersonalTipoLicencia(model, _configuration);

                TempData["Mensaje"] = "Licencia agregada correctamente.";
            }
            catch (Exception ex)
            {
                return PartialView("Mensajeria", new MensajeriaViewModel { IsError = true, Mensaje = ex.Message, Url = "/ConsultaFormulario" });
            }

            return Json(new { success = true, mensaje = "Licencia agregada correctamente." });
        }

        // ---- Editar Licencia (GET) ----
        [HttpGet]
        public IActionResult EditLicencia(int IdPersonalTipoLicencia, int idPersonal)
        {
            try
            {
                BLL.Personal personal = new BLL.Personal();
                BLL.Parametro parametro = new BLL.Parametro();
                var licencias = personal.ListarPersonalTipoLicencia(idPersonal, _configuration);
                var licencia = licencias.FirstOrDefault(l => l.IdPersonalTipoLicencia == IdPersonalTipoLicencia);

                if (licencia == null) licencia = new PersonalTipoLicenciaEntity();

                licencia.tipoLicenciaEntities = parametro.ListarTipoLicencia(_configuration);

                return PartialView("_EditLicenciaModal", licencia); // <-- PartialView
            }
            catch (Exception ex)
            {
                return PartialView("Mensajeria", new MensajeriaViewModel { IsError = true, Mensaje = ex.Message, Url = "/ConsultaFormulario" });
            }
        }

        // ---- Editar Licencia (POST) ----
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditLicencia(PersonalTipoLicenciaEntity model)
        {
            try
            {
                BLL.PersonalTipoLicencia personal = new BLL.PersonalTipoLicencia();
                model.IdUsuario = "ADMIN";
                personal.ActualizarPersonalTipoLicencia(model, _configuration);
                TempData["Mensaje"] = "Licencia actualizada correctamente.";
            }
            catch (Exception ex)
            {
                return PartialView("Mensajeria", new MensajeriaViewModel
                {
                    IsError = true,
                    Mensaje = ex.Message,
                    Url = "/ConsultaFormulario"
                });
            }
            return Json(new { success = true, mensaje = "Licencia actualizada correctamente." });
        }

        // ---- Eliminar Licencia (AJAX POST) ----
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult DeleteLicencia(int id)
        {
            try
            {
                BLL.PersonalTipoLicencia personal = new BLL.PersonalTipoLicencia();

                personal.EliminarPersonalTipoLicencia(id, _configuration);

                return Json(new { success = true, mensaje = "Licencia eliminada correctamente." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }


        // ############################################################
        //
        //   D O C U M E N T O S
        //
        // ############################################################

        // ---- Crear Documento (GET) ----
        [HttpGet]
        public IActionResult CreateDocumento(int idPersonal, string rutPersonal)
        {
            CargarTiposDocumento();
            var model = new DocumentoEntity
            {
                IdPersonal = idPersonal,
                RutPersonal = rutPersonal
            };

            BLL.Parametro parametro = new BLL.Parametro();
            model.tipoDocumentoEntities = parametro.ListarTipoDocumento(_configuration);

            return PartialView("_CreateDocumentoModal", model);
        }

        // ---- Crear Documento (POST) ----
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateDocumento(DocumentoEntity model, IFormFile archivo)
        {
            BLL.Documento personalDocumento = new BLL.Documento();

            if (!ModelState.IsValid) return View(model);

            var listadoDocumentos = personalDocumento.ListarDocumento(model.IdPersonal, _configuration);

            // En el controller
            if (listadoDocumentos.Any(d => d.IdTipoDocumento == model.IdTipoDocumento))
            {
                return Json(new { success = false, mensaje = "No se puede agregar el mismo Tipo Documento." });
            }

            try
            {
                //Conexión FTP fuera del foreach para no reconectar en cada iteración
                using (var cliente = new AsyncFtpClient(
                    _configuration["FTP:Host"],
                    _configuration["FTP:Usuario"],
                    _configuration["FTP:Password"]))
                {
                    //Configuración FTPS(FTP Seguro)
                    //cliente.Config.EncryptionMode = FtpEncryptionMode.Explicit;
                    cliente.Config.EncryptionMode = FtpEncryptionMode.None;
                    cliente.Config.ValidateAnyCertificate = true; // Cambiar a false en producción con certificado válido

                    await cliente.Connect();

                    var carpetaRemota = _configuration["FTP:RutaBase"] + "/" + model.RutPersonal!.Replace(".", "").Replace("-", "");

                    //Leer el archivo en memoria antes de enviarlo por FTP
                    using var memoryStream = new MemoryStream();
                    await archivo!.CopyToAsync(memoryStream);
                    memoryStream.Position = 0;

                    var rutaArchivoRemoto = $"{carpetaRemota}/{archivo.FileName}";

                    try
                    {

                        //Subir archivo al FTP
                        var resultado = await cliente.UploadStream(
                            memoryStream,
                            rutaArchivoRemoto,
                            FtpRemoteExists.Overwrite,
                            createRemoteDir: true
                        );

                        if (resultado == FtpStatus.Failed)
                            throw new Exception($"Error al subir el archivo {archivo.FileName} al FTP.");

                        //Guardar en BD solo si el FTP fue exitoso
                        personalDocumento.Insertar(new DocumentoEntity
                        {
                            IdDocumento = model.IdDocumento,
                            IdPersonal = model.IdPersonal,
                            IdTipoDocumento = model.IdTipoDocumento,
                            NombreDocumento = archivo.FileName,
                            IdUsuario = "ADMIN"
                        }, _configuration);
                    }
                    catch (Exception ex)
                    {
                        //Rollback: si el archivo se subió al FTP pero falló la BD, eliminarlo del FTP
                        if (await cliente.FileExists(rutaArchivoRemoto))
                            await cliente.DeleteFile(rutaArchivoRemoto);

                        //Puedes loggear el error o relanzar la excepción según tu necesidad
                        throw new Exception($"Error procesando el archivo {archivo.FileName}: {ex.Message}", ex);
                    }

                    await cliente.Disconnect();
                }

                TempData["Mensaje"] = "Documento agregado correctamente.";
            }
            catch (Exception ex)
            {
                return PartialView("Mensajeria", new MensajeriaViewModel { IsError = true, Mensaje = ex.Message, Url = "/ConsultaFormulario" });
            }

            return Json(new { success = true, mensaje = "Registro guardado correctamente." });
        }

        // ---- Editar Documento (GET) ----
        [HttpGet]
        public IActionResult EditDocumento(int idDocumento, int idPersonal)
        {
            try
            {
                BLL.Documento personalDocumento = new BLL.Documento();
                var documentos = personalDocumento.ListarDocumento(idPersonal, _configuration);
                var documento = documentos.FirstOrDefault(d => d.IdDocumento == idDocumento);

                if (documento == null) documento = new DocumentoEntity();

                CargarTiposDocumento(documento.IdTipoDocumento);
                return PartialView("_EditDocumentoModal", documento);
            }
            catch (Exception ex)
            {
                return PartialView("Mensajeria", new MensajeriaViewModel { IsError = true, Mensaje = ex.Message, Url = "/ConsultaFormulario" });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditDocumento(DocumentoEntity model, IFormFile archivo)
        {
            try
            {
                BLL.Documento personalDocumento = new BLL.Documento();

                //Conexión FTP fuera del foreach para no reconectar en cada iteración
                using (var cliente = new AsyncFtpClient(
                    _configuration["FTP:Host"],
                    _configuration["FTP:Usuario"],
                    _configuration["FTP:Password"]))
                {
                    //Configuración FTPS(FTP Seguro)
                    //cliente.Config.EncryptionMode = FtpEncryptionMode.Explicit;
                    cliente.Config.EncryptionMode = FtpEncryptionMode.None;
                    cliente.Config.ValidateAnyCertificate = true; // Cambiar a false en producción con certificado válido

                    await cliente.Connect();

                    var carpetaRemota = _configuration["FTP:RutaBase"] + "/" + model.RutPersonal!.Replace(".", "").Replace("-", "");

                    //Leer el archivo en memoria antes de enviarlo por FTP
                    using var memoryStream = new MemoryStream();
                    await archivo!.CopyToAsync(memoryStream);
                    memoryStream.Position = 0;

                    var rutaArchivoRemoto = $"{carpetaRemota}/{archivo.FileName}";

                    try
                    {
                        // Se elimina el archivo anterior y reemplazarlo por el nuevo a cargar.
                        if (await cliente.FileExists($"{carpetaRemota}/{model.NombreDocumento}"))
                            await cliente.DeleteFile($"{carpetaRemota}/{model.NombreDocumento}");

                        //Subir archivo al FTP
                        var resultado = await cliente.UploadStream(
                            memoryStream,
                            rutaArchivoRemoto,
                            FtpRemoteExists.Overwrite,
                            createRemoteDir: true
                        );

                        if (resultado == FtpStatus.Failed)
                            throw new Exception($"Error al subir el archivo {archivo.FileName} al FTP.");

                        //Guardar en BD solo si el FTP fue exitoso
                        personalDocumento.Actualizar(new DocumentoEntity
                        {
                            IdDocumento = model.IdDocumento,
                            IdPersonal = model.IdPersonal,
                            IdTipoDocumento = model.IdTipoDocumento,
                            NombreDocumento = archivo.FileName,
                            IdUsuario = "ADMIN"
                        }, _configuration);
                    }
                    catch (Exception ex)
                    {
                        //Rollback: si el archivo se subió al FTP pero falló la BD, eliminarlo del FTP
                        if (await cliente.FileExists(rutaArchivoRemoto))
                            await cliente.DeleteFile(rutaArchivoRemoto);

                        //Puedes loggear el error o relanzar la excepción según tu necesidad
                        throw new Exception($"Error procesando el archivo {archivo.FileName}: {ex.Message}", ex);
                    }

                    await cliente.Disconnect();
                }

            }
            catch (Exception ex)
            {
                return PartialView("Mensajeria", new MensajeriaViewModel { IsError = true, Mensaje = ex.Message, Url = "/ConsultaFormulario" });
            }

            return Json(new { success = true, mensaje = "Documento actualizado correctamente." });
        }

        // ---- Eliminar Documento (AJAX POST) ----
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> DeleteDocumento(int id)
        {
            try
            {
                BLL.Documento documentoBLL = new BLL.Documento();

                // 1. Obtener datos del documento ANTES de eliminar (para tener RutPersonal y NombreDocumento)
                var documento = documentoBLL.ObtenerDocumento(id, _configuration);

                // 2. Eliminar de la BD
                documentoBLL.Eliminar(id, _configuration);

                // 3. Eliminar archivo del FTP (solo si se obtuvieron los datos)
                if (documento != null && !string.IsNullOrEmpty(documento.NombreDocumento) && !string.IsNullOrEmpty(documento.RutPersonal))
                {
                    using (var cliente = new AsyncFtpClient(
                        _configuration["FTP:Host"],
                        _configuration["FTP:Usuario"],
                        _configuration["FTP:Password"]))
                    {
                        cliente.Config.EncryptionMode = FtpEncryptionMode.None;
                        cliente.Config.ValidateAnyCertificate = true;

                        await cliente.Connect();

                        var carpetaRemota = _configuration["FTP:RutaBase"] + "/" +
                                            documento.RutPersonal.Replace(".", "").Replace("-", "");

                        var rutaArchivoRemoto = $"{carpetaRemota}/{documento.NombreDocumento}";

                        if (await cliente.FileExists(rutaArchivoRemoto))
                            await cliente.DeleteFile(rutaArchivoRemoto);

                        await cliente.Disconnect();
                    }
                }

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // ============================================================
        //  Helper privado — carga tipos de documento en ViewBag
        // ============================================================
        private void CargarTiposDocumento(int idSeleccionado = 0)
        {
            BLL.Parametro parametro = new BLL.Parametro();
            ViewBag.TiposDocumento = parametro.ListarTipoDocumento(_configuration)
                .Select(t => new SelectListItem
                {
                    Value = t.IdTipoDocumento.ToString(),
                    Text = t.NombreTipoDocumento,
                    Selected = t.IdTipoDocumento == idSeleccionado
                }).ToList();
        }
    }
}
