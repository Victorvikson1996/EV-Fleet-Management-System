using System;
using System.Collections.Concurrent;

// Event Bus for pub/sub
public class EventBus
{
    private readonly ConcurrentDictionary<string, List<Action<object>>> _subscribers = new();

    public void Publish<T>(string eventName, T data)
    {
        if (_subscribers.TryGetValue(eventName, out var subscribers))
        {
            foreach (var subscriber in subscribers)
            {
                if (data != null)
                {
                    subscriber(data);
                }
            }
        }
    }

    public void Subscribe<T>(string eventName, Action<T> callback)
    {
        _subscribers.AddOrUpdate(eventName, new List<Action<object>> { data => callback((T)data) }, (key, old) =>
        {
            old.Add(data => callback((T)data));
            return old;
        });
    }

    public void Unsubscribe<T>(string eventName, Action<T> callback)
    {
        if (_subscribers.TryGetValue(eventName, out var subscribers))
        {
            subscribers.RemoveAll(subscriber => subscriber == (Action<object>)(data => callback((T)data)));
            if (subscribers.Count == 0)
                _subscribers.TryRemove(eventName, out _);
        }
    }
}