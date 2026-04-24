using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System.Linq;

public class SImpleHashTable<TKey, TValue> : IDictionary<TKey, TValue> where TKey : IComparable<TKey>
{
    protected HashTable<TKey, TValue>[] root;
    protected int size;
    protected int Capacity => root.Length;
    public SImpleHashTable(int capacity = 16)
    {
        root = new HashTable<TKey, TValue>[capacity];
        size = 0;
    }

    public TValue this[TKey key]
    {
        get
        {
            if (TryGetValue(key, out TValue value))
            {
                return value;
            }
            else
            {
                throw new KeyNotFoundException($"{key}겹침");
            }
        }
        set
        {
            Add(key, value);
        }
    }

    public ICollection<TKey> Keys => throw new System.NotImplementedException();

    public ICollection<TValue> Values => throw new System.NotImplementedException();

    public int Count => size;

    public bool IsReadOnly => throw new System.NotImplementedException();

    public void Add(TKey key, TValue value)
    {
        int index = GetHash(key);
        if ((float)(size+1)/root.Length>=0.75)
        {
            Resize();
        }
        if (root[index] != null && root[index].IsOccupied)
        {
            throw new ArgumentException($"{key} : 해시 충돌");
        }
        if (root[index] == null)
        {
            root[index] = new HashTable<TKey, TValue>(key, value);
        }
        root[index].Key = key;
        root[index].Value = value;
        root[index].IsOccupied = true;
        size++;
    }
    public int GetHash(TKey key)
    {
        if(key == null) throw new ArgumentNullException(nameof(key));
        int hash = key.GetHashCode();
        return (hash & 0x7fffffff) % root.Length;
    }
    public void Add(KeyValuePair<TKey, TValue> item)
    {
        Add(item.Key, item.Value);
    }


    public void Clear()
    {
        throw new System.NotImplementedException();
    }

    public bool Contains(KeyValuePair<TKey, TValue> item)
    {
        throw new System.NotImplementedException();
    }

    public bool ContainsKey(TKey key)
    {
        return TryGetValue(key, out _);
    }

    public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
    {
        throw new System.NotImplementedException();
    }

    public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
    {
        foreach(var item in root)
        {
            if (item != null && item.IsOccupied)
            {
                yield return new KeyValuePair<TKey, TValue>(item.Key, item.Value);
            }
        }
      
    }

    public bool Remove(TKey key)
    {
        int index = GetHash(key);
        if (root[index] != null && key.CompareTo(root[index].Key)==0)
        {
            root[index].IsOccupied = false;
            root[index].Key = default;
            root[index].Value = default;
            size--;
        }
        return false;
    
    }
    public void Resize()
    {
        var oldset = root;
        root = new HashTable<TKey, TValue>[Capacity*2];
        size = 0;
        foreach(var item in oldset)
        {
            if(item !=null &&item.IsOccupied)
            {
                Add(item.Key, item.Value);
            }
        }
    }
    public bool Remove(KeyValuePair<TKey, TValue> item)
    {
        throw new System.NotImplementedException();
    }

    public bool TryGetValue(TKey key, out TValue value)
    {
        int index = GetHash(key);
        if (root[index]!=null&& key.CompareTo(root[index].Key)==0)
        {
            value = root[index].Value;
            return true;
        }
        value = default;
        return false;
    }


    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}