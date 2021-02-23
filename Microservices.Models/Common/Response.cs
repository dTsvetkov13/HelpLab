using System;
using System.Collections.Generic;
using System.Text;

namespace Microservices.Models.Common
{
    public class Response
    {
        public Statuses Status { set; get; }

        public string Error { set; get; }
    }
}
