using System.Collections.Generic;
using MapCreator;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PrefabMap))]
public class PrefabMapEditor : Editor
{

    private PrefabMap _prefabMap;
    private bool _folded = true;
    private string _key = "";
    private GameObject _val;

    public override void OnInspectorGUI()
    {
        _prefabMap = (PrefabMap) target;
        Dictionary<string, GameObject> copy = _prefabMap.prefabsMap;
        EditorGUILayout.BeginHorizontal();
        GUILayout.Space(10);
        EditorGUILayout.BeginVertical();
        GUILayout.Space(7);
        _folded = EditorGUILayout.Foldout(_folded, "Map");
        if (_folded)
        {
            EditorGUILayout.BeginVertical();
            foreach (KeyValuePair<string, GameObject> pair in copy)
            {
                EditorGUILayout.BeginHorizontal();
                GUILayout.Space(15);
                EditorGUILayout.PrefixLabel(pair.Key);
                GameObject source = pair.Value;
                source = (GameObject) EditorGUILayout.ObjectField(source, typeof(GameObject), true);
                if (source != pair.Value)
                {
                    _prefabMap.prefabsMap[pair.Key] = source;
                }

                if (GUILayout.Button("-", GUILayout.MaxWidth(30)))
                {
                    _prefabMap.prefabsMap.Remove(pair.Key);
                }

                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(25);
            EditorGUILayout.BeginVertical();
            _key = EditorGUILayout.TextField("Key", _key);
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("Value");
            _val = (GameObject) EditorGUILayout.ObjectField(_val, typeof(GameObject), true);
            EditorGUILayout.EndHorizontal();
            if (GUILayout.Button("Add"))
            {
                if (!_prefabMap.prefabsMap.ContainsKey(_key))
                    _prefabMap.prefabsMap.Add(_key, _val);
                else
                    _prefabMap.prefabsMap[_key] = _val;

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