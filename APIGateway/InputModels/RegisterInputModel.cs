using System.ComponentModel.DataAnnotations;

namespace WebMonitoringApi.InputModels
{
    public class RegisterInputModel
    {
        [MaxLength(100)]
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }

        [EmailAddress]
        [Required]
        public string Email { get; set; }
    }
}
