using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Exporter))]
public class MapExporterEditor : Editor
{

    private string _file = "";
    
    public override void OnInspectorGUI()
    {
        EditorGUILayout.BeginHorizontal();
        GUILayout.Space(10);
        EditorGUILayout.BeginVertical();
        GUILayout.Space(7);
  
        ((Exporter)target).gravity = EditorGUILayout.Vector2Field("Gravity", ((Exporter)target).gravity);

        _file = EditorGUILayout.TextField("File Name", _file);
        
        if (GUILayout.Button("Export"))
        {
            ((Exporter)target).Export(_file);
            _file = "";
        }

        EditorGUILayout.EndVertical();
        EditorGUILayout.EndHorizontal();
    }
}