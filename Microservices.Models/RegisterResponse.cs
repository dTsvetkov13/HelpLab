using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;

namespace Microservices.Models
{
    public class RegisterResponse : IdentityResult
    {
        [JsonProperty]
        public new bool Succeeded { get; set; }

        [JsonProperty]
        public new IEnumerable<IdentityError> Errors { get; set; }
    }
}
