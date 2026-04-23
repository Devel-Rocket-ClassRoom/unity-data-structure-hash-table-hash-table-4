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

    private List<UiHashTableSlot> uiSlotList = new List<UiHashTableSlot>();

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

    public void UpdateSlots()
    {
        Debug.Log("UpdateSlot");
    }
}