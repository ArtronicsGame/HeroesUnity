using System.Collections;
using System.Collections.Generic;
using MapCreator;
using UnityEngine;
using Event = EventSystem.Model.Event;

public class Creator : EventBehaviour
{
    public PrefabMap prefabMap;
    
    new void Start()
    {
        base.Start();
    }

    protected override void OnEvent(Event e)
    {
        switch (e.Type)
        {
            case "NewItem":
                GameObject gameObjectDef = prefabMap.GetObject(e.Info["objName"]);
                Vector2 pos = new Vector2(float.Parse(e.Info["X"]), float.Parse(e.Info["Y"]));
                Quaternion angle = Quaternion.Euler(0, 0, Mathf.Rad2Deg * float.Parse(e.Info["Angle"]));
                GameObject element = Instantiate(gameObjectDef, pos, angle);
                element.transform.parent = gameObject.transform;
                element.name = e.Info["elemName"];
                break;
        }
    }
}
