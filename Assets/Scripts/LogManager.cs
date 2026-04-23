using System.Collections.Generic;
using TMPro;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.UI;

public class LogManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI logText;
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private TMP_Dropdown dropdown;
    [SerializeField] private TMP_Dropdown dropdown2;
    [SerializeField] private Button button;
    [SerializeField] private Queue<string> logQueue = new Queue<string>();

    public void Start()
    {
        dropdown.onValueChanged.AddListener(OnDropdownChanged);
        dropdown2.onValueChanged.AddListener(OnDropdownChanged);

        button.onClick.AddListener(OnButtonClicked);
    }

    public void OnDropdownChanged(int index)
    {
        string selected = dropdown.options[index].text;
        sendText($"드롭다운 변경: {selected}");
    }

    public void OnButtonClicked()
    {
        sendText($"버튼 클릭됨");
    }

    public void sendText(string message)
    {
        logText.text += message + "\n";
        Canvas.ForceUpdateCanvases();
        scrollRect.verticalNormalizedPosition = 0f;
    }
}
