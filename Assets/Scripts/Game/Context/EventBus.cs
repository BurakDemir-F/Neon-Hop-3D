using System;
using System.Collections.Generic;

namespace General
{
    
    public interface IEventBus
    {
        void Register<T>(Action<T> handler);
        void UnRegister<T>(Action<T> handler);
        void Publish<T>(T eventData);
    }

    
    public class EventBus : IEventBus
    {
        private Dictionary<Type, Delegate> _registeredDict = new();

        public void Register<T>(Action<T> action)
        {
            var type = typeof(T);
            if (_registeredDict.TryGetValue(type, out var existingDelegate))
            {
                existingDelegate = Delegate.Combine(existingDelegate, action);
                _registeredDict[type] = existingDelegate;
            }
            else
            {
                _registeredDict[type] = action;
            }
        }

        public void UnRegister<T>(Action<T> action)
        {
            var type = typeof(T);
            
            if (!_registeredDict.ContainsKey(type))
                return;

            var existingDelegate = _registeredDict[type];
            existingDelegate = Delegate.Remove(existingDelegate, action);

            if (existingDelegate == null)
                _registeredDict.Remove(type);
            else
                _registeredDict[type] = existingDelegate;
        }

        public void Publish<T>(T data)
        {
            var type = typeof(T);

            if (_registeredDict.TryGetValue(type, out var existingDelegate))
            {
                ((Action<T>)existingDelegate)?.Invoke(data);
            }
        }
    }
}