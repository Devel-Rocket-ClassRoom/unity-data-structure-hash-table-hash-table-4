using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System.Linq;

public class SImpleHashTable<TKey, TValue> : IDictionary<TKey, TValue> where TKey : IComparable<TKey>
{
    protected HashTable<TKey, TValue>[] root;
    protected int size;
    public SImpleHashTable()
    {
        root = null;
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

        }
    }

    public ICollection<TKey> Keys => throw new System.NotImplementedException();

    public ICollection<TValue> Values => throw new System.NotImplementedException();

    public int Count => throw new System.NotImplementedException();

    public bool IsReadOnly => throw new System.NotImplementedException();

    public void Add(TKey key, TValue value)
    {
        int index = GetHashIndex(key);
        if (root[index] == null)
        {
            root[index] = new HashTable<TKey, TValue>(key, value);
            size++;
        }
        else 
        {
            HashTable<TKey, TValue> current = root[index];
            while (current != null)
            {
                if (current.Key.CompareTo(key) == 0) throw new ArgumentException("중복 키!");
                if (current.Next == null) break;
                current = current.Next;
            }
            current.Next = new HashTable<TKey, TValue>(key, value);
            size++;
        }
    }
    private int GetHashIndex(TKey key)
    {

        return Mathf.Abs(key.GetHashCode());
    }
    public void Add(KeyValuePair<TKey, TValue> item)
    {
        throw new System.NotImplementedException();
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
        throw new System.NotImplementedException();
    }

    public bool Remove(TKey key)
    {
        int index = GetHashIndex(key);
        HashTable<TKey, TValue> current = root[index];
        HashTable<TKey, TValue> prev = null; 
       
        while (current != null)
        {
         
            if (current.Key.CompareTo(key) == 0)
            {
            
                if (prev == null)
                {
                    root[index] = current.Next; 
                }
      
                else
                {
                    prev.Next = current.Next; 
                }

                size--; 
                return true; 
            }
            prev = current;
            current = current.Next;
        }

        return false;
    }

    public bool Remove(KeyValuePair<TKey, TValue> item)
    {
        throw new System.NotImplementedException();
    }

    public bool TryGetValue(TKey key, out TValue value)
    {
        int index = GetHashIndex(key);
        return TryGetValue(root[index], key,out value);
    }
    protected bool TryGetValue(HashTable<TKey, TValue> node, TKey key, out TValue value)
    {
        HashTable<TKey, TValue> current = node;
        while (current != null)
        {
            if (key.CompareTo(current.Key) == 0)
            {
                value = current.Value;
                return true;
            }
            current = current.Next;
        }
        value = default;
        return false;
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}