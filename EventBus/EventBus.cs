using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Reflection;

namespace EventBus
{
    public class EventBus : IEventBus
    {
        private readonly Type _eventHandlerType = typeof(IEventHandler);
        private readonly ConcurrentDictionary<Type, List<Type>> _eventHandlers;
        public EventBus()
        {
            _eventHandlers = new ConcurrentDictionary<Type, List<Type>>();
            Init();
        }
        public void Init()
        {
            AppDomain.CurrentDomain
                .GetAssemblies()
                .SelectMany(p => p.GetTypes())
                .Where(p => _eventHandlerType
                .IsAssignableFrom(p))
                .ToList()
                .ForEach(p => Subscribe(p));
        }

        public void Publish<TEvent>(TEvent @event) where TEvent : IEvent
        {
            var eventType = typeof(TEvent);
            if(_eventHandlers.TryGetValue(eventType,out var handlers))
            {
                foreach (var handler in handlers)
                {
                    var instance = Activator.CreateInstance(handler) as IEventHandler<TEvent>;
                    if(instance != null)
                    {
                        instance.Handle(@event);
                    }
                }
            }
        }

        public void Subscribe(Type eventHandlerType)
        {
            if (_eventHandlerType.IsAssignableFrom(eventHandlerType))
            {
                var @interface = eventHandlerType.GetInterface("IEventHandler`1");
                if (@interface != null)
                {
                    var teventType = @interface.GetGenericArguments()[0];

                    var handlers = _eventHandlers.GetOrAdd(teventType, t => new List<Type>());
                    handlers.Add(eventHandlerType);
                }
            }
        }

        public void Unregister(Type eventHandlerType)
        {
            if(_eventHandlerType.IsAssignableFrom(eventHandlerType))
            {
                var @interface = eventHandlerType.GetInterface("IEventHandler`1");
                if (@interface != null)
                {
                    var teventType = @interface.GetGenericArguments()[0];
                    var handlers = _eventHandlers.GetOrAdd(teventType, t => new List<Type>());
                    handlers.Remove(eventHandlerType);
                }
            }
        }
    }
}
