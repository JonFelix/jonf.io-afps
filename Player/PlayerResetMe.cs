using UnityEngine;
using System.Collections;

public class PlayerResetMe : MonoBehaviour 
{
    public GameObject PlayerController;
    MapSpawnManager mSpawn;
    PlayerQMovement mMovement;
    PlayerHealth mHealth;
    ShootRail mRail;
    RocketShot mRocket;
    PlayerStatistics mStats;

    void Start()
    {
        mSpawn = GameObject.Find("GameSystem").GetComponent<MapSpawnManager>();
        mMovement = GetComponent<PlayerQMovement>();
        mHealth = GetComponent<PlayerHealth>();
        mRail = GetComponentInChildren<ShootRail>();
        mRocket = GetComponentInChildren<RocketShot>();
        mStats = GetComponent<PlayerStatistics>();
    }

    public void Reset()
    {
        transform.position = mSpawn.GetSpawnPosition();
        mMovement.Velocity = Vector3.zero;
        mHealth.SetHealth(mHealth.StartHealth);
        mHealth.SetArmor(mHealth.StartArmor);
        mRail.Ammo = mRail.StartAmmo;
        mRocket.Ammo = mRocket.StartAmmo;
        
    }

    public void DeathReset()
    {
        Reset();
        GameObject[] mPlayers = GameObject.FindGameObjectsWithTag("Player");

        foreach (GameObject i in mPlayers)
        {
            if (i.name != gameObject.name)
            {
                i.GetComponent<PlayerStatistics>().Score += 1;
            }
        }
    }
	
}
