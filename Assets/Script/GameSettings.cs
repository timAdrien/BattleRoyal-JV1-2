using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSettings : MonoBehaviour {

    public static GameSettings instance;
    
    private string roomName;
    private int scoreLimit;

    #region Accesseurs variables
    public string RoomName
    {
        get
        {
            return roomName;
        }

        set
        {
            roomName = value;
        }
    }
    public int ScoreLimit
    {
        get
        {
            return scoreLimit;
        }

        set
        {
            scoreLimit = value;
        }
    }
    #endregion

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;

        // Pour transferer à d'autres scenes
        DontDestroyOnLoad(this);
    }
}
