using UnityEngine;

public class HashTable<TKey,TValue>
{
    public TKey Key { get; set; }
    public TValue Value { get; set; }
    public bool IsOccupied {  get; set; }
    public HashTable<TKey, TValue> Next { get; set; }
    public HashTable(TKey key, TValue value)
    {
        this.Key = key;
        this.Value = value;
    }
}
