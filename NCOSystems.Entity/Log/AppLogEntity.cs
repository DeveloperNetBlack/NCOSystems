// AppLogEntity.cs
namespace NCOSystems.Entity.Log
{
    public class AppLogEntity
    {
        public long Id { get; set; }
        public string Level { get; set; } = "Information";
        public string? Category { get; set; }
        public string? EventType { get; set; }
        public string Message { get; set; } = string.Empty;
        public string? Exception { get; set; }
        public string? StackTrace { get; set; }
        public string? UserName { get; set; }
        public string? IpAddress { get; set; }
        public string? RequestPath { get; set; }
        public string? Payload { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public int? DurationMs { get; set; }
    }
}