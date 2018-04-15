
using UnityEngine;
using UnityEngine.UI;

public class UserAccount_Lobby : MonoBehaviour {

    public Text usernameText;

    private void Start()
    {
        if (UserAccountManager.IsLoggedIn)
            usernameText.text = UserAccountManager.PlayerUsername;
    }

    public void LogOut()
    {
        if (UserAccountManager.IsLoggedIn)
            UserAccountManager.instance.LogOut();
    }
}
