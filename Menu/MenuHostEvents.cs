using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MenuHostEvents : MonoBehaviour 
{
    void Host()
    {
        PlayerPrefs.SetInt("Server", 1);
        string mName = GameObject.Find("NameInput").GetComponent<InputField>().text;
        string mPassword = GameObject.Find("PasswordInput").GetComponent<InputField>().text;
        if(mName == "")
        {
            mName = "New Game";
        }

        if(mPassword != "")
        {
            Network.incomingPassword = mPassword;
        }
        Network.InitializeServer(1, 5123, false);
        MasterServer.RegisterHost("JonJon", mName);
        Application.LoadLevel("Def");
    }

    void Back()
    {
        Application.LoadLevel("MenuStart");
    }
}
