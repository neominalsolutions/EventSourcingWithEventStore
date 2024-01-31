using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventSourcing_Example_With_EventStore.Exceptions
{
    public class StreamNotFoundException : Exception
    {
        public StreamNotFoundException() : base("Stream not found")
        { }
        public StreamNotFoundException(string message) : base(message)
        { }
    }
}
