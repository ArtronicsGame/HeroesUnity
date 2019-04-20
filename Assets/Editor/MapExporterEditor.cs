using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Exporter))]
public class MapExporterEditor : Editor
{
    private string _file = "";
    private bool spawnFold = false;

    public override void OnInspectorGUI()
    {
        Exporter exporter = (Exporter) target;
        EditorGUILayout.BeginHorizontal();
        GUILayout.Space(10);
        EditorGUILayout.BeginVertical();
        GUILayout.Space(7);

        exporter.gravity = EditorGUILayout.Vector2Field("Gravity", exporter.gravity);
        exporter.width = EditorGUILayout.FloatField("Width", exporter.width);
        exporter.height = EditorGUILayout.FloatField("Height", exporter.height);
        
        spawnFold = EditorGUILayout.Foldout(spawnFold, "Spawn Places");

        if (spawnFold)
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(10);
            EditorGUILayout.BeginVertical();
            GUILayout.Space(7);

            for (int i = 0; i < exporter.spawnPlaces.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();
                exporter.spawnPlaces[i] = EditorGUILayout.Vector2Field("Place " + (i + 1), exporter.spawnPlaces[i]);
                if (GUILayout.Button("-", GUILayout.MaxWidth(30)))
                    exporter.spawnPlaces.RemoveRange(i, 1);            
                EditorGUILayout.EndHorizontal();
            }

            if (GUILayout.Button("Add"))
            {
                exporter.spawnPlaces.Add(Vector2.zero);
            }

            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();
        }

        GUILayout.Space(10);
        Rect rect = EditorGUILayout.GetControlRect(false, 1);
        rect.height = 1;
        EditorGUI.DrawRect(rect, new Color ( 0.5f,0.5f,0.5f, 1 ) );
        GUILayout.Space(10);

        
        _file = EditorGUILayout.TextField("File Name", _file);

        if (GUILayout.Button("Export"))
        {
            exporter.Export(_file);
            _file = "";
        }

        EditorGUILayout.EndVertical();
        EditorGUILayout.EndHorizontal();
    }

    private void OnSceneGUI()
    {
        Exporter exporter = (Exporter) target;
        for (int i = 0; i < exporter.spawnPlaces.Count; i++)
        {
            exporter.spawnPlaces[i] = Handles.PositionHandle(exporter.spawnPlaces[i], Quaternion.identity);
        }
    }
}