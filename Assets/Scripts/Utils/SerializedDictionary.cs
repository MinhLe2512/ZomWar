using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SerializedDictionary : ISerializationCallbackReceiver
{
    [SerializeField] private List<string> keys = new();
    [SerializeField] private List<AudioClip> values = new();

    private Dictionary<string, AudioClip> dictionary = new();

    public Dictionary<string, AudioClip> ToDictionary() => dictionary;

    public void OnBeforeSerialize()
    {
        keys.Clear();
        values.Clear();

        foreach (var kvp in dictionary)
        {
            keys.Add(kvp.Key);
            values.Add(kvp.Value);
        }
    }

    public void OnAfterDeserialize()
    {
        dictionary = new Dictionary<string, AudioClip>();

        for (int i = 0; i < Math.Min(keys.Count, values.Count); i++)
        {
            if (!dictionary.ContainsKey(keys[i]))
                dictionary.Add(keys[i], values[i]);
        }
    }

    // Optional: Add indexer for convenience
    public AudioClip this[string key]
    {
        get => dictionary[key];
        set => dictionary[key] = value;
    }

    public bool ContainsKey(string key) => dictionary.ContainsKey(key);
    public Dictionary<string, AudioClip>.KeyCollection Keys => dictionary.Keys;
    public Dictionary<string, AudioClip>.ValueCollection Values => dictionary.Values;
}
