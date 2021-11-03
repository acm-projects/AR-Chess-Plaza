using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    public Component playerNameTextField;
    void Start()
    {
        playerNameTextField.GetComponent<InputField>().text = PlayerPrefs.GetString("playerName");
    }

    public void PlayerNameTextFieldChanged(string newText)
    {
        if (newText != "")
        {
            PlayerPrefs.SetString("playerName", newText);
            PlayerPrefs.Save();
        }
        else
        {
            Debug.Log("Textfield empty");
            playerNameTextField.GetComponent<InputField>().text = PlayerPrefs.GetString("playerName");

        }
    }

    public void ResetDataButtonPressed()
    {
        PlayerPrefs.DeleteAll();
        GameManager.Instance.ResetData();
        playerNameTextField.GetComponent<InputField>().text = PlayerPrefs.GetString("playerName");
    }
}
