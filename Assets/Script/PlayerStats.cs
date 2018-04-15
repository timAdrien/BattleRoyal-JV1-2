
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{

    public Text killCount;
    public Text deathCount;
    public Text ratio;

    // Use this for initialization
    void Start()
    {
        if (UserAccountManager.IsLoggedIn)
            UserAccountManager.instance.GetData(OnReceivedData);
    }

    void OnReceivedData(string data)
    {
        Debug.Log("data GETED: "+data);
        int kills = DataTranslator.DataToKills(data);
        int deaths = DataTranslator.DataToDeaths(data);

        killCount.text = kills.ToString() + " kills";
        deathCount.text = deaths.ToString() + " morts";

        if (deaths <= 0)
            deaths = 1;
        ratio.text = "ratio : " + ((float)(kills / deaths)).ToString();
    }
}
