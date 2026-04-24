using System;
using System.Collections;
using System.Collections.Generic;

public class OpenAddressingHashTable<TKey, TValue> : IDictionary<TKey, TValue> where TKey : IComparable<TKey>
{
    protected HashTable<TKey, TValue>[] hash;
    protected int size;
    public int Capacity => hash.Length;
    public OpenAddressingHashTable(int capacity = 10)
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
                throw new KeyNotFoundException("키 없음");
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
        int nextindex = GetSecondaryHash(key);
        int firstdeletedindex = -1;
        if ((float)(size + 1) / hash.Length >= 0.6)
        {
            Resize();
            index = GetHash(key);
            nextindex = GetSecondaryHash(key);
        }
        
        while (hash[index] != null && hash[index].IsOccupied)
        {
            if (key.CompareTo(hash[index].Key) == 0)
            {
                throw new ArgumentException("키 충돌");
            }
            if (hash[index].IsDeleted && firstdeletedindex == -1)
            {
                firstdeletedindex = index;
            }
            
            index = (index +nextindex) % hash.Length;
        }
        int deleteCheckindex = (firstdeletedindex != -1) ? firstdeletedindex : index;
        if (hash[deleteCheckindex] == null)
        {
            hash[deleteCheckindex] = new HashTable<TKey, TValue>(key, value);
        }
        hash[deleteCheckindex].Key = key;
        hash[deleteCheckindex].Value = value;
        hash[deleteCheckindex].IsOccupied = true;
        hash[deleteCheckindex].IsDeleted = false;
        size++;
    }
    public int GetHash(TKey key)
    {
        if (key == null) throw new ArgumentNullException(nameof(key));
        int hash = key.GetHashCode();
        return (hash & 0x7fffffff) % this.hash.Length;
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
        hash = new HashTable<TKey, TValue>[hash.Length];
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
        if (hash == null) yield break;
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
        int nextindex = GetSecondaryHash(key);
        int temp = index;
        while (hash[index] != null)
        {
            if (hash[index].IsOccupied && key.CompareTo(hash[index].Key) == 0)
            {
                hash[index].Key = default;
                hash[index].Value = default;
                hash[index].IsOccupied = false;
                hash[index].IsDeleted = true;
                size--;
                return true;
            }
            index = (index + nextindex) % hash.Length;
            if (index == temp) break;
        }
        return false;
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
        int nextindex = GetSecondaryHash(key);
        int hashindex = index;
        while (hash[index] != null)
        {
            if (hash[index].IsOccupied && key.CompareTo(hash[index].Key) == 0)
            {
                value = hash[index].Value;
                return true;
            }
            index = (index + nextindex) % hash.Length;
            if (index == hashindex) break;
        }

        value = default;
        return false;
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
