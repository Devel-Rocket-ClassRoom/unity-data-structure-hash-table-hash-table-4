using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiHashTableSlot : MonoBehaviour
{
    public int slotIndex = -1;

    public TextMeshProUGUI hashTableText;
    public Button slotButton;

    public void SetData(string key, string value)
    {
        hashTableText.text = $"Key: {key}, Value: {value}";
    }
}