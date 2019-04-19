using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ItemExporter))]
public class ItemExporterEditor : Editor
{

    private string _file = "";
    
    public override void OnInspectorGUI()
    {
        EditorGUILayout.BeginHorizontal();
        GUILayout.Space(10);
        EditorGUILayout.BeginVertical();
        GUILayout.Space(7);
  
        _file = EditorGUILayout.TextField("File Name", _file);
        
        if (GUILayout.Button("Export"))
        {
            ((ItemExporter)target).Export(_file);
            _file = "";
        }

        EditorGUILayout.EndVertical();
        EditorGUILayout.EndHorizontal();
    }
}