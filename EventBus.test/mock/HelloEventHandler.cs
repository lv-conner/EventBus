using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace EventBus.test.mock
{
    public class CNHelloEventHandler : IEventHandler<HelloEvent>
    {
        public void Handle(HelloEvent @event)
        {
            Debug.WriteLine("你好" + @event.Name);
        }
    }
    public class ENHelloEventHandler : IEventHandler<HelloEvent>
    {
        public void Handle(HelloEvent @event)
        {
            Debug.WriteLine("Hello" + @event.Name);
        }
    }
}
