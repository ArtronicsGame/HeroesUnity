using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ResponseAnalyzer))]
public class ResponseAnalyzerEditor : Editor
{
    private bool _folded = true;
    private string _key = "";
    private EventBehaviour _val;

    
    public override void OnInspectorGUI()
    {
        ResponseAnalyzer responseAnalyzer = (ResponseAnalyzer) target;
        Dictionary<string, EventBehaviour> copy = new Dictionary<string, EventBehaviour>(responseAnalyzer.map);
        EditorGUILayout.BeginHorizontal();
        GUILayout.Space(10);
        EditorGUILayout.BeginVertical();
        GUILayout.Space(7);
        _folded = EditorGUILayout.Foldout(_folded, "Map");
        if (_folded)
        {
            EditorGUILayout.BeginVertical();
            foreach (KeyValuePair<string, EventBehaviour> pair in copy)
            {
                EditorGUILayout.BeginHorizontal();
                GUILayout.Space(15);
                EditorGUILayout.PrefixLabel(pair.Key);
                EventBehaviour source = pair.Value;
                source = (EventBehaviour) EditorGUILayout.ObjectField(source, typeof(EventBehaviour), true);
                if (source != pair.Value)
                {
                    responseAnalyzer.map[pair.Key] = source;
                }

                if (GUILayout.Button("-", GUILayout.MaxWidth(30)))
                {
                    responseAnalyzer.map.Remove(pair.Key);
                }

                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(25);
            EditorGUILayout.BeginVertical();
            _key = EditorGUILayout.TextField("Key", _key);
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("Value");
            _val = (EventBehaviour) EditorGUILayout.ObjectField(_val, typeof(EventBehaviour), true);
            EditorGUILayout.EndHorizontal();
            if (GUILayout.Button("Add"))
            {
                if (!responseAnalyzer.map.ContainsKey(_key))
                    responseAnalyzer.map.Add(_key, _val);
                else
                    responseAnalyzer.map[_key] = _val;

                _key = "";
                _val = null;
            }

            EditorGUILayout.EndVertical();

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.EndVertical();
        }

        EditorGUILayout.EndVertical();
        EditorGUILayout.EndHorizontal();
    }
}