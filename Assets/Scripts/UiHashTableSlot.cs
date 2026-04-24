using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiHashTableSlot : MonoBehaviour
{
    public List<string> keys = new List<string>();
    public int slotIndex = -1;

    public TextMeshProUGUI indexText;
    public TextMeshProUGUI hashTableText;
    public Button slotButton;

    public Color normalColor;
    public Color addColor;

    public void SetData(string key, int value)
    {
        hashTableText.text = $"K: {key}, V: {value}";
    }

    public void SetEmpty()
    {
        hashTableText.text = $"K: , V: ";
        SetNormalColor();
    }

    public void SetIndexText(int index)
    {
        indexText.text = $"I: {slotIndex}";
    }

    public void SetAddColor()
    {
        slotButton.image.color = Color.powderBlue;
        addColor = slotButton.image.color;
    }

    public void SetNormalColor()
    {
        slotButton.image.color = Color.white;
        normalColor = slotButton.image.color;
    }
}