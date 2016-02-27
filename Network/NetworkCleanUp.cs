using UnityEngine;
using System.Collections;

public class NetworkCleanUp : MonoBehaviour 
{
    void OnDisconnectedFromServer()
    {
        Cleanup();
    }

    void OnApplicationQuit()
    {
        Cleanup();
    }


    
    public void Cleanup()
    {
        //GetComponent<NetworkView>().RPC("CallCleanup", RPCMode.Others);
    }

    [RPC]
    void CallCleanup()
    {
        Destroy(gameObject);
    }
}
