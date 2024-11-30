using System.ComponentModel.DataAnnotations;

namespace VueAppMvc.Server.Models
{
    public class BookingModel
    {
        public int Id { get; set; }

        [MaxLength(100)] // Limit Name to 100 characters
        public string Name { get; set; } = string.Empty;

        [MaxLength(150)] // Limit Email to 150 characters
        public string Email { get; set; } = string.Empty;

        [MaxLength(15)] // Limit Phone to 15 characters (e.g., +1 (123) 456-7890)
        public string Phone { get; set; } = string.Empty;

        public AddressModel Address { get; set; } = new AddressModel();

        [MaxLength(50)] // Limit Service to 50 characters
        public string Service { get; set; } = string.Empty;

        [MaxLength(10)] // Limit Date to 10 characters (e.g., "YYYY-MM-DD")
        public string Date { get; set; } = string.Empty;

        [MaxLength(8)] // Limit Time to 8 characters (e.g., "HH:mm AM/PM")
        public string Time { get; set; } = string.Empty;
    }
}
