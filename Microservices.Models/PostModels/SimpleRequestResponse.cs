using System;
using System.Collections.Generic;
using System.Text;

namespace Microservices.Models.PostModels
{
    public class SimpleRequestResponse
    {
        public string Error { set; get; }

        public bool IsError { set; get; }

        public bool Succeeded { set; get; }
    }
}
