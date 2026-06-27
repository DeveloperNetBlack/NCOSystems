using Microsoft.AspNetCore.Mvc;
using NCOSystems.Entity.Log;

namespace NCOSystems.WEB.Controllers
{
    public class AppLogController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly BLL.AppLog _log;

        public AppLogController(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _configuration = configuration;
            _log = new BLL.AppLog(configuration, httpContextAccessor);
        }

        public IActionResult Index(LogFiltroEntity filtro)
        {
            try
            {
                var logs = _log.Listar(filtro);
                ViewBag.Filtro = filtro;
                return View(logs);
            }
            catch (Exception ex)
            {
                return Content("ERROR: " + ex.Message);
            }
        }

        [HttpGet]
        public JsonResult Detalle(long id)
        {
            try
            {
                var logs = _log.Listar(new LogFiltroEntity());
                var log = logs.FirstOrDefault(x => x.Id == id);
                return Json(new { isError = false, data = log });
            }
            catch (Exception ex)
            {
                return Json(new { isError = true, mensaje = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult Purgar(int diasRetener)
        {
            try
            {
                int eliminados = _log.Purgar(diasRetener);

                _log.Info($"Purga de logs ejecutada manualmente",
                    eventType: "PURGA_LOGS",
                    category: "AppLog",
                    payload: new { diasRetener, eliminados });

                return Json(new { isError = false, mensaje = $"Se eliminaron {eliminados} registros anteriores a {diasRetener} días." });
            }
            catch (Exception ex)
            {
                return Json(new { isError = true, mensaje = ex.Message });
            }
        }
    }
}