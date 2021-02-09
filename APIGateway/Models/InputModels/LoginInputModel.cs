using System.ComponentModel.DataAnnotations;

namespace WebMonitoringApi.InputModels
{
    public class LoginInputModel
    {
        [MaxLength(100)]
        [Required(ErrorMessage = "Email is required")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
    }
}
