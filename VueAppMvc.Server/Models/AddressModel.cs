using System.ComponentModel.DataAnnotations;

namespace VueAppMvc.Server.Models
{
    public class AddressModel
    {
        public int Id { get; set; }
        [MaxLength(100)] // Limit Street to 100 characters
        public string Street { get; set; } = string.Empty;

        [MaxLength(50)] // Limit City to 50 characters
        public string City { get; set; } = string.Empty;

        [MaxLength(20)] // Limit State to 20 characters
        public string State { get; set; } = string.Empty;

        [MaxLength(10)] // Limit Zip to 10 characters
        public string Zip { get; set; } = string.Empty;
    }
}
