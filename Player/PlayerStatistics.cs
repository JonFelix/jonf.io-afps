using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerStatistics : MonoBehaviour 
{
    string mOpponentName;
    NetworkView mNetView;
    Text mPlayerText;
    Text mOpponentText;
    Text mPlayerScoreText;
    Text mOpponentScoreText;
    PlayerStatistics mOtherPlayer;
    int mScore = 0;

    public int Score
    {
        get
        {
            return mScore;
        }
        set
        {
            mScore = value;
        }
    }


	void Start () 
    {
        mNetView = GetComponent<NetworkView>();
        mPlayerText = GameObject.Find("PlayerNameDisplay").GetComponent<Text>();
        mOpponentText = GameObject.Find("OpponentNameDisplay").GetComponent<Text>();
        mPlayerScoreText = GameObject.Find("PlayerScoreDisplay").GetComponent<Text>();
        mOpponentScoreText = GameObject.Find("OpponentScoreDisplay").GetComponent<Text>();
        if (PlayerPrefs.GetInt("Server") == 0)
        {
            mNetView.RPC("SendName", RPCMode.OthersBuffered, PlayerPrefs.GetString("PlayerName"));
        }
	}
	
	void Update () 
    {
        mPlayerText.text = PlayerPrefs.GetString("PlayerName");
        mOpponentText.text = mOpponentName;

        mPlayerScoreText.text = mScore.ToString();
        if(mOtherPlayer != null)
        {
            mOpponentScoreText.text = mOtherPlayer.Score.ToString();
        }
        else
        {
            mOpponentScoreText.text = "";
        }
	}

    void OnPlayerConnected()
    {
        mNetView.RPC("SendName", RPCMode.OthersBuffered, PlayerPrefs.GetString("PlayerName"));
    }

    [RPC]
    void SendName(string name)
    {
        mOpponentName = name;
        foreach(GameObject i in GameObject.FindGameObjectsWithTag("Player"))
        {
            if(i.name != gameObject.name)
            {
                mOtherPlayer = i.GetComponent<PlayerStatistics>();
            }
        }
    }
}
