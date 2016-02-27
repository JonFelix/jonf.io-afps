using UnityEngine;
using System.Collections;

public class PlayerDeath : MonoBehaviour
{
    PlayerHealth mHealth;
    PlayerResetMe mReset;

    void Start()
    {
        mHealth = GetComponent<PlayerHealth>();
        mReset = GetComponent<PlayerResetMe>();
    }

    void Update()
    {
        if (mHealth.IsDead)
        {
            GetComponent<PlayerAnimateHit>().Death();
            mReset.DeathReset();
        }
    }
}
