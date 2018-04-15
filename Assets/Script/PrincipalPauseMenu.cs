
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;

public class PrincipalPauseMenu : MonoBehaviour {

    public static bool isOn;
    private static NetworkManager networkManager;
    bool syncedDone;

    private void Start()
    {
        networkManager = NetworkManager.singleton;
    }

    public void LeaveRoom()
    {
        Debug.Log("Saving before leave");
        PlayerScore.Leaving = true;
        PlayerScore.SyncNow(OnDataSaved);
    }

    static void OnDataSaved()
    {
        Debug.Log("Leaved");
        MatchInfo matchInfo = networkManager.matchInfo;
        networkManager.matchMaker.DropConnection(matchInfo.networkId, matchInfo.nodeId, 0, networkManager.OnDropConnection);
        networkManager.StopHost();
    }
}
