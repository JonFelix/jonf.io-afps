using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class InterfaceDisplayDamage : MonoBehaviour 
{
    public void Display(float damage)
    {
        if(GetComponentInParent<NetworkView>().isMine)
        {
            GameObject mDisplayText = (GameObject)Instantiate(Resources.Load("DamageText"), transform.position, Quaternion.identity);
            mDisplayText.GetComponent<TextMesh>().text = damage.ToString();
        }
    }
}
