using UnityEngine;
using System.Collections;

public class MenuHostDataHolder : MonoBehaviour 
{
    public HostData Data;

    public GameObject PasswordPrompt;
	
    void Click()
    {
        if (Data.passwordProtected)
        {
            PasswordPrompt.SetActive(true);
            print("tead");
            PasswordPrompt.GetComponent<MenuServerJoinWithPassword>().Data = Data;
        }
        else
        {
            PlayerPrefs.SetInt("Server", 0);
            Network.Connect(Data);
            Application.LoadLevel("Def");
        }
        
    }
}
