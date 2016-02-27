using UnityEngine;
using System.Collections;

public class NetworkClient : MonoBehaviour
{
    
    void OnConnectedToServer()
    {
        Debug.Log("Server Joined");
    }
}
