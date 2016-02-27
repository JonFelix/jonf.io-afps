using UnityEngine;
using System.Collections;

public class InterfaceDamageText : MonoBehaviour {

    Vector3 PlayerPosition;

    void Update()
    {
        foreach(GameObject i in GameObject.FindGameObjectsWithTag("Player"))
        {
            if(i.GetComponent<NetworkView>().isMine)
            {
                PlayerPosition = i.transform.position;
            }
        }
        transform.LookAt(-PlayerPosition);
    }
}
