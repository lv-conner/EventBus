using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace EventBus
{
    public class MemoryQueueEventBus:EventBus
    {
        private readonly ConcurrentQueue<object> _tasks;
        private readonly AutoResetEvent _singal;
        private readonly CancellationToken _token;
        public MemoryQueueEventBus()
        {
            _tasks = new ConcurrentQueue<object>();
            _singal = new AutoResetEvent(true);
            _token = new CancellationToken();
            Task.Run((Action)Loop);
        }
        public override void Publish<TEvent>(TEvent @event)
        {
            _tasks.Enqueue(@event);
        }
        private void Loop()
        {
            while(true)
            {
                if(_token.IsCancellationRequested)
                {
                    break;
                }
                if(_tasks.TryDequeue(out var task))
                {
                    var eventType = task.GetType();
                    if (_eventHandlers.TryGetValue(eventType, out var handlers))
                    {
                        foreach (var handler in handlers)
                        {
                            var instance = Activator.CreateInstance(handler);
                            var methodInfo = handler.GetMethod("Handle");
                            if(methodInfo == null)
                            {
                                methodInfo.Invoke(instance,new object[] { task });
                            }
                        }
                    }
                }
                else
                {
                    _singal.WaitOne();
                }
            }
        }
    }
}
