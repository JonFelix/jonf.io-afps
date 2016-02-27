using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MenuServerJoinWithPassword : MonoBehaviour 
{
    public HostData Data;
    
    void Start()
    {
        gameObject.SetActive(false);
    }

    void Click()
    {
        string mPassword = GameObject.Find("PasswordPromptInput").GetComponentInChildren<Text>().text;
        PlayerPrefs.SetInt("Server", 0);
        Network.Connect(Data, mPassword);
        Application.LoadLevel("Def");
    }
}
