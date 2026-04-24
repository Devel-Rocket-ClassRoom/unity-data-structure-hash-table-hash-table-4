using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenAddressingHashTable<TKey, TValue> : IDictionary<TKey, TValue> where TKey : IComparable<TKey>
{
    protected HashTable<TKey, TValue>[] hash;
    protected int size;
    public OpenAddressingHashTable(int capacity = 16)
    {
        hash = new HashTable<TKey, TValue>[capacity];
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
                throw new KeyNotFoundException("같은 키");
            }
        }
        set
        {
            Add(key, value);
        }
    }

    public ICollection<TKey> Keys => throw new System.NotImplementedException();

    public ICollection<TValue> Values => throw new System.NotImplementedException();

    public int Count => hash.Length;

    public bool IsReadOnly => throw new System.NotImplementedException();

    public void Add(TKey key, TValue value)
    {
        
        if ((float)(size + 1) / hash.Length >= 0.75)
        {
            Resize();
        }
        int index = GetHash(key);
        int nextindex = GetSecondaryHash(key);
        while (hash[index] != null && hash[index].IsOccupied)
        {
            if (key.CompareTo(hash[index].Key)==0)
            {
                hash[index].Value = value;
                return;
            }
            index = (index + nextindex) % hash.Length;
        }         
        
        if (hash[index] == null)
        {
            hash[index] = new HashTable<TKey, TValue>(key, value);
        }
        hash[index].Key = key;
        hash[index].Value = value;
        hash[index].IsOccupied = true;
        hash[index].IsDeleted = false;
        size++;
    }
    public int GetHash(TKey key)
    {
        if (key == null) throw new ArgumentNullException(nameof(key));
        int hash = key.GetHashCode();
        return (hash & 0x7fffffff) % this.hash.Length ;
    }

    public int GetSecondaryHash(TKey key)
    {
        if (key == null)
            throw new ArgumentNullException(nameof(key));

        int hash = key.GetHashCode();
        return 1 + ((hash & 0x7fffffff) % (this.hash.Length - 1));
    }

    public void Add(KeyValuePair<TKey, TValue> item)
    {
        Add(item.Key, item.Value);
    }

    public void Clear()
    {
        hash = null;
        size = 0;
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
        foreach (var item in hash)
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
        hash[index].IsOccupied = false;
        hash[index].IsDeleted = true;
        size--;
        return true;
    }

    public bool Remove(KeyValuePair<TKey, TValue> item)
    {
        throw new System.NotImplementedException();
    }
    public void Resize()
    {
        var oldset = hash;
        hash = new HashTable<TKey, TValue>[oldset.Length * 2];
        size = 0;
        foreach (var item in oldset)
        {
            if (item != null && item.IsOccupied)
            {
                Add(item.Key, item.Value);
            }
        }
    }
    public bool TryGetValue(TKey key, out TValue value)
    {
        int index = GetHash(key);
        if (hash[index] != null && key.CompareTo(hash[index].Key) == 0)
        {
            value = hash[index].Value;
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
