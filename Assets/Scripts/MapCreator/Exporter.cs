using System.Collections;
using System.Collections.Generic;
using System.IO;
using MapCreator.Model;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;

public class Exporter : MonoBehaviour
{

    public Vector2 gravity = new Vector2(0, -10);
    
    private Map _map;
    
    void ObjectHandler(GameObject o)
    {
        ExtraData extraData = o.GetComponent<ExtraData>();
        if(extraData == null)
            return;
        PolygonCollider2D component = o.GetComponent<PolygonCollider2D>();
        var position = o.transform.position;
        PolygonBody polygonBody = new PolygonBody(extraData, o.name, position.x, position.y, o.transform.eulerAngles.z);
        foreach (Vector2 p in component.points)
        {
            var localScale = o.transform.localScale;
            polygonBody.AddNode(p.x * localScale.x, p.y * localScale.y);
        }
        _map.AddBody(polygonBody);
    }
    
    public void Export(string fileName)
    {
        _map = new Map(gravity.x, gravity.y);
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
}
