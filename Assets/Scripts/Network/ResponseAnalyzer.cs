using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;
using Event = EventSystem.Model.Event;

[Serializable] 
public class DictionaryOfGameObjects: SerializableDictionary<string, EventBehaviour> {}

public class ResponseAnalyzer : MonoBehaviour
{
    public DictionaryOfGameObjects map = new DictionaryOfGameObjects();
    
    public void Analyze(Event response)
    {
        if (map.ContainsKey(response.Type))
        {
            EventBehaviour system = map[response.Type];
            system.AddEvent(response);
        }
        else
        {
            Debug.Log("Unknown Type Of Response");
        }
    }
}
