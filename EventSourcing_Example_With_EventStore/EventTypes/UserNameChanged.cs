using EventSourcing_Example_With_EventStore.EventTypes.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventSourcing_Example_With_EventStore.EventTypes
{
    public class UserNameChanged : IEvent
    {
        public string UserId { get; set; }
        public string NewUserName { get; set; }
    }
}
