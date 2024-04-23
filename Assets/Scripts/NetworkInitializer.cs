using Fusion;
using Fusion.Sockets;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class NetworkInitializer : NetworkBehaviour, INetworkRunnerCallbacks
{
    public static NetworkInitializer instance;
    //public GameObject gameManagerPrefab;
    public bool spawnPlayer = false;

    public bool connectOnAwake = false;
    [HideInInspector]public NetworkRunner runner;
    NetworkRunner plchldrRunner;

    [SerializeField]
    NetworkObject playerPrefab;

    public string playerName;

    private List<SessionInfo> sessions = new List<SessionInfo>();
    [Header("Session List")]
    public GameObject roomListCanvas;
    public Button refreshButton;
    public Transform sessionListContent;
    public GameObject sessionEntryPrefab;

    public GameObject preGameCanvas;

    public void ConnectToLobby(string playerName)
    {
        roomListCanvas.SetActive(true);
        this.playerName = playerName;

        if (runner == null)
            runner = gameObject.AddComponent<NetworkRunner>();

        runner.JoinSessionLobby(SessionLobby.Shared);
    }
    public async void ConnectToSession(string sessionName)
    {
        roomListCanvas.SetActive(false);

        if(runner == null)
            runner = gameObject.AddComponent<NetworkRunner>();

        await runner.StartGame(new StartGameArgs()
        {
            GameMode = GameMode.Shared,
            SessionName = sessionName,
            SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>()
        });

        //GameManager.instance.AddPlayerData(runner.TryGetComponent<PlayerData>);
    }

    public async void CreateSession()
    {
        roomListCanvas.SetActive(false);
        string randName = "Room-" + UnityEngine.Random.Range(1000, 9999).ToString();

        if (runner == null)
            runner = gameObject.AddComponent<NetworkRunner>();
        
        await runner.StartGame(new StartGameArgs()
        {
            GameMode = GameMode.Shared,
            SessionName = randName,
            PlayerCount = 10,
            SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>()
        });

        //Instantiate(gameManagerPrefab);
    }
    public void RefreshSessionListUI()
    {
        foreach (Transform child in sessionListContent)
        {
            Destroy(child.gameObject);
        }
        foreach(SessionInfo session in sessions)
        {
            if (session.IsVisible)
            {
                GameObject entry = GameObject.Instantiate(sessionEntryPrefab, sessionListContent);
                SessionEntryPrefab script = entry.GetComponent<SessionEntryPrefab>();
                script.roomName.text = session.Name;
                script.playerCount.text = session.PlayerCount + "/" + session.MaxPlayers;

                if(session.IsOpen == false || session.PlayerCount >= session.MaxPlayers)
                {
                    script.joinButton.interactable = false;
                }
                else
                {
                    script.joinButton.interactable= true;
                }
            }
        }
    }
    public void Awake()
    {
        if(instance == null) { instance = this; }

        //if(connectOnAwake)
            //ConnectToRunner();
    }

    public void OnConnectedToServer(NetworkRunner runner)
    {
        preGameCanvas.SetActive(true);

        plchldrRunner = runner;
        NetworkObject playerObject = runner.Spawn(playerPrefab, Utils.GetRandomSpawnPoint(), Quaternion.identity);
        playerObject.transform.name = playerName + "/PlayerObject";
        runner.SetPlayerObject(runner.LocalPlayer, playerObject);
    }

    public void SpawnPlayerObject()
    {
        //NetworkObject playerObject = plchldrRunner.Spawn(playerPrefab, Utils.GetRandomSpawnPoint(), Quaternion.identity);
        //playerObject.transform.name = playerName + "/PlayerObject";
        //plchldrRunner.SetPlayerObject(runner.LocalPlayer, playerObject);
        //plchldrRunner = null;
    }
    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason){}
    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token){}
    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data){}
    public void OnDisconnectedFromServer(NetworkRunner runner){}
    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken){}
    public void OnInput(NetworkRunner runner, NetworkInput input){}
    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input){}
    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player){}
    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        runner.Despawn(playerPrefab);
    }
    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ArraySegment<byte> data){}
    public void OnSceneLoadDone(NetworkRunner runner){}
    public void OnSceneLoadStart(NetworkRunner runner){}
    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
    {
        sessions.Clear();
        sessions = sessionList;
    }
    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason){}
    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message){}
}
