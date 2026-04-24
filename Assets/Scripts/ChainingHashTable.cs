using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainingHashTable<TKey, TValue> : IDictionary<TKey, TValue>
{
    private LinkedList<KeyValuePair<TKey, TValue>>[] buckets;

    private int size;
    private int count;

    private const int DefaultCapacity = 16;
    private const double LoadFactor = 0.75;
    public int Capacity => size;  

    public int Count => count;

    public bool IsReadOnly => false;



    public ChainingHashTable(int capacity = DefaultCapacity)
    {
        size = capacity;
        buckets = new LinkedList<KeyValuePair<TKey, TValue>>[size];
        for (int i = 0; i < size; i++)
        {
            buckets[i] = new LinkedList<KeyValuePair<TKey, TValue>>();
        }
        count = 0;
    }//생성자

    public int GetHash(TKey key)
    {
        return (key.GetHashCode() & 0x7FFFFFFF) % size;//양수로 변환 후 배열 크기로 나눈 나머지
    }

    public TValue this[TKey key]
    {
        get
        {
            if(TryGetValue(key, out var value)) return value;
            throw new KeyNotFoundException("키 X");
        }
        set
        {
            if(key == null) throw new ArgumentNullException(nameof(key));
            int index = GetHash(key);
            var node = FindNode(index, key);

            if(node != null)
            {
               buckets[index].Remove(node);
               buckets[index].AddLast(new KeyValuePair<TKey, TValue>(key, value));
               return;
            }
            Add(key, value);
        }
    }


    public ICollection<TKey> Keys
    {
        get
        {
            List<TKey> keys = new List<TKey>();//키들을 저장할 리스트
            foreach (var bucket in buckets)
            {
                foreach (var kvp in bucket)
                {
                    keys.Add(kvp.Key);
                }
            }
            return keys;
        }
    }


    public ICollection<TValue> Values
    {
        get
        {
            List<TValue> values = new List<TValue>();
            foreach (var bucket in buckets)
            {
                foreach (var kvp in bucket)
                {
                    values.Add(kvp.Value);
                }
            }
            return values;
        }
    }


    public void Add(TKey key, TValue value)
    {
        if(key == null ) throw new ArgumentNullException(nameof(key));
        if((double)count / size > LoadFactor)
        {
            Resize();
        }
        int index = GetHash(key);
        var bucket = GetBucket(index);
        var node = FindNode(index, key);
        if (node != null)
        {
            node.Value = new KeyValuePair<TKey, TValue>(key, value);
        }
        else
        {
            bucket.AddLast(new KeyValuePair<TKey, TValue>(key, value));//새로운 키-값 쌍을 버킷의 연결 리스트에 추가
            count++;//전체 요소 수 증가
        }
    }

    public void Add(KeyValuePair<TKey, TValue> item)
    {
        Add(item.Key, item.Value);
    }

    public void Clear()
    {
        for(int i = 0; i < buckets.Length;i++)
        {
            buckets[i] = new LinkedList<KeyValuePair<TKey, TValue>>();
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
        if (array.Length - arrayIndex < count) throw new ArgumentException("배열의 크기가 충분하지 않습니다.");


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
        int newSize = size * 2;
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
        size = newSize;
    }
}
