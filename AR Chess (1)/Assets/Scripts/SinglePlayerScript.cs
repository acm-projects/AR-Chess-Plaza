using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SinglePlayerScript : MonoBehaviour
{
    public Sprite whiteColorChoice;
    public Sprite blackColorChoice;

    public Component topColorChoice;
    public Component bottomColorChoice;


    void Start()
    {
        topColorChoice.GetComponent<Image>().sprite = whiteColorChoice;
        bottomColorChoice.GetComponent<Image>().sprite = blackColorChoice;
    }

    public void SwitchColors()
    {
        //Stores Component for switch
        Sprite tempSprite = topColorChoice.GetComponent<Image>().sprite;

        topColorChoice.GetComponent<Image>().sprite = bottomColorChoice.GetComponent<Image>().sprite;

        bottomColorChoice.GetComponent<Image>().sprite = tempSprite;      
    }

    public void UsernameTextFieldChanged(string newText)
    {
        if (newText != "")
        {
            Debug.Log("Username text detected");
            Debug.Log(newText);
        }
        else
        {
            Debug.Log("Textfield empty");
        }
    }

    public void ComputerTextFieldChanged(string newText)
    {
        if (newText != "")
        {
            Debug.Log("Computer text detected");
            Debug.Log(newText);
        }
        else
        {
            Debug.Log("Textfield empty");
        }
    }

    /*public void getTopComp()
    {
        topColorChoice = GetComponent<Image>();
    }

    public void getBottomComp()
    {
        bottomColorChoice = GetComponent<Image>();
    }*/
}
