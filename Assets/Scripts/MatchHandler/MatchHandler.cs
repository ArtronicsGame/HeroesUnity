using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Event = EventSystem.Model.Event;

public class MatchHandler : EventBehaviour
{
    void Start()
    {
        
    }

    new void Update()
    {
        base.Update();
        
    }

    protected override void OnEvent(Event e)
    {
        Debug.Log(e.Type);
    }
}
