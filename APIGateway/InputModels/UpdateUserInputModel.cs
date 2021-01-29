using System.ComponentModel.DataAnnotations;

namespace WebMonitoringApi.InputModels
{
    public class UpdateUserInputModel
    {
        public string CurrentUserName { get; set; }
        public string NewUserName { get; set; }

        [EmailAddress]
        public string CurrentEmail { get; set; }

        [EmailAddress]
        public string NewEmail { get; set; }

        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
    }
}
