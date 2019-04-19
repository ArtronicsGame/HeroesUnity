using System.Collections;
using System.Collections.Generic;
using System.IO;
using MapCreator.Model;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;

public class ItemExporter : MonoBehaviour
{
        
    public void Export(string fileName){
        ExtraData extraData = GetComponent<ExtraData>();
            if(extraData == null)
        return;
        var position = transform.position;
        Body body = new Body(extraData, name, position.x, position.y, transform.eulerAngles.z);
            
        Collider2D[] colliders = GetComponents<Collider2D>();
            foreach (Collider2D collider in colliders)
        {
            if (collider is PolygonCollider2D polyCol)
            {
                Polygon polygon = new Polygon();
                foreach (Vector2 p in polyCol.points)
                {
                    var localScale = transform.localScale;
                    polygon.AddNode(p.x * localScale.x, p.y * localScale.y);
                }
                body.AddShape(polygon);
            }
            else if (collider is CircleCollider2D circleCol)
            {
                var localScale = transform.localScale;
                var offset = circleCol.offset;
                Circle circle = new Circle(offset.x * localScale.x, offset.y * localScale.y, circleCol.radius);
                body.AddShape(circle);
            }
            else if(collider is EdgeCollider2D chainCol)
            {
                Chain chain = new Chain();
                foreach (Vector2 p in chainCol.points)
                {
                    var localScale = transform.localScale;
                    chain.AddNode(p.x * localScale.x, p.y * localScale.y);
                }
                body.AddShape(chain);
            }
        }
   
        string bodyData = JsonConvert.SerializeObject(body);
        
        StreamWriter streamWriter = new StreamWriter("Assets/Maps/" + fileName + ".txt");
        streamWriter.Write(bodyData);
        streamWriter.Close();
        
        #if UNITY_EDITOR
        AssetDatabase.ImportAsset("Assets/Maps/" + fileName + ".txt");
        #endif
    }
}
