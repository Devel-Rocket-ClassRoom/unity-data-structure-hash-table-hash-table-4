using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LogManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI logText;
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private UiHashTableSlotList slotList;
    [SerializeField] private int simpleCapacity;
    [SerializeField] private int chainingCapacity;
    [SerializeField] private int openAddressingCapacity;

    [Header("Dropdown")]
    [SerializeField] private TMP_Dropdown hashTableTypes;
    [SerializeField] private TMP_Dropdown probes;

    [Header("Button")]
    [SerializeField] private Button addButton;
    [SerializeField] private Button removeButton;
    [SerializeField] private Button clearButton;

    [Header("InputField")]
    [SerializeField] private TMP_InputField keyInput;
    [SerializeField] private TMP_InputField valueInput;

    private SimpleHashTable<string, int> simpleHashTable;
    private ChainingHashTable<string, int> chainingHashTable;
    private OpenAddressingHashTable<string, int> openAddressingHashTable;

    private List<string> keys = new List<string>();

    public void Start()
    {
        hashTableTypes.onValueChanged.AddListener(OnHashTableTypeChanged);
        probes.onValueChanged.AddListener(OnProbeChanged);

        addButton.onClick.AddListener(OnAddButtonClicked);
        removeButton.onClick.AddListener(OnRemoveButtonClicked);
        clearButton.onClick.AddListener(OnClearButtonClicked);

        simpleHashTable = new SimpleHashTable<string, int>();
        chainingHashTable = new ChainingHashTable<string, int>();
        openAddressingHashTable = new OpenAddressingHashTable<string, int>();

        simpleCapacity = simpleHashTable.Capacity;
        chainingCapacity = chainingHashTable.Capacity;
        openAddressingCapacity = openAddressingHashTable.Capacity;

        probes.interactable = (probes.value == 2);
    }

    public void OnHashTableTypeChanged(int index)
    {
        string selected = hashTableTypes.options[index].text;
        probes.interactable = (index == 2);
        OnClearButtonClicked();
        sendText($"충돌 타입 변경: {selected}");
    }

    public void OnProbeChanged(int index)
    {
        string selected = probes.options[index].text;
        sendText($"Probe 변경: {selected}");
    }

    public void OnAddButtonClicked()
    {
        string key = keyInput.text;
        int value = int.Parse(valueInput.text);
        if (string.IsNullOrEmpty(key) || string.IsNullOrEmpty(value.ToString()))
        {
            return;
        }

        int simpleIndex = simpleHashTable.GetHash(key) % simpleCapacity;
        int chainingIndex = chainingHashTable.GetHash(key) % chainingCapacity;
        int openAddressingIndex = openAddressingHashTable.GetHash(key) % openAddressingCapacity;

        switch (hashTableTypes.value)
        {
            case 0:
                try
                {
                    slotList.SetCapacity(simpleCapacity);
                    simpleHashTable.Add(key, value);
                    keys.Add(key);
                    slotList.SetSlotData(simpleIndex, key, value);
                    slotList.uiSlotList[simpleIndex].keys.Add(key);
                    sendText($"ADD: {key} -> {value}");
                }
                catch (ArgumentOutOfRangeException)
                {
                    sendText("ADD 실패: 해쉬 충돌");
                }
                catch
                {
                    new ArgumentException();
                    sendText("ADD 실패: 키 충돌");
                }
                break;

            case 1:
                slotList.SetCapacity(chainingCapacity);
                chainingHashTable.Add(key, value);
                keys.Add(key);
                slotList.SetSlotData(chainingIndex, key, value);
                slotList.uiSlotList[chainingIndex].keys.Add(key);
                sendText($"ADD: {key} -> {value}");
                break;

            case 2:
                slotList.SetCapacity(openAddressingCapacity);
                openAddressingHashTable.Add(key, value);
                keys.Add(key);
                slotList.SetSlotData(openAddressingIndex, key, value);
                slotList.uiSlotList[openAddressingIndex].keys.Add(key);
                sendText($"ADD: {key} -> {value}");
                break;
        }
    }

    public void OnRemoveButtonClicked()
    {
        string key = keyInput.text;
        if (string.IsNullOrEmpty(key))
        {
            return;
        }

        switch (hashTableTypes.value)
        {
            case 0:
                simpleHashTable.Remove(key);
                break;

            case 1:
                chainingHashTable.Remove(key);
                break;

            case 2:
                openAddressingHashTable.Remove(key);
                break;
        }

        keys.Remove(key);
        SetEmpty(key);
    }

    public void OnRemoveButtonClicked(string key)
    {
        if (string.IsNullOrEmpty(key))
        {
            return;
        }

        switch (hashTableTypes.value)
        {
            case 0:
                simpleHashTable.Remove(key);
                break;

            case 1:
                chainingHashTable.Remove(key);
                break;

            case 2:
                openAddressingHashTable.Remove(key);
                break;
        }

        SetEmpty(key);
    }

    public void OnClearButtonClicked()
    {
        simpleHashTable.Clear();
        chainingHashTable.Clear();
        openAddressingHashTable.Clear();

        keys.Clear();
        slotList.SetEmpty();
        sendText("CLEAR: 모든 항목 삭제됨");
    }

    public void sendText(string message)
    {
        logText.text += message + "\n";

        Canvas.ForceUpdateCanvases();
        scrollRect.verticalNormalizedPosition = 0f;
    }

    public void SetEmpty(string key)
    {
        foreach (var slot in slotList.uiSlotList)
        {
            if (slot.keys.Contains(key))
            {
                slot.keys.Remove(key);

                if (slot.keys.Count == 0)
                {
                    slot.SetEmpty();
                }
                else
                {
                    slot.hashTableText.text = string.Join(", ", slot.keys);
                }

                break;
            }
        }
    }
}