using UnityEngine;
using System.Collections;

public class InterfaceIngameOptions : MonoBehaviour 
{
    public GameObject Canvas;

    bool mInOptions = false;
    GameObject mInstantiatedOptions;
    NetworkView mNetView;

    public bool InOptions
    {
        get
        {
            return mInOptions;
        }
    }

	void Start ()
    {
        mNetView = GetComponent<NetworkView>();
	}
	
	// Update is called once per frame
	void Update () 
    {
	    if(Input.GetButtonDown("Cancel"))
        {
            if(mInOptions)
            {
                Destroy(mInstantiatedOptions);
            }
            else
            {
                GameObject mOptions = (GameObject)Resources.Load("OptionsPrefab");
                //mOptions.transform.SetParent(Canvas.transform, false);
                mInstantiatedOptions = (GameObject)Instantiate(mOptions);
            }
            mInOptions = !mInOptions;
        }
	}

    public void CloseOptions()
    {
        Destroy(mInstantiatedOptions);
        mInOptions = false;
    }
}
