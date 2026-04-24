using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LogManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI logText;
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private UiHashTableSlotList slotList;
    [SerializeField] private int capacity;

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

    public void Start()
    {
        hashTableTypes.onValueChanged.AddListener(OnHashTableTypeChanged);
        probes.onValueChanged.AddListener(OnProbeChanged);

        addButton.onClick.AddListener(OnAddButtonClicked);
        removeButton.onClick.AddListener(OnRemoveButtonClicked);
        clearButton.onClick.AddListener(OnClearButtonClicked);

        capacity = slotList.uiSlotList.Count;

        simpleHashTable = new SimpleHashTable<string, int>(capacity);
        chainingHashTable = new ChainingHashTable<string, int>(capacity);
        openAddressingHashTable = new OpenAddressingHashTable<string, int>(capacity);
    }

    public void OnHashTableTypeChanged(int index)
    {
        string selected = hashTableTypes.options[index].text;
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
        if (string.IsNullOrEmpty(key))
        {
            return;
        }

        int index = Mathf.Abs(key.GetHashCode()) % capacity;

        switch (hashTableTypes.value)
        {
            case 0:
                simpleHashTable.Add(key, value);
                slotList.SetSlotData(index, key, value);
                sendText("Simple");
                break;

            case 1:
                chainingHashTable.Add(key, value);
                slotList.SetSlotData(index, key, value);
                sendText("Chaining");
                break;

            case 2:
                openAddressingHashTable.Add(key, value);
                slotList.SetSlotData(index, key, value);
                sendText("OpenAddressing");
                break;
        }
    }

    public void OnRemoveButtonClicked()
    {
        sendText($"Remove 버튼 클릭됨");
    }

    public void OnClearButtonClicked()
    {
        var lists = slotList.uiSlotList;
        foreach (var slot in lists)
        {
            slot.SetEmpty();
        }
        sendText("Clear 버튼 클릭됨");
    }

    public void sendText(string message)
    {
        logText.text += message + "\n";

        Canvas.ForceUpdateCanvases();
        scrollRect.verticalNormalizedPosition = 0f;
    }
}
