using System;
using System.Collections.Generic;

public class GameEvent
{
    
}

public static class EventManager
{
    private static readonly Dictionary<Type, Action<GameEvent>> _events = new();
    private static readonly Dictionary<Delegate, Action<GameEvent>> _lookUp = new();

    public static void AddListener<T>(Action<T> evt) where T : GameEvent
    {
        if (!_lookUp.ContainsKey(evt))
        {
            Action<GameEvent> newAction = e => evt((T) e);
            _lookUp[evt] = newAction;
            
            if (_events.TryGetValue(typeof(T), out Action<GameEvent> innerAction))
            {
                _events[typeof(T)] = innerAction += newAction;
            }
            else
            {
                _events[typeof(T)] = newAction;
            }
        }
    }

    public static void RemoveListener<T>(Action<T> evt) where T : GameEvent
    {
        if (_lookUp.TryGetValue(evt, out var action))
        {
            if (_events.TryGetValue(typeof(T), out var tempAction))
            {
                tempAction -= action;
                if (tempAction == null)
                    _events.Remove(typeof(T));
                else
                    _events[typeof(T)] = tempAction;
            }
            
            _lookUp.Remove(evt);
        }
    }

    public static void BroadCast(GameEvent evt)
    {
        if (_events.TryGetValue(evt.GetType(), out var action))
        {
            action.Invoke(evt);
        }
    }
    
    public static void Clear()
    {
        _events.Clear();
        _lookUp.Clear();
    }
}