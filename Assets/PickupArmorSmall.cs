using UnityEngine;
using System.Collections;

public class PickupArmorSmall : MonoBehaviour
{
    float Armor = 5;
    float TurnSpeed = 50f;
    bool mCollided = false;

    void Start()
    {

    }

    void Update()
    {
        transform.rotation = Quaternion.Lerp(transform.rotation, transform.rotation * Quaternion.Euler(Vector3.up), TurnSpeed * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && !mCollided)
        {
            print("Armor picked up!");
            other.gameObject.GetComponent<PlayerHealth>().AddArmor(Armor);
            GetComponent<AudioSource>().Play();
            GetComponent<MeshRenderer>().enabled = false;
            mCollided = true;
            Destroy(gameObject, 1);
        }
    }
}
