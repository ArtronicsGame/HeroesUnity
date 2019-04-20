using System.Collections;
using System.Collections.Generic;
using System.IO;
using MapCreator.Model;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;

public class Exporter : MonoBehaviour
{
    public List<Vector2> spawnPlaces = new List<Vector2>();
    public float width, height; 

    public Vector2 gravity = new Vector2(0, -10);
    
    private Map _map;
    
    void ObjectHandler(GameObject o)
    {
        ExtraData extraData = o.GetComponent<ExtraData>();
        if(extraData == null)
            return;
        var position = o.transform.position;
        Body body = new Body(extraData, o.name, position.x, position.y, o.transform.eulerAngles.z);
        
        Collider2D[] colliders = o.GetComponents<Collider2D>();
        foreach (Collider2D collider in colliders)
        {
            if (collider is PolygonCollider2D polyCol)
            {
                Polygon polygon = new Polygon();
                foreach (Vector2 p in polyCol.points)
                {
                    var localScale = o.transform.localScale;
                    polygon.AddNode(p.x * localScale.x, p.y * localScale.y);
                }
                body.AddShape(polygon);
            }
            else if (collider is CircleCollider2D circleCol)
            {
                var localScale = o.transform.localScale;
                var offset = circleCol.offset;
                Circle circle = new Circle(offset.x * localScale.x, offset.y * localScale.y, circleCol.radius);
                body.AddShape(circle);
            }
            else if(collider is EdgeCollider2D chainCol)
            {
                Chain chain = new Chain();
                foreach (Vector2 p in chainCol.points)
                {
                    var localScale = o.transform.localScale;
                    chain.AddNode(p.x * localScale.x, p.y * localScale.y);
                }
                body.AddShape(chain);
            }
        }
        
        _map.AddBody(body);
    }
    
    public void Export(string fileName)
    {
        _map = new Map(gravity.x, gravity.y, width, height, spawnPlaces);
        foreach (Transform trans in transform)
        {
            GameObject o = trans.gameObject;
            ObjectHandler(o);
        }

        string mapData = JsonConvert.SerializeObject(_map);
        
        StreamWriter streamWriter = new StreamWriter("Assets/Maps/" + fileName + ".txt");
        streamWriter.Write(mapData);
        streamWriter.Close();
        
        #if UNITY_EDITOR
        AssetDatabase.ImportAsset("Assets/Maps/" + fileName + ".txt");
        #endif
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(new Vector3(0, 0), new Vector3(width, 0));
        Gizmos.DrawLine(new Vector3(width, 0), new Vector3(width, height));
        Gizmos.DrawLine(new Vector3(width, height), new Vector3(0, height));
        Gizmos.DrawLine(new Vector3(0, height), new Vector3(0, 0));
    }
}
