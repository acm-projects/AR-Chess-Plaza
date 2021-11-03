using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System;

public class GameManager : MonoBehaviour
{
    #region Variables
    public string playerColor;
    public string computerColor;

    public int themeIndx = PlayerPrefs.GetInt("themeIndx");
    #endregion

    #region Singleton
    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this.gameObject);
        }

    }
    #endregion

    //Checks for empty data and resets values to default
    public void ResetData()
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
    #region Save System (not in use yet)
    public void SaveData()
    {
        //Sets file path
        string path = Application.persistentDataPath + "/gamedata.acm";

        //Instantiates binary formatter
        BinaryFormatter formatter = new BinaryFormatter();

        //Creates FileStream Object which will WIRTE operations
        FileStream stream = new FileStream(path, FileMode.Create);

        //Serializes data into file path
        formatter.Serialize(stream, this);
        Debug.Log("Data written to " + path + " @ " + DateTime.Now.ToShortTimeString());

        //Closes FileStream
        stream.Close();
    }

    public void LoadData()
    {
        //Sets file path
        string path = Application.persistentDataPath + "/gamedata.acm";

        if (File.Exists(path))
        {
            //Instantiates binary formatter
            BinaryFormatter formatter = new BinaryFormatter();

            //Creates FileStream Object which will READ operations
            FileStream stream = new FileStream(path, FileMode.Open);

            //Serializes data into file path
            Instance = formatter.Deserialize(stream) as GameManager;
            Debug.Log("Data read from " + path + " @ " + DateTime.Now.ToShortTimeString());

            //Closes FileStream
            stream.Close();

        }
        else
        {
            //Reports empty file path
            Debug.LogError("Save file not found in " + path);
        }


    }

    #endregion

}
