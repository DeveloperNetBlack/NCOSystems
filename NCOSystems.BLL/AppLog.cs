// AppLog.cs
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using NCOSystems.Entity.Log;
using System.Text.Json;

namespace NCOSystems.BLL
{
    public class AppLog
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _http;

        public AppLog(IConfiguration configuration, IHttpContextAccessor http)
        {
            _configuration = configuration;
            _http = http;
        }

        public void Info(string message, string? eventType = null, string? category = null, object? payload = null)
            => Registrar("Information", message, eventType, category, payload, null);

        public void Warning(string message, string? eventType = null, string? category = null, object? payload = null)
            => Registrar("Warning", message, eventType, category, payload, null);

        public void Error(string message, Exception? ex = null, string? eventType = null, string? category = null, object? payload = null)
            => Registrar("Error", message, eventType, category, payload, ex);

        private void Registrar(string level, string message, string? eventType, string? category, object? payload, Exception? ex)
        {
            try
            {
                var ctx = _http.HttpContext;

                var log = new AppLogEntity
                {
                    Level = level,
                    Category = category,
                    EventType = eventType,
                    Message = message,
                    Exception = ex?.Message,
                    StackTrace = ex?.StackTrace,
                    UserName = ctx?.User?.Identity?.Name ?? "ADMIN",
                    IpAddress = ctx?.Connection?.RemoteIpAddress?.ToString(),
                    RequestPath = ctx?.Request?.Path.ToString(),
                    Payload = payload != null ? JsonSerializer.Serialize(payload) : null,
                    CreatedAt = DateTime.Now
                };

                new DAL.AppLogDAL().Insertar(log, _configuration);
            }
            catch
            {
                // El log nunca debe romper el flujo principal
            }
        }

        public List<AppLogEntity> Listar(LogFiltroEntity filtro)
        {
            return new DAL.AppLogDAL().Listar(filtro, _configuration);
        }

        public int Purgar(int diasRetener)
        {
            return new DAL.AppLogDAL().Purgar(diasRetener, _configuration);
        }
    }
}