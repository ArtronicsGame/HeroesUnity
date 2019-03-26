using System;
using UnityEngine;
using Utils;

namespace MapCreator
{
    [Serializable]
    public class DictionaryOfPrefabs: SerializableDictionary<string, GameObject> {}

    
    [CreateAssetMenu(fileName = "PrefabMap")]
    public class PrefabMap : ScriptableObject
    {
        public DictionaryOfPrefabs prefabsMap = new DictionaryOfPrefabs();

        public GameObject GetObject(string name)
        {
            return prefabsMap[name];
        }
    }
}