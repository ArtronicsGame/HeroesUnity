using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using RotaryHeart.Lib.SerializableDictionary;
using UnityEngine;
using Event = EventSystem.Model.Event;

[Serializable] 
public class DictionaryOfGameObjects: SerializableDictionaryBase<string, EventBehaviour> {}

[Serializable]
public class ResponseAnalyzer : MonoBehaviour
{
    [SerializeField]
    public DictionaryOfGameObjects map = new DictionaryOfGameObjects();
    
    [MethodImpl(MethodImplOptions.Synchronized)]
    public bool Analyze(Event response)
    {
        if (map.ContainsKey(response.Type))
        {
            EventBehaviour system = map[response.Type];
            system.AddEvent(response);
        }
        else
        {
            Debug.Log("Unknown Type Of Response\n" + response.Type);
            return false;
        }

        return true;
    }
}
