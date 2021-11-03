using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameplayManager : MonoBehaviour
{
    // Start is called before the first frame update
    public Component playerNameText;
    public Component computerNameText;

    void Start()
    {
        playerNameText.GetComponent<Text>().text = PlayerPrefs.GetString("playerName");
        computerNameText.GetComponent<Text>().text = PlayerPrefs.GetString("computerName");

    }


}
