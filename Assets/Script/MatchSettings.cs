using UnityEngine.Networking;

[System.Serializable]
public class MatchSettings {

    public float respawnTime = 3f;

    [SyncVar]
    public string roomName;

    [SyncVar]
    public int scoreLimit;

}
