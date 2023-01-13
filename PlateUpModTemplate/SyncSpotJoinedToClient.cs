using EmotesAPI;
using Kitchen;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;


class SyncSpotJoinedToClient : INetMessage
{
    NetworkInstanceId netId;
    NetworkInstanceId spot;
    bool worldProp;
    int posInArray;

    public SyncSpotJoinedToClient()
    {

    }

    public SyncSpotJoinedToClient(NetworkInstanceId netId, NetworkInstanceId spot, bool worldProp, int posInArray)
    {
        this.netId = netId;
        this.spot = spot;
        this.worldProp = worldProp;
        this.posInArray = posInArray;
    }

    public void Deserialize(NetworkReader reader)
    {
        netId = reader.ReadNetworkId();
        spot = reader.ReadNetworkId();
        worldProp = reader.ReadBoolean();
        posInArray = reader.ReadInt32();
    }

    public void OnReceived()
    {
        GameObject bodyObject = Util.FindNetworkObject(netId);
        GameObject spotObject = Util.FindNetworkObject(spot);
        if (!bodyObject)
        {
            Debug.Log($"EmoteAPI: Body is null!!!");
        }
        if (!spotObject)
        {
            Debug.Log($"EmoteAPI: spotObject is null!!!");
        }
        BoneMapper joinerMapper = bodyObject.GetComponent<PlayerView>().transform.GetComponentInChildren<BoneMapper>();
        joinerMapper.PlayAnim("none", 0);
        if (worldProp)
        {
            joinerMapper.currentEmoteSpot = spotObject.GetComponentsInChildren<EmoteLocation>()[posInArray].gameObject;
            CustomEmotesAPI.JoinedProp(joinerMapper.currentEmoteSpot, joinerMapper, joinerMapper.currentEmoteSpot.GetComponent<EmoteLocation>().owner);
        }
        else
        {
            joinerMapper.currentEmoteSpot = spotObject.GetComponent<PlayerView>().transform.GetComponentsInChildren<EmoteLocation>()[posInArray].gameObject;
            CustomEmotesAPI.JoinedBody(joinerMapper.currentEmoteSpot, joinerMapper, joinerMapper.currentEmoteSpot.GetComponent<EmoteLocation>().owner);
        }
    }

    public void Serialize(NetworkWriter writer)
    {
        writer.Write(netId);
        writer.Write(spot);
        writer.Write(worldProp);
        writer.Write(posInArray);
    }
}
