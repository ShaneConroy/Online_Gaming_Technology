using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAttachment : MonoBehaviour
{
    public GameObject vehicle;
    public GameObject[] objectToInstantiate; // Array of prefabs to choose from
    public Transform frontPosition;

    private void Start()
    {
        if (vehicle == null)
        {
            vehicle = GameObject.Find("Car Prototype 1");
        }
        if (frontPosition == null)
        {
            frontPosition = vehicle.transform;
        }
    }

    public void OnButtonClick()
    {
        // Randomly select a prefab from the objectToInstantiate array
        int randomIndex = Random.Range(0, objectToInstantiate.Length);
        GameObject selectedObject = objectToInstantiate[randomIndex];

        // Instantiate the selected prefab
        GameObject newObject = Instantiate(selectedObject, frontPosition.position, frontPosition.rotation);

        // Set the parent and add the FixedJoint
        newObject.transform.parent = vehicle.transform;

        FixedJoint joint = newObject.AddComponent<FixedJoint>();
        joint.connectedBody = vehicle.GetComponent<Rigidbody>();
    }
}
