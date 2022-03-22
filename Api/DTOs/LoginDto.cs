using System.ComponentModel.DataAnnotations;

namespace Api.DTOs
{
    public class LoginDto
    {
        [Required]           //for Validation
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}