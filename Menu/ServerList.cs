using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ServerList : MonoBehaviour 
{
    public GameObject ButtonPrefab;

    HostData[] mHostList;
    Transform mPane;
    GameObject[] mServerButtons;
    GameObject mPasswordPrompt;
	
	void Start () 
    {
        RefreshHostList();
        mPane = GameObject.Find("ServerListPane").transform;
        ButtonPrefab = (GameObject)Resources.Load("ServerListItem");
        mPasswordPrompt = GameObject.Find("PasswordPrompt");
	}
	
	void Update () {
	
	}

    void OnMasterServerEvent(MasterServerEvent serverevent)
    {
        mHostList = MasterServer.PollHostList();
        mServerButtons = new GameObject[mHostList.Length];
        for (int i = 0; i < mHostList.Length; i++ )
        {
            mServerButtons[i] = (GameObject)Instantiate(ButtonPrefab, Vector3.zero, Quaternion.identity);
            mServerButtons[i].transform.SetParent(mPane, false);
           // mServerButtons[i].GetComponent<RectTransform>().SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, 15 - (45 * i), 30);
            mServerButtons[i].GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -15 + (-45 * i));
            //mServerButtons[i].GetComponent<RectTransform>().position = new Vector2(-(mPane.GetComponent<RectTransform>().rect.width / 2) , -15 - (45 * i));
            int IPLength = 0;
            string tmpIp = "";
            while (IPLength < mHostList[i].ip.Length)
            {
                tmpIp = mHostList[i].ip[IPLength] + " ";
                IPLength++;
            }
            GameObject.Find(mServerButtons[i].name + "/NameText").GetComponentInChildren<Text>().text = mHostList[i].gameName;
            GameObject.Find(mServerButtons[i].name + "/PopulationText").GetComponentInChildren<Text>().text =  mHostList[i].connectedPlayers.ToString() + '/' + mHostList[i].playerLimit;
            GameObject.Find(mServerButtons[i].name + "/IpText").GetComponentInChildren<Text>().text =  tmpIp;
            GameObject.Find(mServerButtons[i].name + "/PasswordImage").GetComponentInChildren<Image>().enabled = mHostList[i].passwordProtected;
            //mServerButtons[i].GetComponentInChildren<Text>().text =(mHostList[i].passwordProtected ? "PW: ":"") + mHostList[i].gameName + "   " + mHostList[i].connectedPlayers.ToString() + '/' + mHostList[i].playerLimit + " IP: " + tmpIp;
            mServerButtons[i].GetComponent<MenuHostDataHolder>().Data = mHostList[i];
            mServerButtons[i].GetComponent<MenuHostDataHolder>().PasswordPrompt = mPasswordPrompt;
        }
    }

    void RefreshHostList()
    {
        if (mServerButtons != null)
        {
            for (int i = 0; i < mServerButtons.Length; i++)
            {
                mServerButtons[i].SetActive(false);
            }
        }
        MasterServer.RequestHostList("JonJon");
    }
   

    void Back()
    {
        Application.LoadLevel("MenuStart");
    }


    void CancelPaswordPrompt()
    {
        mPasswordPrompt.SetActive(false);
    }
}
