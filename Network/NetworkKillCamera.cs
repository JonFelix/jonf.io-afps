using UnityEngine;
using System.Collections;

public class NetworkKillCamera : MonoBehaviour 
{	
	void Update () 
    {
        if (!GetComponent<NetworkView>().isMine)
        {
            Camera[] mCams = GetComponentsInChildren<Camera>();
            mCams[0].enabled = false;
            mCams[1].enabled = false;
            GetComponentInChildren<AudioListener>().enabled = false;
            //gameObject.tag = "Enemy";
        }
        else
        {
            GameObject.Find("Weapons/RailGun/RailBody").layer = LayerMask.NameToLayer("Weapon");
            GameObject.Find("Weapons/RocketLauncher/RocketBody").layer = LayerMask.NameToLayer("Weapon");
            GameObject.Find("Weapons/Taser/TaserBody").layer = LayerMask.NameToLayer("Weapon");
        }
        enabled = false;
	}
}
