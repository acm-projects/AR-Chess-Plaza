using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SinglePlayerScript : MonoBehaviour
{
    #region Variables
    //Color Choice Objects
    public Sprite whiteColorChoice;
    public Sprite blackColorChoice;

    public Component topColorChoice;
    public Component bottomColorChoice;

    //Textfield Objects
    public Component playerNameTextField;
    public Component computerNameTextField;

    //Dropdown Objects
    public Component difficultyChoice;
    public Component gamemodeChoice;
    public Component themeChoice;
    #endregion

    void Start()
    {
        //Sets color choices
        GameManager.Instance.playerColor = "White";
        GameManager.Instance.computerColor = "Black";
        topColorChoice.GetComponent<Image>().sprite = whiteColorChoice;
        bottomColorChoice.GetComponent<Image>().sprite = blackColorChoice;

        //Sets textfields
        playerNameTextField.GetComponent<InputField>().text = PlayerPrefs.GetString("playerName");
        computerNameTextField.GetComponent<InputField>().text = PlayerPrefs.GetString("computerName");

        //Sets dropdown choices
        difficultyChoice.GetComponent<Dropdown>().value = PlayerPrefs.GetInt("difficultyIndx");
        gamemodeChoice.GetComponent<Dropdown>().value = PlayerPrefs.GetInt("gamemodeIndx");
        themeChoice.GetComponent<Dropdown>().value = PlayerPrefs.GetInt("themeIndx");
    }

    //Color Switcher
    public void SwitchColors()
    {
        //Switches Global Color Variables
        string tempColor = GameManager.Instance.playerColor;
        GameManager.Instance.playerColor = GameManager.Instance.computerColor;
        GameManager.Instance.computerColor = tempColor;
        Debug.Log("Player Color: " + GameManager.Instance.playerColor);


        //Stores Component for switch
        Sprite tempSprite = topColorChoice.GetComponent<Image>().sprite;

        topColorChoice.GetComponent<Image>().sprite = bottomColorChoice.GetComponent<Image>().sprite;

        bottomColorChoice.GetComponent<Image>().sprite = tempSprite;      
    }

    #region Textfield Methods
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

    public void ComputerTextFieldChanged(string newText)
    {
        if (newText != "")
        {
            PlayerPrefs.SetString("computerName", newText);
            PlayerPrefs.Save();
        }
        else
        {
            Debug.Log("Textfield empty");
            computerNameTextField.GetComponent<InputField>().text = PlayerPrefs.GetString("computerName");

        }
    }
    #endregion

    #region Dropdown Methods
    public void DifficultyChanged(int newInt)
    {
        PlayerPrefs.SetInt("difficultyIndx", newInt);
        PlayerPrefs.Save();
    }

    public void GamemodeChanged(int newInt)
    {
        PlayerPrefs.SetInt("gamemodeIndx", newInt);
        PlayerPrefs.Save();
    }

    public void ThemeChanged(int newInt)
    {
        PlayerPrefs.SetInt("themeIndx", newInt);
        PlayerPrefs.Save();
    }
    #endregion

}
