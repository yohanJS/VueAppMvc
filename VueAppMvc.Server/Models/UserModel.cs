using System.ComponentModel.DataAnnotations;

namespace VueAppMvc.Server.Models
{
    public class UserModel
    {
        public int Id { get; set; }

        [MaxLength(100)] // Limit Name to 100 characters
        public string Name { get; set; } = string.Empty;

        [MaxLength(150)] // Limit Email to 150 characters
        public string Email { get; set; } = string.Empty;

        [MaxLength(15)] // Limit Phone to 15 characters (e.g., +1 (123) 456-7890)
        public string Phone { get; set; } = string.Empty;

        [MaxLength(100)] // Limit Street to 100 characters
        public string Street { get; set; } = string.Empty;

        [MaxLength(50)] // Limit City to 50 characters
        public string City { get; set; } = string.Empty;

        [MaxLength(20)] // Limit State to 20 characters
        public string State { get; set; } = string.Empty;

        [MaxLength(10)] // Limit Zip to 10 characters
        public string Zip { get; set; } = string.Empty;

        public List<ServiceAppModel> Services { get; set; } = new List<ServiceAppModel>();
    }
}
