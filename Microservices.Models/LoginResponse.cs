using System;
using System.Collections.Generic;
using System.Text;

namespace Microservices.Models
{
    public class LoginResponse
    {
        public string AccessToken { get; set; }
       
        public string IdentityToken { get; set; }
        
        public string Scope { get; set; }
        
        public string IssuedTokenType { get; set; }
        
        public string TokenType { get; set; }
        
        public string RefreshToken { get; set; }
        
        public string ErrorDescription { get; set; }
        
        public int ExpiresIn { get; set; }

        public bool IsError { get; set; }

        public string Error { get; set; }
    }
}
