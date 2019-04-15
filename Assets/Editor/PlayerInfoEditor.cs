using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PlayerInfo))]
public class PlayerInfoEditor : Editor
{
    private PlayerInfo _playerInfo;
    private bool[] _fold = new bool[20];
    private bool _heroesFold = false;

    public override void OnInspectorGUI()
    {
        _playerInfo = (PlayerInfo) target;
        if (_playerInfo.PlayerData.HeroProperties != null)
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(10);
            EditorGUILayout.BeginVertical();
            GUILayout.Space(7);
            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.TextField("ID", _playerInfo.PlayerData.ID);
            EditorGUILayout.TextField("Username", _playerInfo.PlayerData.Username);
            EditorGUILayout.TextField("Clan Position", _playerInfo.PlayerData.PlayerClanPosition);
            EditorGUILayout.TextField("Match IP", _playerInfo.PlayerData.MatchIP);
            EditorGUILayout.TextField("Match Port", _playerInfo.PlayerData.MatchPort.ToString());
            EditorGUILayout.TextField("Trophies", _playerInfo.PlayerData.Trophies.ToString());
            EditorGUILayout.TextField("Coins", _playerInfo.PlayerData.Coins.ToString());
            EditorGUILayout.TextField("Experience", _playerInfo.PlayerData.Experience.ToString());
            EditorGUILayout.TextField("Current Hero", _playerInfo.PlayerData.CurrentHero);

            _heroesFold = EditorGUILayout.Foldout(_heroesFold, "Heroes Properties");
            if (_heroesFold)
            {
                EditorGUILayout.BeginHorizontal();
                GUILayout.Space(10);
                EditorGUILayout.BeginVertical();
                
                int i = 0;
                foreach (KeyValuePair<string, HeroInfo> pair in _playerInfo.PlayerData.HeroProperties)
                {
                    _fold[i] = EditorGUILayout.Foldout(_fold[i], pair.Key);
                    if (_fold[i])
                    {
                        EditorGUILayout.BeginHorizontal();
                        GUILayout.Space(10);
                        EditorGUILayout.BeginVertical();
                        
                        EditorGUILayout.TextField("Level", pair.Value.BasicInfo.Level.ToString());
                        EditorGUILayout.TextField("Equip 1", pair.Value.BasicInfo.Equip1.ToString());
                        EditorGUILayout.TextField("Equip 2", pair.Value.BasicInfo.Equip2.ToString());
                        EditorGUILayout.Toggle("Unlocked", pair.Value.IsUnlocked);
                        EditorGUILayout.TextField("Trophies", pair.Value.Trophies.ToString());
                        EditorGUILayout.TextField("Experience", pair.Value.Experience.ToString());


                        
                        EditorGUILayout.EndVertical();
                        EditorGUILayout.EndHorizontal();
                    }

                    i++;
                }
                
                EditorGUILayout.EndVertical();
                EditorGUILayout.EndHorizontal();
            }

            EditorGUI.EndDisabledGroup();
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();
        }
        else
        {
            EditorGUILayout.HelpBox("Run Game To Get Data", MessageType.Info);
        }
    }
}