using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class ChainingHashTable<TKey, TValue> : IDictionary<TKey, TValue>
{
    private LinkedList<KeyValuePair<TKey, TValue>>[] buckets;

    private int bucketsize;
    private int count;

    private const int DefaultCapacity = 16;
    private const double LoadFactor = 0.75;

    
    

    public ChainingHashTable(int capacity = DefaultCapacity)
    {
        bucketsize = capacity;
        buckets = new LinkedList<KeyValuePair<TKey, TValue>>[bucketsize];
        for (int i = 0; i < bucketsize; i++)
        {
            buckets[i] = new LinkedList<KeyValuePair<TKey, TValue>>();
        }
        count = 0;
    }

    public int GetHash(TKey key)
    {
        return (key.GetHashCode() & 0x7FFFFFFF) % bucketsize;
    }

    public TValue this[TKey key]
    {
        get
        {
            if(TryGetValue(key, out var value)) return value;
            throw new KeyNotFoundException("Key not found");
        }
        set
        {
            if(key == null) throw new ArgumentNullException(nameof(key));
            int index = GetHash(key);
            var bucket = FindNode(index, key);
        }
    }


    public ICollection<TKey> Keys => throw new NotImplementedException();

    public ICollection<TValue> Values => throw new NotImplementedException();

    public int Count => count;

    public bool IsReadOnly => false;

    public void Add(TKey key, TValue value)
    {
        throw new System.NotImplementedException();
    }

    public void Add(KeyValuePair<TKey, TValue> item)
    {
        throw new System.NotImplementedException();
    }

    public void Clear()
    {
        for(int i = 0; i < buckets.Length;i++)
        {
            buckets[i] = null;
        }
        count = 0;
    }

    public bool Contains(KeyValuePair<TKey, TValue> item)
    {
        if(!TryGetValue(item.Key, out var value)) return false;
        return EqualityComparer<TValue>.Default.Equals(value, item.Value);
    }

    public bool ContainsKey(TKey key)
    {
        if (key == null) throw new ArgumentNullException(nameof(key));
        return FindNode(GetHash(key), key) != null;
    }

    public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
    {
        if (array == null) throw new ArgumentNullException(nameof(array));
        if (arrayIndex < 0 || arrayIndex > array.Length) throw new ArgumentOutOfRangeException(nameof(arrayIndex));
        if (array.Length - arrayIndex < count) throw new ArgumentException("The array is too small to copy the elements.");


        foreach (var kvp in this)
        {
            array[arrayIndex++] = kvp;
        }
    }

    public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
    {
        foreach (var bucket in buckets)
        {
            foreach (var kvp in bucket)
            {
                yield return kvp;
            }
        }
    }

    public bool Remove(TKey key)
    {
        if(key == null) throw new ArgumentNullException(nameof(key));
        int index = GetHash(key);
        var node = FindNode(index, key);
        if (node == null) return false;
        buckets[index].Remove(node);
        count--;
        return true;
    }

    public bool Remove(KeyValuePair<TKey, TValue> item)
    {
        return Remove(item.Key);
    }

    public bool TryGetValue(TKey key, out TValue value)
    {
        if(key == null) throw new ArgumentNullException(nameof(key));
        int index = GetHash(key);
        var node = FindNode(index, key);
        if (node != null)
        {
            value = node.Value.Value;
            return true;
        }
        value = default;
        return false;
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    private LinkedList<KeyValuePair<TKey, TValue>> GetBucket(int index)
    {
        if(buckets[index] == null)
        {
            buckets[index] = new LinkedList<KeyValuePair<TKey, TValue>>();
        }
        return buckets[index];
    }
     

    private LinkedListNode<KeyValuePair<TKey, TValue>> FindNode(int index, TKey key)
    {
        var bucket = buckets[index];
        var current = bucket.First;
        while (current != null)
        {
            if (current.Value.Key.Equals(key))
            {
                return current;
            }
            current = current.Next;
        }
        return null;
    }

    private void Resize()
    {
        int newSize = bucketsize * 2;
        var newBuckets = new LinkedList<KeyValuePair<TKey, TValue>>[newSize];
        for (int i = 0; i < newSize; i++)
        {
            newBuckets[i] = new LinkedList<KeyValuePair<TKey, TValue>>();
        }
        foreach (var bucket in buckets)
        {
            foreach (var kvp in bucket)
            {
                int newIndex = (kvp.Key.GetHashCode() & 0x7FFFFFFF) % newSize;
                newBuckets[newIndex].AddLast(kvp);
            }
        }
        buckets = newBuckets;
        bucketsize = newSize;
    }
}
