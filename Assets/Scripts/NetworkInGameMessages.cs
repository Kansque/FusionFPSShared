using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
public class NetworkInGameMessages : NetworkBehaviour
{
    InGameMessageUI inGameMessageUI;

    private void Start()
    {

    }

    public void SendInGameRPCMessage(string userName, string message)
    {
        RPC_InGameMessage($"<b>{userName}</b> {message}");
    }


    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    public void RPC_InGameMessage(string message, RpcInfo info = default)
    {
        if (inGameMessageUI == null)
            inGameMessageUI = NetworkPlayer.Local.GetComponent<InGameMessageUI>();

        if (inGameMessageUI != null)
            inGameMessageUI.OnGameMessageReceived(message);
    }
}
