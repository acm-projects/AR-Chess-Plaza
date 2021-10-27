using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{

    static string previousScene = "Game Mode Screen";

    public void getPreviousScene(){
        previousScene = SceneManager.GetActiveScene().name;
        Debug.Log("Previous Scene: " + previousScene);
    }

    public void loadSceneWithName(string sceneName){
        getPreviousScene();
        SceneManager.LoadScene(sceneName);
    }

    public void loadPreviousScene(){
        SceneManager.LoadScene(previousScene);
    }

}
