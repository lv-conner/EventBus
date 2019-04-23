using System;
using System.Collections.Generic;
using System.Text;

namespace EventBus.test.mock
{
    public class HelloEvent:IEvent
    {
        public string Name { get; set; }
    }
}
