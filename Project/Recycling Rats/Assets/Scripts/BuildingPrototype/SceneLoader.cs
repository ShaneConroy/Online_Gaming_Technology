using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Unity.Netcode;

public class SceneLoader : NetworkBehaviour
{
    public string SceneName;
    public GameObject Car;
    public Rigidbody[] cockpit;
    public GameObject Enemy;
    public Button startGameButton;

    public NetworkVariable<bool> isClientReady = new NetworkVariable<bool>(
        false,
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Server
    );

    private NetworkVariable<bool> clientReadyNetVar = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

    IEnumerator Start()
    {
        yield return new WaitUntil(() => BuildSceneNetworkState.Instance != null);

        BuildSceneNetworkState.Instance.clientIsReady.OnValueChanged += (oldVal, newVal) =>
        {
            if (newVal)
            {
                startGameButton.gameObject.SetActive(true);
            }
        };
    }

    [ServerRpc(RequireOwnership = false)]
    public void ClientReadyUpServerRpc(ServerRpcParams rpcParams = default)
    {
        Debug.Log($"Client {rpcParams.Receive.SenderClientId} is ready!");
        isClientReady.Value = true;
    }


    public void OnClientReadyUp()
    {
        if (!IsHost)
        {
            Debug.Log("Client clicked Ready!");
            ClientReadyUpServerRpc();
        }
    }

    public void OnHostStartGame()
    {
        if (!IsHost) return;

        if (!isClientReady.Value)
        {
            Debug.Log("Client is NOT ready. Cannot start.");
            return;
        }

        LoadSceneClientRpc();
        StartCoroutine(LoadYourAsyncScene());
    }



    [ClientRpc]
    void LoadSceneClientRpc()
    {
        if (!IsHost)
        {
            Car = GameObject.FindGameObjectWithTag("Left Car");
            Enemy = GameObject.FindGameObjectWithTag("Right Car");

            GameObject[] cockpitObjects = GameObject.FindGameObjectsWithTag("Cockpit");
            cockpit = new Rigidbody[cockpitObjects.Length];
            for (int i = 0; i < cockpitObjects.Length; i++)
            {
                cockpit[i] = cockpitObjects[i].GetComponent<Rigidbody>();
            }
            StartCoroutine(LoadYourAsyncScene());
        }
    }

    public void loadScene()
    {
        Car = GameObject.FindGameObjectWithTag("Left Car");
        Enemy = GameObject.FindGameObjectWithTag("Right Car");

        GameObject[] cockpitObjects = GameObject.FindGameObjectsWithTag("Cockpit");
        cockpit = new Rigidbody[cockpitObjects.Length];
        for (int i = 0; i < cockpitObjects.Length; i++)
        {
            cockpit[i] = cockpitObjects[i].GetComponent<Rigidbody>();
        }

        OnHostStartGame();
    }

    IEnumerator LoadYourAsyncScene()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(SceneName, LoadSceneMode.Additive);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        foreach (Rigidbody rb in cockpit)
        {
            if (rb != null)
            {
                rb.useGravity = true;
                rb.isKinematic = false;
            }
        }

        CarMovement.movingAllowed = true;

        if (Car != null && Car.tag == "Left Car")
        {
            Car.transform.SetPositionAndRotation(new Vector3(-10f, 4f, 0), transform.rotation);
        }
        if (Enemy != null && Enemy.tag == "Right Car")
        {
            Enemy.transform.SetPositionAndRotation(new Vector3(10f, 4f, 0), transform.rotation);
        }

        if (Car != null)
        {
            if (!Car.GetComponent<CarMovement>()) Car.AddComponent<CarMovement>();
            if (!Car.GetComponent<HealthManager>()) Car.AddComponent<HealthManager>();
        }

        if (Enemy != null)
        {
            if (!Enemy.GetComponent<CarMovement>()) Enemy.AddComponent<CarMovement>();
            if (!Enemy.GetComponent<HealthManager>()) Enemy.AddComponent<HealthManager>();
        }

        if (Car != null)
            SceneManager.MoveGameObjectToScene(Car, SceneManager.GetSceneByName(SceneName));

        if (Enemy != null)
            SceneManager.MoveGameObjectToScene(Enemy, SceneManager.GetSceneByName(SceneName));

        yield return new WaitForSeconds(1f);

        SceneManager.UnloadSceneAsync(currentScene);
    }
}
