using Kitchen;
using Kitchen.Serializers;
using Kitchen.Transports;
using Kitchen.NetworkSupport;
using Steamworks.Data;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using Unity.Entities;
using UnityEngine;
using UnityEngine.Networking;
using Steamworks;

class SyncAnimationToClientsPlate : SteamLobbyTransport
{
    InputIdentifier id;
    string animation;
    int position;
    int eventNum;
    public SyncAnimationToClientsPlate(InputIdentifier id, string animation, int pos, int eventNum)
    {
        this.id = id;
        this.animation = animation;
        this.position = pos;
        this.eventNum = eventNum;
        BinaryFormatter bf = new BinaryFormatter();
        using (var ms = new MemoryStream())
        {
            bf.Serialize(ms, this);
            SteamNetworking.SendP2PPacket(this.Platform.GetOtherLobbyMembers(this.CurrentLobby).ToList<SteamNetworkTarget>()[0].ID, ms.ToArray(), -1, 0, P2PSend.Reliable);
        }
    }
}
class SyncAnimationToClients : INetMessage
{
    NetworkInstanceId netId;
    string animation;
    int position;
    int eventNum;

    public SyncAnimationToClients()
    {

    }

    public SyncAnimationToClients(NetworkInstanceId netId, string animation, int pos, int eventNum)
    {
        this.netId = netId;
        this.animation = animation;
        this.position = pos;
        this.eventNum = eventNum;
    }

    public void Deserialize(NetworkReader reader)
    {
        //Debug.Log($"EmoteAPI: POSITION: {reader.Position}, SIZE: {reader.Length}");

        netId = reader.ReadNetworkId();
        animation = reader.ReadString();
        position = reader.ReadInt32();
        eventNum = reader.ReadInt32();
    }

    public void OnReceived()
    {
        //if (NetworkServer.active)
        //    return;


        GameObject bodyObject = Util.FindNetworkObject(netId);
        if (!bodyObject)
        {
            Debug.Log($"EmoteAPI: Body is null!!!");
        }

        Debug.Log($"EmoteAPI: Recieved message to play {animation} on client. Playing on {bodyObject.GetComponent<PlayerView>().transform}");

        bodyObject.GetComponent<PlayerView>().transform.GetComponentInChildren<BoneMapper>().PlayAnim(animation, position, eventNum);
    }

    public void Serialize(NetworkWriter writer)
    {
        writer.Write(netId);
        writer.Write(animation);
        writer.Write(position);
        writer.Write(eventNum);
    }
}
