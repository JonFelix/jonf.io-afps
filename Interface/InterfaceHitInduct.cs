using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class InterfaceHitInduct : MonoBehaviour {
    
    public float MaxSize = 1f;

    RectTransform mRect;
    Vector3 mOriginalSize;
    Image mImage;
    bool mHitRun = false;
    Color mOriginalColor;
    Color mTargetColor;

	void Start () 
    {
        mRect = GetComponent<RectTransform>();
        mImage = GetComponent<Image>();
        mImage.enabled = false;
        mOriginalSize = mRect.localScale;
        mOriginalColor = mImage.color;
        mTargetColor = mImage.color;
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (mHitRun)
        {
            if (Vector3.Distance(Vector3.zero, mRect.localScale) < MaxSize)
            {
                mRect.localScale = new Vector3(Mathf.Lerp(mRect.localScale.x, MaxSize, 10 * Time.deltaTime), Mathf.Lerp(mRect.localScale.y, MaxSize, 10 * Time.deltaTime), 0f);
                mTargetColor.a = Mathf.Lerp(mTargetColor.a, 0, Time.deltaTime);
                mImage.color = mTargetColor;
            }
            else
            {
                mHitRun = false;
                mImage.enabled = false;
                mRect.localScale = new Vector3(mOriginalSize.x, mOriginalSize.y, 0f);
                mImage.color = mOriginalColor;
            }
        }
	}

    public void StartHitIndict()
    {
        mHitRun = true;
        mImage.enabled = true;
        mRect.localScale = new Vector3(mOriginalSize.x, mOriginalSize.y, 0f);
        mImage.color = mOriginalColor;
    }
}
