using System.ComponentModel.DataAnnotations;

namespace WebMonitoringApi.InputModels
{
    public class LoginInputModel
    {
        [MaxLength(100)]
        public string UserName { get; set; }

        public string Password { get; set; }
    }
}
