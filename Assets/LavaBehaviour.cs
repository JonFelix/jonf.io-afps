using UnityEngine;
using System.Collections;

public class LavaBehaviour : MonoBehaviour 
{
    public float Damage = 5f;

    void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<PlayerHealth>().DealDamage(Damage * Time.deltaTime);   
        }
    }
}
