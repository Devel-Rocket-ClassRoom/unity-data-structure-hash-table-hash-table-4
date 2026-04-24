using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiHashTableSlot : MonoBehaviour
{
    public int slotIndex = -1;

    public TextMeshProUGUI indexText;
    public TextMeshProUGUI hashTableText;
    public Button slotButton;

    public void SetData(string key, int value)
    {
        hashTableText.text = $"K: {key}, V: {value}";
    }

    public void SetEmpty()
    {
        hashTableText.text = $"K: , V: ";
    }

    public void SetIndexText(int index)
    {
        indexText.text = $"I: {slotIndex}";
    }
}