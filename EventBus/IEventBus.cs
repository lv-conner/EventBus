using System;
using System.Collections.Generic;
using System.Text;

namespace EventBus
{
    public interface IEventBus
    {
        void Subscribe(Type eventHandlerType);
        void Unregister(Type eventHandlerType);
        void Publish<TEvent>(TEvent @event) where TEvent : IEvent;

    }
}
