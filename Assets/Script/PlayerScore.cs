using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Player))]
public class PlayerScore : MonoBehaviour {

    static int lastKills = 0;
    static int lastDeaths = 0;

    static Player player;
    public static bool syncedDone = false;
    public static bool Leaving = false;
    public delegate void OnDataSynced();
    private static OnDataSynced funcOnDataSynced;

    // Use this for initialization
    void Start () {
        player = GetComponent<Player>();
        StartCoroutine(SyncScoreLoop());
	}
    
    IEnumerator SyncScoreLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(10f);

            if(!Leaving)
                SyncNow(null);
        }
    }

    public static void SyncNow(OnDataSynced onDataSynced)
    {
        if (UserAccountManager.IsLoggedIn)
        {
            int killsSinceLast = player.kills - lastKills;
            int deathsSinceLast = player.deaths - lastDeaths;

            if (killsSinceLast <= lastKills && deathsSinceLast <= lastDeaths)
            {
                if (onDataSynced != null)
                    onDataSynced.Invoke();
                return;
            }
            funcOnDataSynced = onDataSynced;
            UserAccountManager.instance.SaveData(player, OnDataRecieved);
        }
    }

    static void OnDataRecieved()
    {
        lastKills = player.kills;
        lastDeaths = player.deaths;
        if (funcOnDataSynced != null)
            funcOnDataSynced.Invoke();
    }
}
