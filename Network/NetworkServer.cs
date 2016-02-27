using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class NetworkServer : MonoBehaviour
{
    public GameObject PlayerPrefab;


    Text mText;
    int mPlayerCount;
    bool mMatchReady = false;
    NetworkPlayer[] mPlayers;
    NetworkView mNetView;

    public bool Ready
    {
        get
        {
            return mMatchReady;
        }
    }

    public int PlayerCount
    {
        get
        {
            return mPlayerCount;
        }
    }

    void Start()
    {
        
        mText = GameObject.Find("DebugText").GetComponent<Text>();
        mPlayers = new NetworkPlayer[2];
        mNetView = GetComponent<NetworkView>();
        GetComponent<NetworkManager>().SpawnPlayer();
        mPlayerCount++;
    }

    void Update()
    {
        mText.text = "Player count: " + mPlayerCount.ToString() + '\n';
        mText.text += "Match Ready: " + (mMatchReady ? bool.TrueString : bool.FalseString);
    }

    void OnServerInitialized()
    {
        Debug.Log("Server Initializied");
        IncreasePlayerCount();
    }

    void OnPlayerConnected(NetworkPlayer player)
    {
        IncreasePlayerCount();
        mPlayers[mPlayerCount - 1] = player;
        if(mPlayerCount == 2)
        {
            mMatchReady = true;
        }
    }

    void OnPlayerDisconnected()
    {
        DecreasePlayerCount();
        if(mPlayerCount < 2)
        {
            mMatchReady = false;
        }
    }

    void IncreasePlayerCount()
    {
        mPlayerCount++;
        print("Player count: " + mPlayerCount.ToString());
    }

    void DecreasePlayerCount()
    {
        mPlayerCount--;
        print("Player count: " + mPlayerCount.ToString());
    }



    //HEALTH SERVER LOGIC

    
    public void RequestDealDamage(string name, float damage)
    {
        GameObject.Find(name).GetComponent<PlayerHealth>().DealDamage(damage);
        
    }

    public void RequestAddHealth(string name, float health)
    {
        GameObject.Find(name).GetComponent<PlayerHealth>().DealDamage(health);
    }

    
}
