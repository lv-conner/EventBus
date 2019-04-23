using EventBus.test.mock;
using System;
using Xunit;

namespace EventBus.test
{
    public class EventBusTest
    {
        [Fact]
        public void Case1()
        {
            IEventBus eventBus = new EventBus();
            //eventBus.Subscribe(typeof(CNHelloEventHandler));
            //eventBus.Subscribe(typeof(ENHelloEventHandler));
            eventBus.Publish(new HelloEvent() { Name = "tim lv" });
        }
    }
}
