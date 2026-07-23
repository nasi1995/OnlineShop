using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace OnlineShop.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public string FullName { get; set; } = string.Empty;

        public string? Province { get; set; }

        public string? City { get; set; }

        public string? Address { get; set; }

        public string? PostalCode { get; set; }

        public string? Mobile { get; set; }

        public string? ProfileImage { get; set; }
    }
}