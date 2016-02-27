using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MenuOptionsEvents : MonoBehaviour 
{
    GameObject PlayerOptions;

    void Start()
    {
        PlayerOptions = GameObject.Find("PlayerOptions");
        GameObject.Find("NameInput").GetComponent<InputField>().text = PlayerPrefs.GetString("PlayerName");
        GameObject.Find("HorizontalSensitivityInput").GetComponent<InputField>().text = (PlayerPrefs.GetFloat("HorizontalSensitivity") / 10).ToString();
        GameObject.Find("VerticalSensitivityInput").GetComponent<InputField>().text = (PlayerPrefs.GetFloat("VerticalSensitivity") / 10).ToString();
        GameObject.Find("FovSlider").GetComponent<Slider>().maxValue = 40;
        GameObject.Find("FovSlider").GetComponent<Slider>().value = PlayerPrefs.GetFloat("PlayerFieldOfView") - 70;
        GameObject.Find("FovValue").GetComponent<Text>().text = PlayerPrefs.GetFloat("PlayerFieldOfView").ToString();
        PlayerOptions.SetActive(false);
        if (Application.loadedLevelName == "MenuOptions")
        {
            Destroy(GameObject.Find("DisconnectButton"));
        }
    }


    void Video()
    {
        
    }

    void Audio()
    {

    }

    void Player()
    {
        if (!PlayerOptions.activeSelf)
        {
            PlayerOptions.SetActive(true);
        }
    }

    void Back()
    {
        if (Application.loadedLevelName == "MenuOptions")
        {
            Application.LoadLevel("MenuStart");
        }
        else
        {
            foreach(GameObject i in GameObject.FindGameObjectsWithTag("Player"))
            {
                i.GetComponent<InterfaceIngameOptions>().CloseOptions();
            }
        }
    }

    void ChangeName()
    {
        PlayerPrefs.SetString("PlayerName", GameObject.Find("NameInput").GetComponent<InputField>().text);
    }

    void Disconnect()
    {
        foreach (GameObject i in GameObject.FindGameObjectsWithTag("Player"))
        {
            if (i.GetComponent<NetworkView>().isMine)
            {
                i.GetComponent<NetworkCleanUp>().Cleanup();
            }
        }
        
        Network.Disconnect();
        Application.LoadLevel("MenuStart");
    }

    void ChangeSensitivity()
    {
        float vert = 0f;
        float hor = 0;
        if (float.TryParse(GameObject.Find("HorizontalSensitivityInput").GetComponent<InputField>().text, out hor) && float.TryParse(GameObject.Find("VerticalSensitivityInput").GetComponent<InputField>().text, out vert))
        {
            PlayerPrefs.SetFloat("HorizontalSensitivity", hor * 10);
            PlayerPrefs.SetFloat("VerticalSensitivity", vert * 10);
        }
    }

    void ChangeFOV()
    {
        float fov = GameObject.Find("FovSlider").GetComponent<Slider>().value;
        PlayerPrefs.SetFloat("PlayerFieldOfView", 70 + fov);
        GameObject.Find("FovValue").GetComponent<Text>().text = PlayerPrefs.GetFloat("PlayerFieldOfView").ToString();
    }
}
