using Fusion;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Diagnostics;
using UnityEngine.SocialPlatforms;

public class NetworkPlayer : NetworkBehaviour, IPlayerLeft
{

    public static NetworkPlayer Local { get; set; }

    [Networked(OnChanged = nameof(OnNameChanged))]
    public NetworkString<_16> nameP { get; set; }

    bool isPublicJoinMessageSent = false;

    public Transform playerModel;
    public PlayerData playerData;
    public PlayerDataLiteral playerDataLiteral;
    bool timeToRefresh = true;
    public NetworkBool spawned  = false;

    NetworkInGameMessages networkInGameMessages;

    //public PlayerDataStruct playerDataStruct;
    public override void Spawned()
    {
        playerData = GetComponent<PlayerData>();
        playerDataLiteral = GetComponent<PlayerDataLiteral>();
        if (HasStateAuthority)
        {
        
            Local = this;

            Utils.SetRenderLayerInChildren(playerModel, LayerMask.NameToLayer("LocalPlayerModel"));
            //Utils.ChangeLayersRecursively(playerModel, "LocalPlayerModel");
            //Camera.main.gameObject.SetActive(false);
        }
        /*else
        {
            LocalCamera localCamera = GetComponentInChildren<LocalCamera>();
            localCamera.enabled = false;

            AudioListener listener = GetComponentInChildren<AudioListener>();
            listener.enabled = false;

            //Destroy(GetComponent<PlayerMovement>());

            //localUI.SetActive(false);
        }

        //Runner.SetPlayerObject(Object.InputAuthority, Object);

        //transform.name = nameP.ToString();
    }*/
    }

    private void Update()
    {
        if(timeToRefresh && !GameManager.instance.gameStarted)
            StartCoroutine(RefreshList());
    }


    IEnumerator RefreshList()
    {
        timeToRefresh = false;
        yield return new WaitForSeconds(2f);
        timeToRefresh = true;
    }

    public void PlayerLeft(PlayerRef player)
    {
        if (player == Object.InputAuthority)
        {
            Runner.Despawn(Object);

        }
    }

    public void OnReadyPressed()
    {
        //Debug.Log(GameManager.instance.playerDataList[0].ToString());
        //GameManager.instance.RefreshPlayerReadyList();
        //for (int i = 0; i < GameManager.instance.playerDataStructs.Length; i++)
        //{
          //  if (GameManager.instance.playerDataStructs[i].Equals(playerData.dt)) {
                //GameManager.instance.playerDataStructs[i].Ready = true;
              //  var tempStruct = GameManager.instance.playerDataStructs[i];
              //  tempStruct.Ready = true;
                //GameManager.instance.playerDataStructs[i] = tempStruct;
              //  GameManager.instance.RPC_ChangePlayerData(GameManager.instance.playerDataStructs[i], tempStruct);
            //}
       // }

        
        /*for (int i = 0; i < GameManager.instance.playerDataLiterals.Count(); i++)
        {
            if (GameManager.instance.playerDataLiterals[i] == playerDataLiteral)
            {
                //GameManager.instance.playerDataStructs[i].Ready = true;
                var tempStruct = GameManager.instance.playerDataLiterals[i];
                tempStruct.Ready = true;
                //GameManager.instance.playerDataStructs[i] = tempStruct;
                GameManager.instance.RPC_ChangePlayerDataClass(GameManager.instance.playerDataLiterals[i], tempStruct);
            }
        }*/

        GameManager.instance.RPC_AddToReadyCount();
        GameManager.instance.RPC_RefreshList();
    }

    static void OnNameChanged(Changed<NetworkPlayer> player)
    {
        player.Behaviour.OnNameChanged();
    }
    private void OnNameChanged()
    {
        playerName.text = nameP.ToString();
    }

    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    public void RPC_SetPlayerName(string playerName, RpcInfo info = default)
    {
        nameP = playerName;

        if (!isPublicJoinMessageSent)
        {
            networkInGameMessages.SendInGameRPCMessage(playerName, " joined");

            isPublicJoinMessageSent = true;
        }
    }


    public TextMeshProUGUI playerName;

}    
