using System;
using System.Collections.Generic;
using System.Text;

namespace Microservices.Models
{
    public class UpdateUser
    {
        public string CurrentUserName { get; set; }

        public string NewUserName { get; set; }

        public string CurrentEmail { get; set; }

        public string NewEmail { get; set; }

        public string CurrentPassword { get; set; }

        public string NewPassword { get; set; }
    }
}
