using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace MapCreator.Model
{
    public class Map
    {
        [JsonProperty("Gravity")] public Vec2 Gravity { get; set; }
        
        [JsonProperty("Bodies")] public List<Body> Bodies { get; set; }

        public Map(float x, float y)
        {
            Gravity = new Vec2(x, y);
            Bodies = new List<Body>();
        }

        public void AddBody(Body body)
        {
            Bodies.Add(body);
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
    
    public class Body
    {
        [JsonProperty("Dynamic")] public bool Dynamic { get; set; }
        [JsonProperty("Name")] public string Name { get; set; }
        [JsonProperty("Type")] public string Type { get; set; }
        [JsonProperty("Position")] public Vec2 Position { get; set; }
        [JsonProperty("Angle")] public float Angle { get; set; }
        [JsonProperty("Polygons")] public List<Polygon> Polygons { get; set; }
        [JsonProperty("Circles")] public List<Circle> Circles { get; set; }
        [JsonProperty("Chains")] public List<Chain> Chains { get; set; }
        
        public Body(ExtraData extraData, string name, float x, float y, float angle)
        {
            Dynamic = extraData.dynamic;
            Name = name;
            Type = extraData.type;
            Position = new Vec2(x, y);
            if (angle > 180)
                angle -= 360;
            Angle = angle * Mathf.Deg2Rad;
            
            Polygons = new List<Polygon>();
            Circles = new List<Circle>();
            Chains = new List<Chain>();
        }
        
        public void AddShape(Polygon polygon)
        {
            Polygons.Add(polygon);
        }

        public void AddShape(Circle circle)
        {
            Circles.Add(circle);
        }
        
        public void AddShape(Chain chain)
        {
            Chains.Add(chain);
        }
        
        
    }

    public class Polygon
    {
        [JsonProperty("Nodes")] public List<Vec2> Nodes { get; set; }

        public Polygon()
        {
            Nodes = new List<Vec2>();
        }

        public void AddNode(float x, float y)
        {
            Nodes.Add(new Vec2(x, y));
        }
    }
    
    public class Circle
    {
        [JsonProperty("Position")] public Vec2 Position { get; set; }
        [JsonProperty("Radius")] public float Radius { get; set; }

        public Circle(float x, float y, float radius)
        {
            Position = new Vec2(x, y);
            Radius = radius;
        }
    }
    
    public class Chain
    {

        [JsonProperty("Nodes")] public List<Vec2> Nodes { get; set; }

        public Chain()
        {
            Nodes = new List<Vec2>();
        }

        public void AddNode(float x, float y)
        {
            Nodes.Add(new Vec2(x, y));
        }
    }
}