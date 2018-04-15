using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using DatabaseControl;

public class UserAccountManager : MonoBehaviour
{
    public static UserAccountManager instance;

    //These store the username and password of the player when they have logged in
    public static string PlayerUsername { get; protected set; }
    private static string PlayerPassword = "";

    public static bool IsLoggedIn { get; protected set; }

    public string loggedInSceneName = "Lobby";
    public string loggedOutSceneName = "LoginMenu";

    public delegate void OnDataReceiveidCallback(string data);
    public delegate void OnDataSavedCallback();

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;

        // Pour transferer à d'autres scenes
        DontDestroyOnLoad(this);
    }

    public void LogIn(string username, string password)
    {
        PlayerUsername = username;
        PlayerPassword = password;
        IsLoggedIn = true;

        SceneManager.LoadScene(loggedInSceneName);
    }

    public void LogOut()
    {
        PlayerUsername = "";
        PlayerPassword = "";
        IsLoggedIn = false;
        SceneManager.LoadScene(loggedOutSceneName);
    }

    public void SaveData(Player player, OnDataSavedCallback onDataReceiveid)
    {
        if (IsLoggedIn)
        {
            StartCoroutine(sendSaveData(player, onDataReceiveid));
        }
    }

    public void GetData(OnDataReceiveidCallback onDataReceiveid)
    {
        if (IsLoggedIn)
        {
            StartCoroutine(sendGetData(onDataReceiveid));
        }
    }

    public void SetData(string data, OnDataSavedCallback onDataReceiveid)
    {
        if (IsLoggedIn)
        {
            StartCoroutine(sendSetData(data, onDataReceiveid));
        }
    }

    IEnumerator sendSaveData(Player player, OnDataSavedCallback onDataReceiveid)
    {
        IEnumerator e = DCF.GetUserData(PlayerUsername, PlayerPassword); // << Send request to get the player's data string. Provides the username and password
        while (e.MoveNext())
        {
            yield return e.Current;
        }
        string response = e.Current as string; // << The returned string from the request

        if (response == "Error")
        {
            Debug.Log("Une erreur est survenue lors de l'envoi des données...");
        }
        else
        {
            int kills = DataTranslator.DataToKills(response);
            int deaths = DataTranslator.DataToDeaths(response);

            int newKills = player.kills + kills;
            int newDeaths = player.deaths + deaths;

            string newData = DataTranslator.ValuesToData(newKills, newDeaths);
            SetData(newData, onDataReceiveid);
        }
    }

    IEnumerator sendGetData(OnDataReceiveidCallback onDataReceiveid)
    {
        string data = "ERROR";
        IEnumerator e = DCF.GetUserData(PlayerUsername, PlayerPassword); // << Send request to get the player's data string. Provides the username and password
        while (e.MoveNext())
        {
            yield return e.Current;
        }
        string response = e.Current as string; // << The returned string from the request

        if (response == "Error")
        {
            Debug.Log("Une erreur est survenue lors de l'envoi des données...");
        }
        else
        {
            data = response;
        }
        if (onDataReceiveid != null)
            onDataReceiveid.Invoke(data);
    }

    IEnumerator sendSetData(string data, OnDataSavedCallback onDataReceiveid)
    {
        IEnumerator e = DCF.SetUserData(PlayerUsername, PlayerPassword, data); // << Send request to set the player's data string. Provides the username, password and new data string
        while (e.MoveNext())
        {
            yield return e.Current;
        }
        string response = e.Current as string; // << The returned string from the request

        if (response == "Success")
        {
            // GOOD
        }
        else
        {
            Debug.Log("Une erreur est survenue lors de l'envoi des données...");
        }
        if (onDataReceiveid != null)
            onDataReceiveid.Invoke();
    }
}
