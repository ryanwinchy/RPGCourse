using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization; // Required for ISerializationCallbackReceiver interface
using UnityEngine;

[System.Serializable] // Makes the class serializable so it can be saved and loaded

//Class is generic, so can use generic types (T), which can be any type. ISerializationCallbackReceiver gives us OnBeforeSerialize method and OnAfterDeserialize, which run before and after serialization.
//In a nutshell, this class is basically the same as dictionary but is serializable so can be used for saving and loading data.
//This can generally  just be copied without knowing ins and outs. Its the same as a normal dictionary, except serializable so can be saved.


public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
{
    [SerializeField] List<TKey> keys = new List<TKey>(); // Store keys.
    [SerializeField] List<TValue> values = new List<TValue>(); // Store values.

    // Called before serialization
    public void OnBeforeSerialize()
    {
        keys.Clear();                       // Clear the key list.
        values.Clear();                      // Clear the value list.

        
        foreach (KeyValuePair<TKey, TValue> pair in this)                  // Iterate over each key-value pair in this dictionary class.  
        {
            keys.Add(pair.Key);                     // Add the key to the keys list
            values.Add(pair.Value);                // Add the value to the values list
        }
    }

    // Called after deserialization
    public void OnAfterDeserialize()
    {
        this.Clear(); // Clear the dictionary

        // Check if the number of keys is equal to the number of values
        if (keys.Count != values.Count)
        {
            Debug.Log("Keys count is not equal to values count");
        }

        // Iterate over the keys and values lists
        for (int i = 0; i < keys.Count; i++)
        {
            this.Add(keys[i], values[i]); // Adds keys and values back to this dictionary.
        }
    }
}
