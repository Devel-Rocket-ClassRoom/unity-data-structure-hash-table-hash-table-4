using TMPro;
using UnityEngine;

public class UiHashTableInventory : MonoBehaviour
{
    public TMP_Dropdown type;
    public TMP_Dropdown probing;

    public TMP_InputField keyInput;
    public TMP_InputField valueInput;
    public TMP_InputField log;

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
        uiHashTableSlotList.Probing = (UiHashTableSlotList.ProbingStrategy)index;
    }

    public void OnAdd()
    {
        string key = keyInput.text;
        string value = valueInput.text;
        if (string.IsNullOrEmpty(key))
        {
            return;
        }

        int index = Mathf.Abs(key.GetHashCode()) % uiHashTableSlotList.uiSlotList.Count;
        uiHashTableSlotList.SetSlotData(index, key, value);
        Debug.Log("Add");
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