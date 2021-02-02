using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microservices.Models
{
    public class UpdateResponse : IdentityResult
    {
        [JsonProperty]
        public new bool Succeeded { get; set; }

        [JsonProperty]
        public new IEnumerable<IdentityError> Errors { get; set; }
    }
}
