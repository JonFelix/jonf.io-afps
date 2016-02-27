using UnityEngine;
using System.Collections;

public class NetworkSerializedStream : MonoBehaviour
{
    private float lastSynchronziationTime = 0f;
    private float syncDelay = 0f;
    private float syncTime = 0f;
    private Vector3 syncStartPosition = Vector3.zero;
    private Vector3 syncEndPosition = Vector3.zero;

    public float SyncTime
    {
        get
        {
            return syncTime;
        }
        set
        {
            syncTime = value;
        }
    }

    public float SyncDelay
    {
        get
        {
            return syncDelay;
        }
        set
        {
            syncDelay = value;
        }
    }

    public Vector3 StartPosition
    {
        get
        {
            return syncStartPosition;
        }
        set
        {
            syncStartPosition = value;
        }
    }

    public Vector3 EndPosition
    {
        get
        {
            return syncEndPosition;
        }
        set
        {
            syncEndPosition = value;
        }
    }

    void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info)
    {
        Vector3 mSyncPosition = Vector3.zero;
        if (stream.isWriting)
        {
            mSyncPosition = GetComponent<Transform>().position;
            stream.Serialize(ref mSyncPosition);
        }
        else
        {
            stream.Serialize(ref mSyncPosition);
            syncTime = 0f;
            syncDelay = Time.time - lastSynchronziationTime;
            lastSynchronziationTime = Time.time;
            GetComponent<Transform>().position = mSyncPosition;
            syncEndPosition = mSyncPosition;
        }
    }

}
