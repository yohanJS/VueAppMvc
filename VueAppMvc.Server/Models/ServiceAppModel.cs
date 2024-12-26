using System.ComponentModel.DataAnnotations;

namespace VueAppMvc.Server.Models
{
    public class ServiceAppModel
    {
        public int Id { get; set; }

        [MaxLength(50)] // Limit Service to 50 characters
        public string Service { get; set; } = string.Empty;

        [MaxLength(10)] // Limit Date to 10 characters (e.g., "YYYY-MM-DD")
        public string Date { get; set; } = string.Empty;

        [MaxLength(8)] // Limit Time to 8 characters (e.g., "HH:mm AM/PM")
        public string Time { get; set; } = string.Empty;

        public List<UserModel> Users { get; set; } = new List<UserModel>();
    }
}
