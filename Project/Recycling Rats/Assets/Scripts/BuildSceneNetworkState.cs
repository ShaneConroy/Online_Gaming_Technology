using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class BuildSceneNetworkState : NetworkBehaviour
{
    public static BuildSceneNetworkState Instance;

    public NetworkVariable<bool> clientIsReady = new NetworkVariable<bool>(false);

    private void Awake()
    {
        Instance = this;
    }

    [ServerRpc(RequireOwnership = false)]
    public void SetClientReadyServerRpc(bool value)
    {
        clientIsReady.Value = value;
    }
}
