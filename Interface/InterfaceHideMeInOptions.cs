using UnityEngine;
using System.Collections;

public class InterfaceHideMeInOptions : MonoBehaviour 
{

    InterfaceIngameOptions mOptions;
    CanvasRenderer mRenderer;

    void Start()
    {
        mOptions = GetComponentInParent<InterfaceIngameOptions>();
        mRenderer = GetComponent<CanvasRenderer>();
    }
	
	void Update () 
    {
	    if(mOptions.InOptions)
        {
            mRenderer.SetAlpha(0f);
        }
        else
        {
            mRenderer.SetAlpha(1f);
        }
	}
}
