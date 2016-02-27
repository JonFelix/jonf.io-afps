using UnityEngine;
using System.Collections;

public class MenuRotateCamera : MonoBehaviour 
{
    public Vector3 RotateRate;
    public float RotateSpeed;

    static Quaternion mRotation;

	// Use this for initialization
	void Start () 
    {
        transform.rotation = mRotation;
	}
	
	// Update is called once per frame
	void Update ()
    {
	    transform.rotation = Quaternion.Slerp(transform.rotation, transform.rotation * Quaternion.Euler(RotateRate), RotateSpeed * Time.deltaTime);
        mRotation = transform.rotation;
	}
}
