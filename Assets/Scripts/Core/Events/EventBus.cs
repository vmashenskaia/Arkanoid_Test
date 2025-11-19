using System;
using System.Collections.Generic;

namespace Core.Events
{
    /// <summary>
    /// Simple event bus used to broadcast strongly-typed messages across the system.
    /// </summary>
     public class EventBus
    {
        private readonly Dictionary<Type, List<Delegate>> _subscribers;
        
        public EventBus()
        {
            _subscribers = new Dictionary<Type, List<Delegate>>();
        }

        public void Subscribe<T>(Action<T> handler)
        {
            Type type = typeof(T);
            List<Delegate> list = null;

            if (!_subscribers.TryGetValue(type, out list))
            {
                list = new List<Delegate>();
                _subscribers[type] = list;
            }

            list.Add(handler);
        }
        
        public void Unsubscribe<T>(Action<T> handler)
        {
            Type type = typeof(T);
            List<Delegate> list = null;

            if (_subscribers.TryGetValue(type, out list))
            {
                list.Remove(handler);

                if (list.Count == 0)
                {
                    _subscribers.Remove(type);
                }
            }
        }
        
        public void Publish<T>(T data)
        {
            Type type = typeof(T);
            List<Delegate> list = null;

            if (_subscribers.TryGetValue(type, out list))
            {
                Delegate[] copy = list.ToArray();

                foreach (var t in copy)
                {
                    Action<T> action = t as Action<T>;

                    if (action != null)
                    {
                        action.Invoke(data);
                    }
                }
            }
        }
    }
}