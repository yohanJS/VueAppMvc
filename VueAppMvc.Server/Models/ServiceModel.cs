﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace VueAppMvc.Server.Models
{
    public class ServiceModel
    {
        public int Id { get; set; }

        // Foreign key to User
        public int UserId { get; set; }

        [MaxLength(50)] // Limit Service to 50 characters
        public string Service { get; set; } = string.Empty;

        [MaxLength(10)] // Limit Date to 10 characters (e.g., "YYYY-MM-DD")
        public string Date { get; set; } = string.Empty;

        [MaxLength(8)] // Limit Time to 8 characters (e.g., "HH:mm AM/PM")
        public string Time { get; set; } = string.Empty;

        // Navigation property
        [ForeignKey("UserId")]
        [JsonIgnore]
        public UserModel? User { get; set; }

        // Navigation property
        [ForeignKey("BusinessId")]
        [JsonIgnore]
        public string? BusinessId { get; set; }
    }
}
