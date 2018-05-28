
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class HostGame : MonoBehaviour {

    [SerializeField]
    private uint roomSize = 6;

    private NetworkManager networkManager;

    [SerializeField]
    private GameObject gameSettingsPrefab;

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

        roomNameText.text = GameSettings.instance != null ? GameSettings.instance.RoomName : "";

        int index = 0;
        foreach(Dropdown.OptionData option in scoreLimitDropDown.options)
        {
            if (GameSettings.instance != null && option.text == GameSettings.instance.ScoreLimit.ToString())
            {
                scoreLimitDropDown.value = index;
            }
            index++;
        }
    }

    public void CreateRoom()
    {
        string roomName = roomNameText.text;
        if (roomName != "" && roomName != null)
        {
            Debug.Log("Creating Room: " + roomName + " avec " + roomSize + " joueur(s)");
            networkManager.matchMaker.CreateMatch(roomName, roomSize, true, "", "", "", 0, 0, networkManager.OnMatchCreate);
        }
    }

}
