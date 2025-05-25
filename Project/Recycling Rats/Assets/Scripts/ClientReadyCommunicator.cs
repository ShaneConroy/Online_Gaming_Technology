using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ClientReadyCommunicator : NetworkBehaviour
{
    [ServerRpc]
    public void SendReadyServerRpc()
    {
        if (BuildSceneNetworkState.Instance != null)
        {
            BuildSceneNetworkState.Instance.clientIsReady.Value = true;
            Debug.Log("Server received client ready.");
        }
        else
        {
            Debug.LogWarning("BuildSceneNetworkState not found on server.");
        }
    }
}

