using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

//Unity can't serialize dictionaries, but we can make them serializable by
//breaking down the dictionary into two lists of keys and values, respectively
//This allows us to save information such as specific enemy locations, whether
//certain pickups have been collected already, etc., and use that to ensure that
//you can't exploit saves resetting collectibles.
public class SerializableDict<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
    
{
    //Separate dictionary into two lists, keys and values
    [SerializeField] private List<TKey> keys = new List<TKey>();
    [SerializeField] private List<TValue> values = new List<TValue>();

    //Save dictionary to lists, clearing lists first to ensure no errors
    public void OnBeforeSerialize()
    {
        keys.Clear(); 
        values.Clear();
        foreach (KeyValuePair<TKey, TValue> kvp in this)
        {
            keys.Add(kvp.Key);
            values.Add(kvp.Value);
        }
    }

    //Load dictionary to lists, again clearing lists first to ensure no errors
    public void OnAfterDeserialize()
    {
        this.Clear();
        //Quick check to make sure key and value counts match
        if (keys.Count != values.Count)
        {
            Debug.LogError("number of Keys =/= number of Values");
        }
        for (int i = 0; i < keys.Count; i++)
        {
            this.Add(keys[i], values[i]);
        }
    }
}
