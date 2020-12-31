using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;


namespace FFStudio
{
    public abstract class RuntimeSet<TKey, TValue> : ScriptableObject
    {
        public List<TValue> itemList = new List<TValue>(128);
        public Dictionary<TKey, TValue> itemDictionary = new Dictionary<TKey, TValue>(128);

        public void AddList(TValue value)
        {
            if (!itemList.Contains(value))
                itemList.Add(value);
        }
        public void RemoveList(TValue value)
        {
            itemList.Remove(value);
        }

        public void AddDictionary(TKey key, TValue value)
        {
            var _isExits = itemDictionary.ContainsKey(key);

            if (!_isExits)
            {
                itemDictionary.Add(key, value);
            }
        }
        public void RemoveDictionary(TKey key)
        {
            itemDictionary.Remove(key);
        }

        [Button]
        public void ClearSet()
        {
            itemList.Clear();
            itemDictionary.Clear();
        }
        [Button]
        public void LogList()
        {
            foreach (var item in itemList)
            {
                Debug.Log(item.ToString());
            }
        }

        [Button]
        public void LogDictionary()
        {
            foreach (var item in itemDictionary.Values)
            {
                Debug.Log(item.ToString());
            }
        }
    }
}