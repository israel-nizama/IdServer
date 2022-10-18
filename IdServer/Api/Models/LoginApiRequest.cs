using System.ComponentModel.DataAnnotations;

namespace IdServer.Api.Models
{
    public class LoginApiRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
        [Required]
        public string ClientId { get; set; }
    }
}
