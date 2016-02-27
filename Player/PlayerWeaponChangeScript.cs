using UnityEngine;
using System.Collections;

public class PlayerWeaponChangeScript : MonoBehaviour 
{
    RocketShot mRocket;
    ShootRail mRail;
    Taser mTaser;
    NetworkView mNetView;
    InterfaceIngameOptions mOptions;

	void Start () 
    {
        mRocket = GameObject.Find("RocketFirePoint").GetComponent<RocketShot>();
        mRail = GameObject.Find("RailFirePoint").GetComponent<ShootRail>();
        mTaser = GameObject.Find("Taser").GetComponent<Taser>(); 
        mNetView = GetComponent<NetworkView>();
        mOptions = GetComponent<InterfaceIngameOptions>();
	}
	
	void Update () 
    {
        if (mNetView.isMine && !mOptions.InOptions)
        {
            if (Input.GetButtonDown("Fire2"))
            {
                if (!mTaser.SetActive)
                {
                    if (mRocket.SetActive)
                    {
                        mRocket.SetActive = false;
                        mRail.SetActive = true;
                        mTaser.SetActive = false;
                    }
                    else
                    {
                        mRocket.SetActive = true;
                        mRail.SetActive = false;
                        mTaser.SetActive = false;
                    }
                }
                else
                {
                    mRocket.SetActive = !mRocket.SetActive;
                    mRail.SetActive = false;
                    mTaser.SetActive = false; 
                }
            }

            if(Input.GetButtonDown("SelectRocketLauncher"))
            {
                mRocket.SetActive = true;
                mRail.SetActive = false;
                mTaser.SetActive = false;
            }

            if (Input.GetButtonDown("SelectRailGun"))
            {
                mRocket.SetActive = false;
                mRail.SetActive = true;
                mTaser.SetActive = false;
            }

            if(Input.GetButtonDown("SelectTaser"))
            {
                mRocket.SetActive = false;
                mRail.SetActive = false;
                mTaser.SetActive = true;
            }
        }
	}
}
