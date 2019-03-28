using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace MapCreator.Model
{
    public class Map
    {
        [JsonProperty("Gravity")] public Vec2 Gravity { get; set; }
        [JsonProperty("PolygonBodies")] public List<PolygonBody> PolygonBodies { get; set; }

        public Map(float x, float y)
        {
            Gravity = new Vec2(x, y);
            PolygonBodies = new List<PolygonBody>();
        }

        public void AddBody(PolygonBody polygonBody)
        {
            PolygonBodies.Add(polygonBody);
        }
        
    }

    public class Vec2
    {
        [JsonProperty("X")] public float X { get; set; }
        [JsonProperty("Y")] public float Y { get; set; }

        public Vec2(float x, float y)
        {
            X = x;
            Y = y;
        }
    }

    public class PolygonBody
    {
        [JsonProperty("Dynamic")] public bool Dynamic;
        [JsonProperty("Name")] public string Name;
        [JsonProperty("Type")] public string Type;
        [JsonProperty("Position")] public Vec2 Position { get; set; }
        [JsonProperty("Angle")] public float Angle { get; set; }
        [JsonProperty("Nodes")] public List<Vec2> Nodes { get; set; }

        public PolygonBody(ExtraData extraData, string name, float x, float y, float angle)
        {
            Dynamic = extraData.dynamic;
            Name = name;
            Type = extraData.type;
            Position = new Vec2(x, y);
            if (angle > 180)
                angle -= 360;
            Angle = angle * Mathf.Deg2Rad;
            
            Nodes = new List<Vec2>();
        }

        public void AddNode(float x, float y)
        {
            Nodes.Add(new Vec2(x, y));
        }
    }
}