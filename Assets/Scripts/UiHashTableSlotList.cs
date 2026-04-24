using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UiHashTableSlotList : MonoBehaviour
{
    public enum HashTableType
    {
        Simple,
        Chaining,
        OpenAddressing
    }

    public enum ProbingStrategy
    {
        Linear,
        Quadratic,
        DoubleHash
    }

    public UnityEvent onUpdateSlots;

    public ScrollRect scrollRect;
    public UiHashTableSlot prefab;

    public List<UiHashTableSlot> uiSlotList = new List<UiHashTableSlot>();

    private HashTableType type = HashTableType.Simple;
    private ProbingStrategy probe = ProbingStrategy.Linear;

    public HashTableType Type
    {
        get => type;

        set
        {
            if (type != value)
            {
                type = value;
                UpdateSlots();
            }
        }
    }

    public ProbingStrategy Probing
    {
        get => probe;

        set
        {
            if (probe != value)
            {
                probe = value;
                UpdateSlots();
            }
        }
    }

    private void Awake()
    {
        uiSlotList.Capacity = 10;
        UpdateSlots();        
    }

    public void SetSlotData(int index, string key, int value)
    {
        if (index >= 0 && index < uiSlotList.Count)
        {
            uiSlotList[index].SetData(key, value);
        }
    }

    public void UpdateSlots()
    {
        foreach (var slot in uiSlotList)
        {
            if (slot != null)
            {
                Destroy(slot.gameObject);
            }
        }
        uiSlotList.Clear();

        for (int i = 0; i < uiSlotList.Capacity; i++)
        {
            UiHashTableSlot newSlot = Instantiate(prefab, scrollRect.content);
            newSlot.slotIndex = i;
            newSlot.SetEmpty();
            uiSlotList.Add(newSlot);
        }
    }

    public void SetEmpty()
    {
        uiSlotList.Clear();
        UpdateSlots();
    }
}