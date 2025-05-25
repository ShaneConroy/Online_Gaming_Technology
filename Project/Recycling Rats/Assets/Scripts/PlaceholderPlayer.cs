using Unity.Netcode;
using UnityEngine;

public class DummyPlayer : NetworkBehaviour
{
    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            Debug.Log("DummyPlayer spawned for client ID: " + OwnerClientId);
        }
    }
}