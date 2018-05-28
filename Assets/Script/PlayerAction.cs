using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerAction : NetworkBehaviour {

    [SerializeField]
    private GameObject gameSettingsPrefab;

    #region SERVER

    private GameObject serverGameSettings;

    #endregion

    void Update () {

        RaycastHit hit;

        Camera cam = GetComponent<PlayerShoot>().GetCamPrincipale();
        doorRight.doorLooked = false;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, 3))
        {
            if (hit.collider.name == "doorCorners_right" || hit.collider.name == "doorCorners_left")
            {
                doorRight.doorLooked = true;
            }
        }

        if (!isLocalPlayer)
            return;

        if (Input.GetKeyDown(KeyCode.G))
        {
            CmdStartGame("", (int)(Random.value * 100));
        }
    }

    [Command]
    public void CmdStartGame(string pRoomName, int pScoreLimit)
    {
        serverGameSettings = Instantiate(gameSettingsPrefab);
        serverGameSettings.name = "GameSettings";
        serverGameSettings.GetComponent<GameSettings>().ScoreLimit = pScoreLimit;
        serverGameSettings.GetComponent<GameSettings>().RoomName = pRoomName;

        NetworkServer.Spawn(serverGameSettings);
    }
}
