using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InGameMessageUI : MonoBehaviour
{
    public TextMeshProUGUI[] textMeshProUGUIs;
    Queue messageQueue = new Queue();

    private void Start()
    {

    }

    public void OnGameMessageReceived(string message)
    {
        messageQueue.Enqueue(message);

        if (messageQueue.Count > 3)
            messageQueue.Dequeue();

        int index = 0;
        foreach (string messageInQueue in messageQueue)
        {
            textMeshProUGUIs[index].text = messageInQueue;
            index++;
        }
    }
}
