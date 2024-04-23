using System.Collections;
using System.Collections.Generic;
using Fusion;
using TMPro;
using UnityEngine;

public class PlayerData : NetworkBehaviour
{
    
    [Networked] public NetworkString<_32> PlayerName {  get; set; }
    [Networked] public NetworkBool Ready { get; set; }
    [Networked] public NetworkBool Team { get; set; }

    //public PlayerDataStruct dt;

    [SerializeField] TMP_Text playerNameLabel;
    [SerializeField] GameManager gameManager;
    NetworkPlayer player;
    PlayerDataStruct dt;
    //public NetworkInitializer networkInitializer;

    public override void Spawned()
    {
        player = GetComponent<NetworkPlayer>();
        //PlayerName = networkInitializer.playerName;
        player.spawned = true;
        gameManager = GameObject.FindObjectOfType<GameManager>();
        if (HasStateAuthority)
        {
            PlayerName = NetworkInitializer.instance.playerName;
            //Debug.Log(PlayerName.ToString());
            Ready = false;
            Team = false;
            dt = new PlayerDataStruct(PlayerName, Ready, Team);
            Debug.Log("Heybro");
            gameManager.RPC_AddPlayerData(dt);
        }
    }
    private void Start()
    {
        
    }
}
