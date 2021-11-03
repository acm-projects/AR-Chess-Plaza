using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{

    public static string PreviousScene = "Game Mode Screen";

    private void Awake()
    {
        if (!PlayerPrefs.HasKey("playerName"))
        {
            PlayerPrefs.SetString("playerName", "Player Name");
        }

        if (!PlayerPrefs.HasKey("computerName"))
        {
            PlayerPrefs.SetString("computerName", "Computer");
        }

        if (!PlayerPrefs.HasKey("difficultyIndx"))
        {
            PlayerPrefs.SetInt("difficultyIndx", 0);
        }

        if (!PlayerPrefs.HasKey("gamemodeIndx"))
        {
            PlayerPrefs.SetInt("gamemodeIndx", 0);
        }

        if (!PlayerPrefs.HasKey("themeIndx"))
        {
            PlayerPrefs.SetInt("themeIndx", 0);
        }

        PlayerPrefs.Save();
    }

    public void getPreviousScene(){
        PreviousScene = SceneManager.GetActiveScene().name;
        Debug.Log("Previous Scene: " + PreviousScene);
    }

    public void loadSceneWithName(string sceneName){
        getPreviousScene();
        SceneManager.LoadScene(sceneName);
    }

    public void loadPreviousScene(){
        SceneManager.LoadScene(PreviousScene);
    }

}
