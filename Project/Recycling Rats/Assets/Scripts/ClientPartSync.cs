using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ClientPartSync : NetworkBehaviour
{
    public GameObject bodyPrefab;
    public GameObject wheelPrefab;
    public GameObject spikePrefab;
    public GameObject boosterPrefab;
    public GameObject armourPrefab;
    public GameObject springPrefab;

    [ServerRpc(RequireOwnership = false)]
    public void RequestPartSpawnServerRpc(string partType, Vector3 position, Vector3 rotation)
    {
        GameObject prefab = GetPrefab(partType);
        if (prefab == null) return;

        GameObject rightCar = GameObject.FindWithTag("Right Car");
        if (rightCar == null)
        {
            Debug.LogWarning("Host: Right Car not found.");
            return;
        }

        GameObject clone = Instantiate(prefab, position, Quaternion.Euler(rotation), rightCar.transform);
        clone.transform.localScale = Vector3.one;

        LoadPart.createJoint = true;
    }

    [ClientRpc]
    public void SpawnPartOnClientRpc(string partType, Vector3 position, Vector3 rotation)
    {
        GameObject prefab = GetPrefab(partType);
        if (prefab == null) return;

        GameObject leftcar = GameObject.FindWithTag("Left Car");
        if (leftcar == null)
        {
            Debug.LogWarning("Host: Right Car not found.");
            return;
        }

        GameObject clone = Instantiate(prefab, position, Quaternion.Euler(rotation), leftcar.transform);
        clone.transform.localScale = Vector3.one;

        LoadPart.createJoint = true;
    }

    private GameObject GetPrefab(string type)
    {
        return type switch
        {
            "body" => bodyPrefab,
            "wheel" => wheelPrefab,
            "spike" => spikePrefab,
            "booster" => boosterPrefab,
            "armour" => armourPrefab,
            "spring" => springPrefab,
            _ => null
        };
    }
}