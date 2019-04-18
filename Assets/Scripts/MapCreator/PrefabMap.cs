using System;
using RotaryHeart.Lib.SerializableDictionary;
using UnityEngine;

namespace MapCreator
{
    [Serializable]
    public class DictionaryOfPrefabs: SerializableDictionaryBase<string, GameObject> {}
    
    [CreateAssetMenu(fileName = "PrefabMap")]
    public class PrefabMap : ScriptableObject
    {
        [SerializeField]
        public DictionaryOfPrefabs prefabsMap = new DictionaryOfPrefabs();

        public GameObject GetObject(string name)
        {
            return prefabsMap[name];
        }
    }
}