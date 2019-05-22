using System.Collections;
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

    public void  Start()
    {
        StartCoroutine(QueueCheck());
    }

    private IEnumerator QueueCheck()
    {
        while (true)
        {
            while (_events.TryDequeue(out var e))
            {
                OnEvent(e);
            }
            
            yield return new WaitForSeconds(0.005f);
        }
    }

    protected abstract void OnEvent(Event e);
}