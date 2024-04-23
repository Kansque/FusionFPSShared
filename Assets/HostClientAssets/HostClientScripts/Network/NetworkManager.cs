using Fusion;
using Fusion.Sockets;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;
using UnityEngine;

public class NetworkManager : MonoBehaviour
{
    public NetworkRunner runnerPrefab;
    NetworkRunner runner;
    void Start()
    {
        runner = Instantiate(runnerPrefab);
        runner.name = "Network Runner";

        var clientTask = InitalizeNetowrkRunner(runner, GameMode.AutoHostOrClient, NetAddress.Any(), SceneManager.GetActiveScene().buildIndex, null);
    }

    protected virtual Task InitalizeNetowrkRunner(NetworkRunner runner, GameMode gameMode, NetAddress address, SceneRef scene, Action<NetworkRunner> initalized)
    {
        var sceneManager = runner.GetComponents(typeof(MonoBehaviour)).OfType<INetworkSceneManager>().FirstOrDefault();

        if(sceneManager == null)
            sceneManager = runner.gameObject.AddComponent<NetworkSceneManagerDefault>();

        runner.ProvideInput = true;

        return runner.StartGame(new StartGameArgs
        {
            GameMode = gameMode,
            Address = address,
            Scene = scene,
            SessionName = "Room",
            Initialized = initalized,
            SceneManager = sceneManager
        });
    }
    
}
