﻿using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using UnityEngine;
using Event = EventSystem.Model.Event;

public abstract class EventBehaviour : MonoBehaviour
{
    private readonly ConcurrentQueue<Event> _events = new ConcurrentQueue<Event>();

    public void AddEvent(Event e)
    {
        _events.Enqueue(e);
    }

    public void Update()
    {
        Event e;
        if (_events.TryDequeue(out e))
        {
            OnEvent(e);
        }
    }

    protected abstract void OnEvent(Event e);
}