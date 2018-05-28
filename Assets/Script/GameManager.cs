using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class GameManager : MonoBehaviour {

	public static GameManager instance;

    [SerializeField]
    private GameObject sceneCamera;

    public delegate void OnPlayerKilledCallback(string player, string source);
    public OnPlayerKilledCallback onPlayerKillCallback;

    void Awake ()
	{
		if (instance != null)
		{
			Debug.LogError("More than one GameManager in scene.");
		} else
		{
			instance = this;
        }
	}

    public void SetSceneCameraState(bool isActive)
    {
        if (sceneCamera != null)
        {
            sceneCamera.SetActive(isActive);
        }
    }

	#region Player tracking

	private const string PLAYER_ID_PREFIX = "Player ";

    private static Dictionary<string, Player> players = new Dictionary<string, Player>();

    public static void RegisterPlayer (string _netID, Player _player)
    {
        string _playerID = PLAYER_ID_PREFIX + _netID;
        players.Add(_playerID, _player);
        _player.transform.name = _playerID;
        if (players.Count() == 3)
        {
            foreach(Player player in players.Values)
            {
                player.ReadyToPlay = true;
            }
        }
    }

    public static void UnRegisterPlayer (string _playerID)
    {
        players.Remove(_playerID);
    }

    public static Player GetPlayer(string _playerID)
    {
        return players[_playerID];
    }

    public static Player[] GetPlayers()
    {
        return players.Values.ToArray();
    }

    #endregion

}
