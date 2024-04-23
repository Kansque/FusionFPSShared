using Fusion;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField]GameObject canvas;
    TMP_InputField nameInputField;
    Button playButton;


    private void Awake()
    {
        nameInputField = GetComponentInChildren<TMP_InputField>();
        playButton = GetComponentInChildren<Button>();
    }
    public void SubmitName()
    {
        NetworkInitializer.instance.ConnectToLobby(nameInputField.text);
        canvas.SetActive(false);
    }

    public void ActivateButton()
    {
        if(nameInputField.text.Length <= 1)
            playButton.interactable = false;
        else playButton.interactable = true;
    }
}
