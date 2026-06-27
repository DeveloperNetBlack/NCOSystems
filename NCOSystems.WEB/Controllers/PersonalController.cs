using FluentFTP;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
        private readonly BLL.AppLog _log;

        public PersonalController(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _configuration = configuration;
            _log = new BLL.AppLog(configuration, httpContextAccessor);
        }

        public IActionResult Index()
        {
            try
            {
                PersonalViewModel model = TempData.Get<PersonalViewModel>("PersonalData");
                BLL.Parametro parametro = new BLL.Parametro();

                if (model == null)
                    model = new PersonalViewModel();

                var numeros = _configuration.GetSection("WhatsApp:Numeros").Get<List<string>>() ?? new List<string>();
                ViewBag.WhatsAppNumeros = JsonSerializer.Serialize(numeros);

                model.regionEntities = parametro.ListarRegion(_configuration);
                model.tipoLicenciaEntities = parametro.ListarTipoLicencia(_configuration);
                model.tipoDocumentoEntities = parametro.ListarTipoDocumento(_configuration);
                model.estadoCivilEntities = parametro.ListarEstadoCivil(_configuration);
                model.estadoLaboralEntities = parametro.ListarEstadoLaboral(_configuration);
                model.generoEntities = parametro.ListarGenero(_configuration);
                model.paisEntities = parametro.ListarPais(_configuration);

                model.personalTipoLicenciaEntities = new List<PersonalTipoLicenciaEntity>();
                model.IdPais = 40;

                ViewBag.ListaComuna = new List<SelectListItem>();

                return View(model);
            }
            catch (Exception ex)
            {
                _log.Error("Error al cargar Index de Personal", ex,
                    eventType: "ERROR_INDEX",
                    category: "Personal");

                return Content("ERROR: " + ex.Message + " | " + ex.InnerException?.Message);
            }
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
            PersonalViewModel model = TempData.Get<PersonalViewModel>("PersonalData");
            model.personalTipoLicenciaEntities.RemoveAll(x => x.IdPersonalTipoLicencia == Convert.ToInt32(idPersonalTipoLicencia));
            return Json(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string personalData, string datoPersonalTipoLicencia,
            string datoPersonalHijo, [FromForm] List<TipoDocumentoEntity> documentos)
        {
            int idPersonal = 0;
            string rutPersonal = string.Empty;
            var sw = System.Diagnostics.Stopwatch.StartNew();
            BLL.Documento documentoBLL = new BLL.Documento();

            try
            {
                // Validación documentos
                if (!documentos.Any() || documentos.Any(x => x.Archivo == null))
                {
                    _log.Warning("Intento de grabación sin documentos adjuntos",
                        eventType: "VALIDACION_DOC",
                        category: "Personal");

                    return Json(new { isError = true, mensaje = "Debe adjuntar todos los documentos", url = "/Personal" });
                }

                // Grabar datos personales
                idPersonal = Grabar(personalData, datoPersonalTipoLicencia, datoPersonalHijo, out rutPersonal);

                // Proceso FTP
                var carpetaRut = rutPersonal.Replace(".", "").Replace("-", "");

                using (var cliente = new AsyncFtpClient(
                    _configuration["FTP:Host"],
                    _configuration["FTP:Usuario"],
                    _configuration["FTP:Password"]))
                {
                    cliente.Config.EncryptionMode = FtpEncryptionMode.Explicit;
                    cliente.Config.ValidateAnyCertificate = false;

                    await cliente.Connect();

                    _log.Info("Conexión FTP establecida",
                        eventType: "FTP_CONNECT",
                        category: "FTP",
                        payload: new { Host = _configuration["FTP:Host"], RutPersonal = rutPersonal });

                    foreach (var doc in documentos)
                    {
                        var carpetaRemota = $"{_configuration["FTP:RutaBase"]}/{carpetaRut}";
                        var rutaArchivoRemoto = $"{carpetaRemota}/{doc.Archivo!.FileName}";

                        using var memoryStream = new MemoryStream();
                        await doc.Archivo.CopyToAsync(memoryStream);
                        memoryStream.Position = 0;

                        try
                        {
                            if (!await cliente.DirectoryExists(carpetaRemota))
                                await cliente.CreateDirectory(carpetaRemota);

                            var resultado = await cliente.UploadStream(
                                memoryStream,
                                rutaArchivoRemoto,
                                FtpRemoteExists.Overwrite,
                                createRemoteDir: true
                            );

                            if (resultado == FtpStatus.Failed)
                                throw new Exception($"Error al subir el archivo {doc.Archivo.FileName} al FTP.");

                            _log.Info("Archivo subido al FTP correctamente",
                                eventType: "FTP_UPLOAD_OK",
                                category: "FTP",
                                payload: new { doc.Archivo.FileName, rutaArchivoRemoto, idPersonal });

                            // Guardar en BD solo si FTP fue exitoso
                            documentoBLL.Insertar(new DocumentoEntity
                            {
                                IdPersonal = idPersonal,
                                IdTipoDocumento = doc.IdTipoDocumento,
                                NombreDocumento = doc.Archivo.FileName,
                                IdUsuario = "ADMIN"
                            }, _configuration);

                            _log.Info("Documento registrado en BD",
                                eventType: "INSERT_DOCUMENTO",
                                category: "FTP",
                                payload: new { doc.Archivo.FileName, idPersonal, doc.IdTipoDocumento });
                        }
                        catch (Exception ex)
                        {
                            // Rollback FTP si falló
                            if (await cliente.FileExists(rutaArchivoRemoto))
                            {
                                await cliente.DeleteFile(rutaArchivoRemoto);

                                _log.Warning("Rollback FTP ejecutado: archivo eliminado por error en BD",
                                    eventType: "FTP_ROLLBACK",
                                    category: "FTP",
                                    payload: new { doc.Archivo.FileName, rutaArchivoRemoto, idPersonal });
                            }

                            _log.Error($"Error procesando archivo {doc.Archivo.FileName}", ex,
                                eventType: "FTP_UPLOAD_ERROR",
                                category: "FTP",
                                payload: new { doc.Archivo.FileName, rutaArchivoRemoto, idPersonal });

                            throw new Exception($"Error procesando el archivo {doc.Archivo.FileName}: {ex.Message}", ex);
                        }
                    }

                    await cliente.Disconnect();

                    _log.Info("Conexión FTP cerrada correctamente",
                        eventType: "FTP_DISCONNECT",
                        category: "FTP",
                        payload: new { RutPersonal = rutPersonal, TotalDocumentos = documentos.Count });
                }

                sw.Stop();

                _log.Info("Proceso Create completado exitosamente",
                    eventType: "CREATE_OK",
                    category: "Personal",
                    payload: new { idPersonal, rutPersonal, DurationMs = sw.ElapsedMilliseconds });
            }
            catch (Exception ex)
            {
                sw.Stop();

                _log.Error("Error general en Create de Personal", ex,
                    eventType: "CREATE_ERROR",
                    category: "Personal",
                    payload: new { rutPersonal, DurationMs = sw.ElapsedMilliseconds });

                return Json(new { isError = true, mensaje = ex.Message, url = "/Personal" });
            }

            return Json(new { isError = false, mensaje = "Datos grabados exitosamente", url = "/Personal" });
        }

        private int Grabar(string personalData, string personalTipoLicencia, string personalHijo, out string rutPersonal)
        {
            int idPersonal = 0;
            PersonalEntity personalEntity = new PersonalEntity();
            BLL.Personal personalBLL = new BLL.Personal();
            BLL.PersonalHijo personalHijoBLL = new BLL.PersonalHijo();
            BLL.PersonalTipoLicencia personalTipo = new BLL.PersonalTipoLicencia();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                Converters =
                {
                    new StringToIntConverter(),
                    new StringToDateTimeConverter()
                }
            };

            // Deserializar persona
            var persona = JsonSerializer.Deserialize<PersonalViewModel>(personalData, options);

            if (persona == null)
            {
                _log.Warning("Deserialización de personalData retornó null",
                    eventType: "DESERIALIZE_NULL",
                    category: "Grabar",
                    payload: new { personalData });

                throw new Exception("No se pudo deserializar los datos del personal.");
            }

            personalEntity.IdComuna = persona.IdComuna;
            personalEntity.RutPersonal = persona.RutPersonal!.Replace(".", "").ToUpper();
            personalEntity.NombrePersonal = persona.NombrePersonal!.ToUpper();
            personalEntity.ApPaternoPersonal = persona.ApPaternoPersonal!.ToUpper();
            personalEntity.ApMaternoPersonal = persona.ApMaternoPersonal!.ToUpper();
            personalEntity.TelefonoPersonal = persona.TelefonoPersonal;
            personalEntity.CorreoElectronico = persona.CorreoElectronico;
            personalEntity.IdEstadoCivil = persona.IdEstadoCivil;
            personalEntity.IdEstadoLaboral = persona.IdEstadoLaboral;
            personalEntity.IdGenero = persona.IdGenero;
            personalEntity.IdPais = persona.IdPais;
            personalEntity.FecNacimiento = persona.FecNacimiento;
            personalEntity.Direccion = persona.Direccion!.ToUpper();
            personalEntity.IndVigencia = 1;
            personalEntity.IdUsuario = "ADMIN";

            // Insertar persona principal
            try
            {
                idPersonal = personalBLL.Insertar(personalEntity, _configuration);

                _log.Info("Personal insertado correctamente",
                    eventType: "INSERT_PERSONAL",
                    category: "Grabar",
                    payload: new { idPersonal, personalEntity.RutPersonal, personalEntity.NombrePersonal });
            }
            catch (Exception ex)
            {
                _log.Error("Error al insertar personal en BD", ex,
                    eventType: "ERROR_INSERT_PERSONAL",
                    category: "Grabar",
                    payload: new { personalEntity.RutPersonal, personalEntity.NombrePersonal });

                throw;
            }

            // Insertar tipos de licencia
            try
            {
                var tipoLicencia = JsonSerializer.Deserialize<List<PersonalTipoLicenciaEntity>>(personalTipoLicencia, options);

                personalTipo.InsertarPersonalTipoLicencia(tipoLicencia!, idPersonal, _configuration);

                _log.Info("Tipos de licencia insertados correctamente",
                    eventType: "INSERT_TIPO_LICENCIA",
                    category: "Grabar",
                    payload: new { idPersonal, cantidad = tipoLicencia?.Count ?? 0 });
            }
            catch (Exception ex)
            {
                _log.Error("Error al insertar tipos de licencia", ex,
                    eventType: "ERROR_INSERT_TIPO_LICENCIA",
                    category: "Grabar",
                    payload: new { idPersonal });

                throw;
            }

            // Insertar hijos
            try
            {
                var hijoPersonal = JsonSerializer.Deserialize<List<PersonalHijoEntity>>(personalHijo, options);

                personalHijoBLL.InsertarHijo(hijoPersonal!, idPersonal, _configuration);

                _log.Info("Hijos insertados correctamente",
                    eventType: "INSERT_HIJOS",
                    category: "Grabar",
                    payload: new { idPersonal, cantidad = hijoPersonal?.Count ?? 0 });
            }
            catch (Exception ex)
            {
                _log.Error("Error al insertar hijos del personal", ex,
                    eventType: "ERROR_INSERT_HIJOS",
                    category: "Grabar",
                    payload: new { idPersonal });

                throw;
            }

            rutPersonal = personalEntity.RutPersonal;

            return idPersonal;
        }

        public JsonResult ValidarRut(string rutPersonal)
        {
            BLL.Personal personal = new BLL.Personal();
            bool existe = false;

            var listadoPersonal = personal.ListarPersonal(rutPersonal.Replace(".", ""), string.Empty, _configuration);

            if (listadoPersonal.Count > 0)
                existe = true;

            return Json(new { existe });
        }
    }
}