using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace TourDev.Models
{
    public class User : IdentityUser
    {
        public int Id { get; set; }
        [Required]
        public string Login { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
