using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour 
{
    public float MaxHealth = 200f;
    public float StartHealth = 100f;
    public float HealthThreshold = 100f;
    public float HealthLoss = 1f;
    public float MaxArmor = 200f;
    public float StartArmor = 100f;

    float mHealth;
    float mArmor;
    float mDeteriateHealth = 0f;
    Text mHealthText;
    Text mArmorText;
    NetworkView mNetView;
    bool mIsDead = false;
    NetworkManager mNetManager;

    public bool IsDead
    {
        get
        {
            return mIsDead;
        }
    }

	void Start () 
    {
        mHealth = StartHealth;
        mArmor = StartArmor;
        mHealthText = GameObject.Find("HealthDisplay").GetComponent<Text>();
        mArmorText = GameObject.Find("ArmorDisplay").GetComponent<Text>();
        mNetView = GetComponent<NetworkView>();
        mNetManager = GetComponent<NetworkManager>();
	}
	
	void Update () 
    {
        mIsDead = false;
	    if(mHealth > HealthThreshold)
        {
            if(mDeteriateHealth >= 1f)
            {
                mHealth -= HealthLoss;
                mDeteriateHealth = 0f;
            }
            mDeteriateHealth += Time.deltaTime;
        }

        if (mHealth <= 0)
        {

            IDied();
        }
        

        if (mNetView.isMine)
        {
            mHealthText.text = Mathf.RoundToInt(mHealth) + "HP";
            mArmorText.text = Mathf.RoundToInt(mArmor) + "AP";
        }
	}

    void IDied()
    {
        mIsDead = true;
    }

    public void SetHealth(float health)
    {
        mHealth = health;
        mDeteriateHealth = 0f;
    }

    [RPC]
    void WithdrawHealth(float health)
    {
        mHealth = Mathf.Max(0f, mHealth - Mathf.Abs(health));
    }

    
    public void AddHealth(float health)
    {
        mHealth = Mathf.Min(MaxHealth, mHealth + Mathf.Abs(health));
    }

    public void SetArmor(float armor)
    {
        mArmor = armor;
    }

    [RPC]
    void WithdrawArmor(float armor)
    {
        mArmor = Mathf.Max(0f, mArmor - armor);
    }

    public void AddArmor(float armor)
    {
        mArmor = Mathf.Min(MaxArmor, mArmor + Mathf.Abs(armor));
    }

    public void DealDamage(float damage)
    {
        float armor;
        float health = damage;
        if (mArmor > 0f)
        {
            armor = (damage / 3f) * 2f;
            health = (damage / 3f);
            if (mArmor - armor < 0f)
            {
                health += armor - armor;
            }
            mNetView.RPC("WithdrawArmor", RPCMode.All, armor);
        }
        if(!mNetView.isMine)
        {
            GetComponentInChildren<InterfaceDisplayDamage>().Display(health);
        }
        mNetView.RPC("WithdrawHealth", RPCMode.All, health);
    }
}
