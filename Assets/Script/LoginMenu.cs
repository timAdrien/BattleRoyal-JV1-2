﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DatabaseControl; // << Remember to add this reference to your scripts which use DatabaseControl

public class LoginMenu : MonoBehaviour
{

    //All public variables bellow are assigned in the Inspector

    //These are the GameObjects which are parents of groups of UI elements. The objects are enabled and disabled to show and hide the UI elements.
    public GameObject loginParent;
    public GameObject registerParent;
    public GameObject loadingParent;

    //These are all the InputFields which we need in order to get the entered usernames, passwords, etc
    public InputField Login_UsernameField;
    public InputField Login_PasswordField;
    public InputField Register_UsernameField;
    public InputField Register_PasswordField;
    public InputField Register_ConfirmPasswordField;
    //public InputField LoggedIn_DataInputField;
    //public InputField LoggedIn_DataOutputField;

    //These are the UI Texts which display errors
    public Text Login_ErrorText;
    public Text Register_ErrorText;
   

    //Called at the very start of the game
    void Awake()
    {
        ResetAllUIElements();
    }

    //Called by Button Pressed Methods to Reset UI Fields
    void ResetAllUIElements()
    {
        //This resets all of the UI elements. It clears all the strings in the input fields and any errors being displayed
        Login_UsernameField.text = "";
        Login_PasswordField.text = "";
        Register_UsernameField.text = "";
        Register_PasswordField.text = "";
        Register_ConfirmPasswordField.text = "";
        Login_ErrorText.text = "";
        Register_ErrorText.text = "";
    }

    //Called by Button Pressed Methods. These use DatabaseControl namespace to communicate with server.
    IEnumerator LoginUser()
    {
        IEnumerator e = DCF.Login(Login_UsernameField.text, Login_PasswordField.text); // << Send request to login, providing username and password
        while (e.MoveNext())
        {
            yield return e.Current;
        }
        string response = e.Current as string; // << The returned string from the request

        if (response == "Success")
        {
            // Logged in so transition to new scene
            UserAccountManager.instance.LogIn(Login_UsernameField.text, Login_PasswordField.text);

            //ResetAllUIElements();
            //loadingParent.gameObject.SetActive(false);
            //loggedInParent.gameObject.SetActive(true);
            //LoggedIn_DisplayUsernameText.text = "Connecté en tant que: " + playerUsername;
        }
        else
        {
            //Something went wrong logging in. Stop showing 'Loading...' and go back to LoginUI
            loadingParent.gameObject.SetActive(false);
            loginParent.gameObject.SetActive(true);
            if (response == "UserError")
            {
                //The Username was wrong so display relevent error message
                Login_ErrorText.text = "Error: Username non trouvé";
            }
            else
            {
                if (response == "PassError")
                {
                    //The Password was wrong so display relevent error message
                    Login_ErrorText.text = "Error: Password Incorrect";
                }
                else
                {
                    //There was another error. This error message should never appear, but is here just in case.
                    Login_ErrorText.text = "Error: erreur inconnue, réessayez plus tard.";
                }
            }
        }
    }
    IEnumerator RegisterUser()
    {
        IEnumerator e = DCF.RegisterUser(Register_UsernameField.text, Register_PasswordField.text, "[KILLS]0/[DEATHS]0"); // << Send request to register a new user, providing submitted username and password. It also provides an initial value for the data string on the account, which is "Hello World".
        while (e.MoveNext())
        {
            yield return e.Current;
        }
        string response = e.Current as string; // << The returned string from the request

        if (response == "Success")
        {
            // Registered in so transition to new scene
            UserAccountManager.instance.LogIn(Register_UsernameField.text, Register_PasswordField.text);

            //Username and Password were valid. Account has been created. Stop showing 'Loading...' and show the loggedIn UI and set text to display the username.
            //ResetAllUIElements();
            //loadingParent.gameObject.SetActive(false);
            //loggedInParent.gameObject.SetActive(true);
            //LoggedIn_DisplayUsernameText.text = "Connecté en tant que: " + playerUsername;
        }
        else
        {
            //Something went wrong logging in. Stop showing 'Loading...' and go back to RegisterUI
            loadingParent.gameObject.SetActive(false);
            registerParent.gameObject.SetActive(true);
            if (response == "UserError")
            {
                //The username has already been taken. Player needs to choose another. Shows error message.
                Register_ErrorText.text = "Error: Username déjà pris";
            }
            else
            {
                //There was another error. This error message should never appear, but is here just in case.
                Login_ErrorText.text = "Error: erreur inconnue, réessayez plus tard.";
            }
        }
    }

    //UI Button Pressed Methods
    public void Login_LoginButtonPressed()
    {
        //Check the lengths of the username and password. (If they are wrong, we might as well show an error now instead of waiting for the request to the server)
        if (Login_UsernameField.text.Length > 3)
        {
            if (Login_PasswordField.text.Length > 5)
            {
                //Username and password seem reasonable. Change UI to 'Loading...'. Start the Coroutine which tries to log the player in.
                loginParent.gameObject.SetActive(false);
                loadingParent.gameObject.SetActive(true);
                StartCoroutine(LoginUser());
            }
            else
            {
                //Password too short so it must be wrong
                Login_ErrorText.text = "Error: Password Incorrect";
            }
        }
        else
        {
            //Username too short so it must be wrong
            Login_ErrorText.text = "Error: Username Incorrect";
        }
    }
    public void Login_RegisterButtonPressed()
    {
        //Called when the player hits register on the Login UI, so switches to the Register UI
        ResetAllUIElements();
        loginParent.gameObject.SetActive(false);
        registerParent.gameObject.SetActive(true);
    }
    public void Register_RegisterButtonPressed()
    {
        //Get the username and password and repeated password the player entered
        string confirmedPassword = Register_ConfirmPasswordField.text;

        //Make sure username and password are long enough
        if (Register_UsernameField.text.Length > 3)
        {
            if (Register_PasswordField.text.Length > 5)
            {
                //Check the two passwords entered match
                if (Register_PasswordField.text == confirmedPassword)
                {
                    //Username and passwords seem reasonable. Switch to 'Loading...' and start the coroutine to try and register an account on the server
                    registerParent.gameObject.SetActive(false);
                    loadingParent.gameObject.SetActive(true);
                    StartCoroutine(RegisterUser());
                }
                else
                {
                    //Passwords don't match, show error
                    Register_ErrorText.text = "Error: Les passwords ne correspondent pas...";
                }
            }
            else
            {
                //Password too short so show error
                Register_ErrorText.text = "Error: Password trop court";
            }
        }
        else
        {
            //Username too short so show error
            Register_ErrorText.text = "Error: Username trop court";
        }
    }
    public void Register_BackButtonPressed()
    {
        //Called when the player presses the 'Back' button on the register UI. Switches back to the Login UI
        ResetAllUIElements();
        loginParent.gameObject.SetActive(true);
        registerParent.gameObject.SetActive(false);
    }
    public void LoggedIn_SaveDataButtonPressed()
    {
        //Called when the player hits 'Set Data' to change the data string on their account. Switches UI to 'Loading...' and starts coroutine to set the players data string on the server
        loadingParent.gameObject.SetActive(true);
        //loggedInParent.gameObject.SetActive(false);
        //StartCoroutine(SetData(LoggedIn_DataInputField.text));
    }
    public void LoggedIn_LoadDataButtonPressed()
    {
        //Called when the player hits 'Get Data' to retrieve the data string on their account. Switches UI to 'Loading...' and starts coroutine to get the players data string from the server
        loadingParent.gameObject.SetActive(true);
        //loggedInParent.gameObject.SetActive(false);
        //StartCoroutine(GetData());
    }
    public void LoggedIn_LogoutButtonPressed()
    {
        //Called when the player hits the 'Logout' button. Switches back to Login UI and forgets the player's username and password.
        //Note: Database Control doesn't use sessions, so no request to the server is needed here to end a session.
        //ResetAllUIElements();
        UserAccountManager.instance.LogOut();
        loginParent.gameObject.SetActive(true);
        ResetAllUIElements();
        //loggedInParent.gameObject.SetActive(false);
    }
}
