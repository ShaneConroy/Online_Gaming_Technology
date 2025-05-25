using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadPart : MonoBehaviour
{
    public GameObject bodyPrefab;
    public GameObject wheelPrefab;
    public GameObject spikePrefab;
    public GameObject boosterPrefab;
    public GameObject armourPrefab;

    public TMP_InputField prefabNameInput;
    public Button prefabSave;

    public GameObject springPrefab;
    public GameObject CarBuild;

    public Button LoadBody;
    public Button rotateButton;

    Vector3 pieceOffset;
    Vector3 piecePosition;
    Vector3 spawnPos = new Vector3(-6, 1, 0);
    float pieceSpawnOffset = 3.0f;

    GameObject parts;

    private string currentPartType = "";
    public static bool createJoint = false;
    bool partHeld = false;
    bool grabbed = false;
    int timer = 0;
    int renameTimer = 0;

    private void Start()
    {
#if !UNITY_EDITOR
    if (prefabNameInput != null)
        prefabNameInput.gameObject.SetActive(false);

    if (prefabSave != null)
        prefabSave.gameObject.SetActive(false);
#endif

        PrefabCreatorAndLoader.SpawnPoint = CarBuild.transform.position;
        prefabNameInput.text = "CarBuild";
        rotateButton.gameObject.SetActive(false);
    }

    private void FixedUpdate()
    {
        if (timer > 0)
        {
            timer--;
        }
        if (timer < 0)
        {
            timer = 0;
        }
        if (renameTimer > 0)
        {
            renameTimer--;
        }
        if (renameTimer == 1)
        {
            renameSpawnedCar();
        }
    }

    public void removeOriginalCarBuild()
    {
        GameObject[] cars = GameObject.FindGameObjectsWithTag("Left Car");

        foreach (GameObject car in cars)
        {
            Destroy(car);
        }
        renameTimer = 10;

        cars = GameObject.FindGameObjectsWithTag("Right Car");

        foreach (GameObject car in cars)
        {
            Destroy(car);
        }
        renameTimer = 10;
    }

    public void renameSpawnedCar()
    {
        GameObject found = GameObject.FindWithTag("Left Car");
        if (found != null)
        {
            found.name = "CarBuild";
            CarBuild = found;
        }
        else
        {
            Debug.LogWarning("Left Car tag does not exist :(");
        }


        found = GameObject.FindWithTag("Right Car");
        if (found != null)
        {
            found.name = "RightCarBuild";
            CarBuild = found;
        }
        else
        {
            Debug.LogWarning("Right Car tag does not exist :(");
        }
    }

    private void Update()
    {
        if (parts != null && partHeld)
        {
            if (Input.GetMouseButton(0))
            {
                Vector3 inputPos = Input.mousePosition;
                inputPos.z = Camera.main.nearClipPlane;
                piecePosition = Camera.main.ScreenToWorldPoint(inputPos);
                grabbed = true;
            }

            parts.transform.position = new Vector3(
                Mathf.Round(piecePosition.x) - pieceOffset.x,
                Mathf.Round(piecePosition.y) + pieceOffset.y,
                pieceOffset.z
            );
        }

        if (partHeld && Input.GetMouseButtonUp(0) && grabbed && timer <= 0)
        {
            createJoint = true;
            partHeld = false;
            grabbed = false;

            Vector3 pos = parts.transform.position;
            Vector3 rot = parts.transform.rotation.eulerAngles;

            if (Unity.Netcode.NetworkManager.Singleton.IsClient && !Unity.Netcode.NetworkManager.Singleton.IsHost)
            {
                FindObjectOfType<ClientPartSync>().RequestPartSpawnServerRpc(currentPartType, pos, rot);
                createJoint = true;
            }
            else if(Unity.Netcode.NetworkManager.Singleton.IsHost)
            {
                FindObjectOfType<ClientPartSync>().SpawnPartOnClientRpc(currentPartType, pos, rot);
                createJoint = true;
            }
        }
    }

    public void LoadBodyOnClick()
    {
        if (partHeld == false)
        {
            timer = 10;
            partHeld = true;
            Vector3 inputPos = Input.mousePosition;
            inputPos.z = Camera.main.nearClipPlane;
            piecePosition = Camera.main.ScreenToWorldPoint(inputPos);
            piecePosition.y += pieceSpawnOffset;
            pieceOffset = new Vector3(0, 0, 0);

            GameObject parent;
            if (Unity.Netcode.NetworkManager.Singleton.IsClient && !Unity.Netcode.NetworkManager.Singleton.IsHost)
            {
                parent = GameObject.FindWithTag("Right Car");
            }
            else
            {
                parent = CarBuild;
            }

            currentPartType = "body";

            parts = Instantiate(bodyPrefab, parent.transform);
            parts.transform.position = new Vector3(piecePosition.x, piecePosition.y, 0);
            parts.transform.rotation = Quaternion.identity;
            parts.transform.localScale = new Vector3(1, 1, 1);

            //if (Unity.Netcode.NetworkManager.Singleton.IsClient && !Unity.Netcode.NetworkManager.Singleton.IsHost)
            //{
            //    Vector3 rot = parts.transform.rotation.eulerAngles;
            //    FindObjectOfType<ClientPartSync>().RequestPartSpawnServerRpc("body", parts.transform.position, rot);
            //}

            Input.ResetInputAxes();

            rotateButton.gameObject.SetActive(false);
        }
    }

    public void LoadWheelOnClick()
    {
        if (partHeld == false)
        {
            timer = 10;
            partHeld = true;
            Vector3 inputPos = Input.mousePosition;
            inputPos.z = Camera.main.nearClipPlane;
            piecePosition = Camera.main.ScreenToWorldPoint(inputPos);
            piecePosition.y += pieceSpawnOffset;
            pieceOffset = new Vector3(2, 0, 0);

            GameObject parent;
            if (Unity.Netcode.NetworkManager.Singleton.IsClient && !Unity.Netcode.NetworkManager.Singleton.IsHost)
            {
                parent = GameObject.FindWithTag("Right Car");
            }
            else
            {
                parent = CarBuild;
            }

            currentPartType = "wheel";

            parts = Instantiate(wheelPrefab, parent.transform);
            parts.transform.position = new Vector3(piecePosition.x + pieceOffset.x, piecePosition.y + pieceOffset.y, 0);
            parts.transform.rotation = Quaternion.Euler(90, 0, 0);
            parts.transform.localScale = new Vector3(1, 1, 1);

            rotateButton.gameObject.SetActive(false);
        }
    }

    public void LoadSpikeOnClick()
    {
        if (partHeld == false)
        {
            timer = 10;
            partHeld = true;
            Vector3 inputPos = Input.mousePosition;
            inputPos.z = Camera.main.nearClipPlane;
            piecePosition = Camera.main.ScreenToWorldPoint(inputPos);
            piecePosition.y += pieceSpawnOffset;
            pieceOffset = new Vector3(4, 0, 0);

            GameObject parent;
            if (Unity.Netcode.NetworkManager.Singleton.IsClient && !Unity.Netcode.NetworkManager.Singleton.IsHost)
            {
                parent = GameObject.FindWithTag("Right Car");
            }
            else
            {
                parent = CarBuild;
            }

            currentPartType = "spike";

            parts = Instantiate(spikePrefab, parent.transform);
            parts.transform.position = new Vector3(piecePosition.x + pieceOffset.x, piecePosition.y + pieceOffset.y, 0);
            parts.transform.rotation = Quaternion.Euler(0, 0, 0);
            parts.transform.localScale = new Vector3(1, 1, 1);

            rotateButton.gameObject.SetActive(true);
        }
    }

    public void LoadBoosterOnClick()
    {
        if (partHeld == false)
        {
            timer = 10;
            partHeld = true;
            Vector3 inputPos = Input.mousePosition;
            inputPos.z = Camera.main.nearClipPlane;
            piecePosition = Camera.main.ScreenToWorldPoint(inputPos);
            piecePosition.y += pieceSpawnOffset;
            pieceOffset = new Vector3(6, 0, 0);

            GameObject parent;
            if (Unity.Netcode.NetworkManager.Singleton.IsClient && !Unity.Netcode.NetworkManager.Singleton.IsHost)
            {
                parent = GameObject.FindWithTag("Right Car");
            }
            else
            {
                parent = CarBuild;
            }

            currentPartType = "booster";

            parts = Instantiate(boosterPrefab, parent.transform);
            parts.transform.position = new Vector3(piecePosition.x + pieceOffset.x, piecePosition.y + pieceOffset.y, 0);
            parts.transform.rotation = Quaternion.Euler(0, 0, 0);
            parts.transform.localScale = new Vector3(1, 1, 1);

            rotateButton.gameObject.SetActive(true);
        }
    }

    public void LoadArmourOnClick()
    {
        if (partHeld == false)
        {
            timer = 10;
            partHeld = true;
            Vector3 inputPos = Input.mousePosition;
            inputPos.z = Camera.main.nearClipPlane;
            piecePosition = Camera.main.ScreenToWorldPoint(inputPos);
            piecePosition.y += pieceSpawnOffset;
            pieceOffset = new Vector3(8, 0, 0);

            GameObject parent;
            if (Unity.Netcode.NetworkManager.Singleton.IsClient && !Unity.Netcode.NetworkManager.Singleton.IsHost)
            {
                parent = GameObject.FindWithTag("Right Car");
            }
            else
            {
                parent = CarBuild;
            }

            currentPartType = "armour";

            parts = Instantiate(armourPrefab, parent.transform);
            parts.transform.position = new Vector3(piecePosition.x + pieceOffset.x, piecePosition.y + pieceOffset.y, 0);
            parts.transform.rotation = Quaternion.Euler(0, 0, 0);
            parts.transform.localScale = new Vector3(1, 1, 1);

            rotateButton.gameObject.SetActive(false);
        }
    }

    public void rotateButtoner()
    {
        if (parts.tag == "Spike")
        {
            parts.transform.Rotate(0, 180, 0);
        }
        if (parts.tag == "Booster")
        {
            parts.transform.Rotate(0, 180, 0);
        }
    }

    public void LoadSpringOnClick()
    {
        if (partHeld == false)
        {
            timer = 10;
            partHeld = true;
            Vector3 inputPos = Input.mousePosition;
            inputPos.z = Camera.main.nearClipPlane;
            piecePosition = Camera.main.ScreenToWorldPoint(inputPos);
            piecePosition.y += pieceSpawnOffset;
            pieceOffset = new Vector3(11, 0, 0);

            currentPartType = "spring";

            parts = Instantiate(springPrefab, CarBuild.transform);
            parts.transform.position = new Vector3(piecePosition.x + pieceOffset.x, piecePosition.y + pieceOffset.y, 0);
            parts.transform.rotation = Quaternion.Euler(0, 0, 0);
            parts.transform.localScale = new Vector3(1, 1, 1);
        }
    }
}