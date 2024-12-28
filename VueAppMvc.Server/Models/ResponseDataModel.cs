namespace VueAppMvc.Server.Models
{
    public class ResponseDataModel
    {
        public string? ServiceDate { get; set; }
        public List<Service>? Services { get; set; }
    }

    public class Service
    {
        public string? Time { get; set; }
        public string? Name { get; set; }
        public int? ServiceId { get; set; }
        public string? ServiceName { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
    }
}
