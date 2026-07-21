namespace NCOSystems.WEB.Models
{
    public class ClientErrorModel
    {
        public string? Mensaje { get; set; }
        public string? Detalle { get; set; }
        public string? UrlOrigen { get; set; }
        public string? UserAgent { get; set; }
    }
}
