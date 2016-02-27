using UnityEngine;
using System.Collections;

public class JumpPadBehaviour : MonoBehaviour 
{
    public float UpwardsForce = 20f;

    public Vector3 Direction;

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            other.GetComponent<PlayerQMovement>().ExplosionVelocity = Direction * UpwardsForce;
        }
    }
}
