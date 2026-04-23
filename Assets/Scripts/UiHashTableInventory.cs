using TMPro;
using UnityEngine;

public class UiHashTableInventory : MonoBehaviour
{
    public TMP_Dropdown type;
    public TMP_Dropdown probing;

    public TMP_InputField keyInput;
    public TMP_InputField valueInput;
    public TMP_InputField log;

    public UiHashTableSlot uiHashTableSlot;

    public UiHashTableSlotList uiHashTableSlotList;

    private void OnEnable()
    {
        OnChangeType(type.value);
        OnChangeProbe(probing.value);
    }

    public void OnChangeType(int index)
    {
        uiHashTableSlotList.Type = (UiHashTableSlotList.HashTableType)index;
    }

    public void OnChangeProbe(int index)
    {
        uiHashTableSlotList.Probing = (UiHashTableSlotList.Probings)index;
    }

    public void OnAdd()
    {
        Debug.Log("Add");
        uiHashTableSlot.hashTableText.text = keyInput.text;
    }

    public void OnRemove()
    {
        Debug.Log("Remove");
    }

    public void OnClear()
    {
        Debug.Log("Clear");
    }
}