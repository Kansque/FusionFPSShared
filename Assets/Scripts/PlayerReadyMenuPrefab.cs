using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerReadyMenuPrefab : MonoBehaviour
{
    public TextMeshProUGUI playerName, playerReady;

    private void Start()
    {
        transform.localScale = Vector3.one;
        transform.localPosition = Vector3.zero;
    }
}
