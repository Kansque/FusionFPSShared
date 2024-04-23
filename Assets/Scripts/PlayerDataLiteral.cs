using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class PlayerDataLiteral : NetworkBehaviour
{
    [Networked] public NetworkString<_32> PlayerName {  get; set; }
    [Networked] public NetworkBool Ready { get; set; }
    [Networked] public NetworkBool Team { get; set; }

    GameManager gameManager;


    public override void Spawned()
    {
        gameManager = GameObject.FindObjectOfType<GameManager>();
        if (HasStateAuthority)
        {
            PlayerName = NetworkInitializer.instance.playerName;
            Ready = false;
            Team = false;
            gameManager.RPC_AddPlayerDataClass(this);
        }
    }
}
