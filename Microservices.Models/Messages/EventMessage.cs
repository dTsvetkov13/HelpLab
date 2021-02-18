using System;
using System.Collections.Generic;
using System.Text;

namespace Microservices.Models.Messages
{
    public class EventMessage
    {
        public EventMessage(EventsEnum EventObject, string userId)
        {
            Event = EventObject;
            UserId = userId;
        }

        public EventsEnum Event { get; set; }

        public string UserId { get; set; }
    }
}
