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
        public List<UserModel>? Users { get; set; }
    }
}
