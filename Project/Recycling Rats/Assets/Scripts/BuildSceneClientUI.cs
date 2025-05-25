using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;
using System.Collections;

public class BuildSceneClientUI : MonoBehaviour
{
    public Button readyButton;

    private void Start()
    {
        readyButton.interactable = false;
        StartCoroutine(EnableWhenPlayerObjectReady());
    }

    IEnumerator EnableWhenPlayerObjectReady()
    {
        // Wait until Netcode assigns the PlayerObject
        yield return new WaitUntil(() =>
            NetworkManager.Singleton != null &&
            NetworkManager.Singleton.IsClient &&
            NetworkManager.Singleton.LocalClient != null &&
            NetworkManager.Singleton.LocalClient.PlayerObject != null
        );

        readyButton.interactable = true;
        readyButton.onClick.AddListener(OnClientReady);
    }

    void OnClientReady()
    {
        var player = NetworkManager.Singleton.LocalClient.PlayerObject;
        if (player != null)
        {
            player.GetComponent<ClientReadyCommunicator>().SendReadyServerRpc();
            readyButton.interactable = false;
        }
        else
        {
            Debug.LogError("PlayerObject is still null on client.");
        }
    }
}
