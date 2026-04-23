using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LogManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI logText;
    [SerializeField] private ScrollRect scrollRect;

    [Header("Dropdown")]
    [SerializeField] private TMP_Dropdown dropdown;
    [SerializeField] private TMP_Dropdown dropdown2;
    [Header("Button")]
    [SerializeField] private Button addButton;
    [SerializeField] private Button removeButton;
    [SerializeField] private Button clearButton;
    [SerializeField] private Queue<string> logQueue = new Queue<string>();

    public void Start()
    {
        dropdown.onValueChanged.AddListener(OnDropdownChanged);
        dropdown2.onValueChanged.AddListener(OnDropdown2Changed);

        addButton.onClick.AddListener(OnAddButtonClicked);
        removeButton.onClick.AddListener(OnRemoveButtonClicked);
        clearButton.onClick.AddListener(OnClearButtonClicked);
    }

    public void OnDropdownChanged(int index)
    {
        string selected = dropdown.options[index].text;
        sendText($"드롭다운 변경: {selected}");
    }

    public void OnDropdown2Changed(int index)
    {
        string selected = dropdown2.options[index].text;
        sendText($"드롭다운 변경: {selected}");
    }

    public void OnAddButtonClicked()
    {
        sendText($"Add 버튼 클릭됨");
    }

    public void OnRemoveButtonClicked()
    {
        sendText($"Remove 버튼 클릭됨");
    }

    public void OnClearButtonClicked()
    {
        sendText($"Clear 버튼 클릭됨");
    }

    public void sendText(string message)
    {
        logText.text += message + "\n";

        Canvas.ForceUpdateCanvases();
        scrollRect.verticalNormalizedPosition = 0f;
    }
}
