using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Event = EventSystem.Model.Event;

public class HealthManager : EventBehaviour
{
    public PlayerInfo PlayerInfo;
    
    private int health = 3;
    
    new void Start()
    {
        base.Start();
    }

    protected override void OnEvent(Event e)
    {
        switch (e.Type)
        {
            case "Damage":
                if (e.Info["heroID"] == PlayerInfo.HeroName)
                {
                    Destroy(GameObject.Find("Heart " + health));
                    health--;
                }

                break;
        }
    }

}
