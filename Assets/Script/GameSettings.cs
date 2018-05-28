using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GameSettings : NetworkBehaviour {

    public static GameSettings instance;

    [SyncVar]
    [SerializeField]
    private string roomName;

    [SyncVar]
    [SerializeField]
    private int scoreLimit = 5;

    [SyncVar]
    [SerializeField]
    private int respawnTime = 3;

    #region Accesseurs variables
    public string RoomName
    {
        get
        {
            if (instance == null)
            {
                instance = this;
            }
            return instance.roomName;
        }

        set
        {
            if (instance == null)
            {
                instance = this;
            }
            instance.roomName = value;
        }
    }
    public int ScoreLimit
    {
        get
        {
            if (instance == null)
            {
                instance = this;
            }
            return instance.scoreLimit;
        }

        set
        {
            if (instance == null)
            {
                instance = this;
            }
            instance.scoreLimit = value;
        }
    }

    public int RespawnTime
    {
        get
        {
            return respawnTime;
        }

        set
        {
            respawnTime = value;
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
