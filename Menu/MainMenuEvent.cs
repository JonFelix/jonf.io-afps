using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MainMenuEvent : MonoBehaviour 
{
    void Start()
    {
        PlayerPrefs.SetString("PlayerName2", "Test1");
    }

    void StartServer()
    {
        Application.LoadLevel("MenuHost");
    }

    void JoinServer()
    {
        Application.LoadLevel("MenuJoin");
    }

    void Options()
    {
        Application.LoadLevel("MenuOptions");
    }

    void Quit()
    {
        Application.Quit();
    }
}
