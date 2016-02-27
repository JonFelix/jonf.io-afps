using UnityEngine;
using System.Collections;

public class NetworkManager : MonoBehaviour
{
    public GameObject Player1Prefab;
    public GameObject Player2Prefab;

    MapSpawnManager mSpawns;
    NetworkView mNetView;
    NetworkServer mServer;
    int mRetrievedIndex;
    GameObject tmpPlayer;

    void Start()
    {
        Application.runInBackground = true;
        mNetView = GetComponent<NetworkView>();
        if (PlayerPrefs.GetInt("Server") == 1)
        {
            mServer = gameObject.AddComponent<NetworkServer>();
            GetComponent<NetworkServer>().PlayerPrefab = Player1Prefab;
        }
        else
        {
            gameObject.AddComponent<NetworkClient>();
        }
        mSpawns = GameObject.Find("GameSystem").GetComponent<MapSpawnManager>();
    }

    void OnConnectedToServer()
    {
        SpawnPlayer();
    }

    public void SpawnPlayer()
    {
        if (Network.isServer)
        {
            tmpPlayer = (GameObject)Network.Instantiate(Player1Prefab, mSpawns.GetSpawnPosition(), Quaternion.identity, 0);
        }
        else
        {
            tmpPlayer = (GameObject)Network.Instantiate(Player2Prefab, mSpawns.GetSpawnPosition(), Quaternion.identity, 0);
        }
        RequestIndex();
        
    }

    [RPC]
    void AnnouncePlayer(string name)
    {
        print(name + " Connected!");
    }

    [RPC]
    public void RequestDealDamage(string name, float damage)
    {
        if(Network.isServer)
        {
            mServer.RequestDealDamage(name, damage);
        }
        else
        {
            mNetView.RPC("RequestDealDamage", RPCMode.Server, name, damage);
        }
    }

    [RPC]
    public void RequestAddHealth(string name, float health)
    {
        if(Network.isServer)
        {
            mServer.RequestAddHealth(name, health);
        }
        else
        {
            mNetView.RPC("RequestAddHealth", RPCMode.Server, name, health);
        }
    }

    [RPC]
    void RequestIndex()
    {
        if(Network.isServer)
        {
            mNetView.RPC("GetIndex", RPCMode.All, mServer.PlayerCount);
        }
        else
        {
            mNetView.RPC("RequestIndex", RPCMode.Server);
        }
    }

    [RPC]
    void GetIndex(int index)
    {
        mRetrievedIndex = index;
        tmpPlayer.name = "Player" + mRetrievedIndex.ToString();
        mNetView.RPC("AnnouncePlayer", RPCMode.All, tmpPlayer.name);
    }

}
