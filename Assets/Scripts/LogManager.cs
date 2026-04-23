using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LogManager : MonoBehaviour
{
    public TMP_InputField inputField;

    private Queue<string> logQueue = new Queue<string>();

    public  void sendText()
    {

    }
}
