namespace NCOSystems.Entity.Log
{
    public class LogFiltroEntity
    {
        public string? Level { get; set; }
        public string? Category { get; set; }
        public string? EventType { get; set; }
        public DateTime? FechaDesde { get; set; }
        public DateTime? FechaHasta { get; set; }
    }
}