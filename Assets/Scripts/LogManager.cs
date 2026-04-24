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
    [SerializeField] private Queue<string> logQueue = new Queue<string>();

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
        
        simpleHashTable = new SimpleHashTable<string, int>(simpleHashTable.Capacity);
        chainingHashTable = new ChainingHashTable<string, int>(chainingHashTable.Capacity);
        openAddressingHashTable = new OpenAddressingHashTable<string, int>(openAddressingHashTable.Count);

        simpleCapacity = simpleHashTable.Capacity;
        chainingCapacity = chainingHashTable.Capacity;
        openAddressingCapacity = openAddressingHashTable.Count;
    }

    public void OnHashTableTypeChanged(int index)
    {
        string selected = hashTableTypes.options[index].text;
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

        int simpleIndex = Mathf.Abs(key.GetHashCode()) % simpleCapacity;
        int chainingIndex = Mathf.Abs(key.GetHashCode()) % chainingCapacity;
        int openAddressingIndex = Mathf.Abs(key.GetHashCode()) % openAddressingCapacity;

        switch (hashTableTypes.value)
        {
            case 0:
                simpleHashTable.Add(key, value);
                keys.Add(key);
                slotList.SetSlotData(simpleIndex, key, value);
                slotList.uiSlotList[simpleIndex].keys.Add(key);
                sendText("Simple");
                break;

            case 1:
                chainingHashTable.Add(key, value);
                keys.Add(key);
                slotList.SetSlotData(chainingIndex, key, value);
                slotList.uiSlotList[chainingIndex].keys.Add(key);
                sendText("Chaining");
                break;

            case 2:
                openAddressingHashTable.Add(key, value);
                keys.Add(key);
                slotList.SetSlotData(openAddressingIndex, key, value);
                slotList.uiSlotList[openAddressingIndex].keys.Add(key);
                sendText("OpenAddressing");
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
                SetEmpty(key);
                sendText("Remove Simple");
                break;

            case 1:
                chainingHashTable.Remove(key);
                SetEmpty(key);
                sendText("Remove Chaining");
                break;

            case 2:
                openAddressingHashTable.Remove(key);
                SetEmpty(key);
                sendText("Remove OpenAddressing");
                break;
        }
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
                SetEmpty(key);
                sendText("Remove Simple");
                break;

            case 1:
                chainingHashTable.Remove(key);
                SetEmpty(key);
                sendText("Remove Chaining");
                break;

            case 2:
                openAddressingHashTable.Remove(key);
                SetEmpty(key);
                sendText("Remove OpenAddressing");
                break;
        }
    }

    public void OnClearButtonClicked()
    {
        slotList.SetEmpty();
        foreach (var key in keys)
        {
            OnRemoveButtonClicked(key);
        }
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
    }
}
