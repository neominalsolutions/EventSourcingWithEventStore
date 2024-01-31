using EventSourcing_Example_With_EventStore.EventTypes.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventSourcing_Example_With_EventStore.EventTypes
{
    public class UserCreated : IEvent
    {
        public string UserId { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public bool EmailApprove { get; set; }
    }
}
