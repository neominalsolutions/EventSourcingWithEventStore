using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventSourcing_Example_With_EventStore.ViewModels
{
    public class CreateUserVM
    {
        public string Name { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
    }
}
