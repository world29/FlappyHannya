﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public static class BroadcastReceivers
{
    private static readonly IDictionary<Type, IList<GameObject>> m_broadcastReceivers = new Dictionary<Type, IList<GameObject>>();

    public static IList<GameObject> GetHandlersForEvent<TEventType>() where TEventType : IEventSystemHandler
    {
        if (!m_broadcastReceivers.ContainsKey(typeof(TEventType)))
        {
            return null;
        }
        return m_broadcastReceivers[typeof(TEventType)];
    }

    public static void RegisterBroadcastReceiver<TEventType>(GameObject go) where TEventType : IEventSystemHandler
    {
        if (!m_broadcastReceivers.ContainsKey(typeof(TEventType)))
        {
            m_broadcastReceivers.Add(typeof(TEventType), new List<GameObject>());
        }

        m_broadcastReceivers[typeof(TEventType)].Add(go);
    }

    public static void UnregisterBroadcastReceiver<TEventType>(GameObject go)
    {
        if (m_broadcastReceivers.ContainsKey(typeof(TEventType)))
        {
            m_broadcastReceivers[typeof(TEventType)].Remove(go);
        }
    }
}

public static class BroadcastExecuteEvents
{
    public static void Execute<T>(BaseEventData eventData, ExecuteEvents.EventFunction<T> functor) where T : IEventSystemHandler
    {
        var handlers = BroadcastReceivers.GetHandlersForEvent<T>();

        if (handlers == null) return;

        foreach (var handler in handlers)
        {
            ExecuteEvents.Execute<T>(handler, eventData, functor);
        }
    }
}
