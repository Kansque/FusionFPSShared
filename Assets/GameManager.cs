 using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions.Must;
using static UnityEngine.Rendering.CoreUtils;

public class GameManager : NetworkBehaviour
{
    public static GameManager instance;

    List<PlayerData> localList;
    public List<PlayerData> playerDataList;

    [SerializeField]
    [Networked][Capacity(10)] public NetworkArray<PlayerDataStruct> playerDataStructs { get; } = default;
    //[Networked] public int countInts { get; set;}

    public List<PlayerDataLiteral> playerDataLiterals;


    public bool allNotReady, gameStarted;

    public Transform playerReadyListContent;
    public GameObject playerReadyMenuPrefab;
    public GameObject preGameCanvas;

    public TMP_Text countdown;
    float timer = 10f;

    private void Start()
    {
        playerDataList = new List<PlayerData>();
        localList = new List<PlayerData>();
        instance = this;
    }

    private void Update()
    {
        //Debug.Log(countInts.ToString());
    }

    public override void Spawned()
    {
        //playerDataStructs = new NetworkArray<PlayerDataStruct>();
        RPC_AddPlayerData(new PlayerDataStruct("eee", false, false));
    }

    public override void FixedUpdateNetwork()
    {

        if(playerDataList.Count >= 1 && !gameStarted && preGameCanvas.activeInHierarchy)
        {
            foreach(var player in playerDataList)
            {
                if (!player.Ready)
                    allNotReady = true;
            }
            if (!allNotReady)
            {
                countdown.text = timer.ToString();
                timer -= Time.deltaTime;
                
                if (timer < 0)
                {
                    gameStarted = true;
                    NetworkInitializer.instance.SpawnPlayerObject();
                }
            }
            else
            {
                countdown.text = timer.ToString();
                timer = 10;
                //Time.timeScale = 0f;
            }
        }
        if (playerDataStructs.Length >= 1 && !gameStarted && preGameCanvas.activeInHierarchy)
        {
            foreach (var player in playerDataStructs)
            {
               // if (!player.Ready)
                 //   allNotReady = true;
            }
            if (!allNotReady)
            {
                countdown.text = timer.ToString();
                timer -= Time.deltaTime;

                if (timer < 0)
                {
                    gameStarted = true;
                    NetworkInitializer.instance.SpawnPlayerObject();
                }
            }
            else
            {
                countdown.text = timer.ToString();
                timer = 10;
                //Time.timeScale = 0f;
            }
        }
    }

    public void RefreshPlayerReadyList()
    {
        if (!HasStateAuthority)
            return;

        foreach (Transform child in playerReadyListContent)
        {
            Destroy(child.gameObject);
        }
        foreach (PlayerDataStruct player in playerDataStructs)
        {
            GameObject entry = GameObject.Instantiate(playerReadyMenuPrefab, playerReadyListContent);
            PlayerReadyMenuPrefab script = entry.GetComponent<PlayerReadyMenuPrefab>();
            script.playerName.text = player.PlayerName.ToString();
            script.playerReady.text = player.Ready.ToString();
        }
        
    }
    [Rpc]
    public void RPC_AddPlayerData(PlayerDataStruct playerData)
    {
        Debug.Log("HereADD");
        if (!playerDataStructs.Contains(playerData))
        {
            Debug.Log("HereADD");
            //localList.Add(playerData);
            playerDataStructs.Append(playerData);
            playerDataList = localList;
        }
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPC_RemovePlayerData(PlayerDataStruct playerData)
    {
        if (playerDataStructs.Contains(playerData))
        {
            //localList.Remove(playerData);
            //playerDataStructs = playerDataStructs.Where(x => x.PlayerName != playerData.PlayerName).ToArray();
            playerDataList = localList;
        }
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPC_ChangePlayerData(PlayerDataStruct playerData, PlayerDataStruct desiredData)
    {
        if (playerDataStructs.Contains(playerData))
        {
            //playerDataStructs = playerDataStructs.Where(x => x.PlayerName != playerData.PlayerName).ToArray();
            playerDataStructs.Append(desiredData);
        }
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPC_AddToReadyCount()
    {
        //countInts++;
    }

    protected static void PDT(Changed<GameManager> changed)
    {
       changed.Behaviour.playerDataList = changed.Behaviour.localList;
    }




    //Class array

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPC_AddPlayerDataClass(PlayerDataLiteral playerData)
    {
        if (!playerDataLiterals.Contains(playerData))
        {
            //localList.Add(playerData);
            playerDataLiterals.Add(playerData);
            playerDataList = localList;
        }
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPC_RemovePlayerDataClass(PlayerDataLiteral playerData)
    {
        if (playerDataLiterals.Contains(playerData))
        {
            //localList.Remove(playerData);
            playerDataLiterals.Remove(playerData);
            playerDataList = localList;
        }
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPC_ChangePlayerDataClass(PlayerDataLiteral playerData, PlayerDataLiteral desiredData)
    {
        if (playerDataLiterals.Contains(playerData))
        {
            playerDataLiterals.Remove(playerData);
            playerDataLiterals.Add(desiredData);
        }
    }


    public void RefreshPlayerReadyListClass()
    {
        if (!HasStateAuthority)
            return;

        foreach (Transform child in playerReadyListContent)
        {
            Destroy(child.gameObject);
        }
        foreach (PlayerDataLiteral player in playerDataLiterals)
        {
            GameObject entry = GameObject.Instantiate(playerReadyMenuPrefab, playerReadyListContent);
            PlayerReadyMenuPrefab script = entry.GetComponent<PlayerReadyMenuPrefab>();
            script.playerName.text = player.PlayerName.ToString();
            script.playerReady.text = player.Ready.ToString();
        }
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    public void RPC_RefreshList()
    {

       /* for(int i = 0; i < countInts; i++)
        {
            GameObject entry = GameObject.Instantiate(playerReadyMenuPrefab, playerReadyListContent);
            PlayerReadyMenuPrefab script = entry.GetComponent<PlayerReadyMenuPrefab>();
            script.playerName.text = "gibberish";
            script.playerReady.text = "ready";
        }
        Debug.Log(countInts.ToString());*/
    }
}
