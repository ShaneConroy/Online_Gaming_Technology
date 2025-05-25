using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BuildSceneLoader : NetworkBehaviour
{
    public static BuildSceneLoader Instance;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    [ClientRpc]
    public void LoadBuildSceneClientRpc()
    {
        if (!IsHost)
        {
            SceneManager.LoadScene("BuildingPlayer2", LoadSceneMode.Single);
        }
    }

    public void LoadHostBuildScene()
    {
        SceneManager.LoadScene("Building", LoadSceneMode.Single);
    }
}