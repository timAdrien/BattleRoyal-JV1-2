using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using System.Collections;

public class JoinGame : MonoBehaviour {

    private const int COUNTDOWN_JOINING = 5;

    List<GameObject> roomList = new List<GameObject>();
    private NetworkManager networkManager;

    [SerializeField]
    private Text status;

    [SerializeField]
    private GameObject roomListItemPrefab;

    [SerializeField]
    private Transform roomListParent;

    void Start ()
    {
        networkManager = NetworkManager.singleton;
        if(networkManager.matchMaker == null)
        {
            networkManager.StartMatchMaker();
        }

        RefreshRoomList();
	}

    public void RefreshRoomList()
    {
        ClearRoomList();
        if (networkManager.matchMaker == null)
        {
            networkManager.StartMatchMaker();
        }
        networkManager.matchMaker.ListMatches(0, 20,"", false, 0, 0, OnMatchList);
        status.text = "Loading";
    }
	
    public void OnMatchList(bool success, string extendedInfo, List<MatchInfoSnapshot> matches)
    {
        status.text = "";
        if(matches == null)
        {
            status.text = "Erreur de chargement...";
            return;
        }

        ClearRoomList();

        foreach(MatchInfoSnapshot match in matches)
        {
            GameObject roomListItemGO = Instantiate(roomListItemPrefab);
            roomListItemGO.transform.SetParent(roomListParent);

            RoomListItem roomListItem = roomListItemGO.GetComponent<RoomListItem>();
            if(roomListItem != null)
            {
                roomListItem.Setup(match, JoinRoom);
            }

            roomList.Add(roomListItemGO);
        }

        if(roomList.Count == 0)
        {
            status.text = "Aucune partie...";
        }
    }

    public void ClearRoomList()
    {
        for (int i = 0; i < roomList.Count; i++)
        {
            Destroy(roomList[i]);
        }

        roomList.Clear();
    }

    public void JoinRoom(MatchInfoSnapshot match)
    {
        networkManager.matchMaker.JoinMatch(match.networkId, "", "", "", 0, 0, networkManager.OnMatchJoined);
        StartCoroutine(WaitForJoin());
    }

    IEnumerator WaitForJoin()
    {
        ClearRoomList();
        status.text = "Chargement de la partie...";

        int countdown = COUNTDOWN_JOINING;

        while (countdown > 0 )
        {
            status.text = "Chargement de la partie... (" + countdown + ")";
            yield return new WaitForSeconds(1f);
            countdown--;
        }

        // Fail connexion
        status.text = "Erreur de chargement de la partie...";
        yield return new WaitForSeconds(1f);

        MatchInfo matchInfo = networkManager.matchInfo;
        if (matchInfo != null)
        {
            networkManager.matchMaker.DropConnection(matchInfo.networkId, matchInfo.nodeId, 0, networkManager.OnDropConnection);
            networkManager.StopHost();
        }

        RefreshRoomList();
    }
}
