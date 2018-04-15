
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class HostGame : MonoBehaviour {

    [SerializeField]
    private uint roomSize = 6;

    private NetworkManager networkManager;

    [SerializeField]
    private InputField roomNameText;
    [SerializeField]
    private Dropdown scoreLimitDropDown;

    private void Start()
    {
        networkManager = NetworkManager.singleton;
        if(networkManager.matchMaker == null)
        {
            networkManager.StartMatchMaker();
        }
        roomNameText.text = GameSettings.instance != null ? GameSettings.instance.RoomName : "not set";

        int index = 0;
        foreach(Dropdown.OptionData option in scoreLimitDropDown.options)
        {
            if (option.text == GameSettings.instance.ScoreLimit.ToString())
            {
                scoreLimitDropDown.value = index;
            }
            index++;
        }
    }

    public void SetRoomName(string name)
    {
        GameSettings.instance.RoomName = name;
    }

    public void SetScoreLimit(Dropdown change)
    {
        GameSettings.instance.ScoreLimit = int.Parse(change.captionText.text);
    }

    public void CreateRoom()
    {
        string roomName = GameSettings.instance.RoomName;
        if (roomName != "" && roomName != null)
        {
            Debug.Log("Creating Room: " + roomName + "avec " + roomSize + " joueur(s)");
            networkManager.matchMaker.CreateMatch(roomName, roomSize, true, "", "", "", 0, 0, networkManager.OnMatchCreate);
        }
    }
}
