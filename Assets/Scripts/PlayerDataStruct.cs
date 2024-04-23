using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct PlayerDataStruct : INetworkStruct
{
    public NetworkString<_32> PlayerName;
    public NetworkBool Ready {  get; set; }
    public NetworkBool Team  { get; set; }

    public PlayerDataStruct(NetworkString<_32> PlayerName, NetworkBool Ready, NetworkBool Team)
    {
        this.PlayerName = PlayerName;
        this.Ready = Ready;
        this.Team = Team;
    }

}
