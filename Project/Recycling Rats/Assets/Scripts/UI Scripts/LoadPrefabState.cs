using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class LoadPrefabState : MonoBehaviour
{
    public GameObject prefabToInstantiate; 
    private string savePath;

    // Start is called before the first frame update
    void Start()
    {
        savePath = Application.persistentDataPath + "/prefabData.json";
        LoadSavedState();
    }

    public void LoadSavedState()
    {
        //if (File.Exists(savePath))
        //{
        //    string jsonData = File.ReadAllText(savePath);
        //    SerializableObject data = JsonUtility.FromJson<SerializableObject>(jsonData);

        //    GameObject newPrefab = Instantiate(prefabToInstantiate);
        //    newPrefab.transform.localScale = data.scale;
        //    DeserializeObject(newPrefab, data);

        //    Debug.Log("Prefab state loaded from: " + savePath);
        //}
        //else
        //{
        //    Debug.LogWarning("No saved data found.");
        //}
    }

    void DeserializeObject(GameObject obj, SerializableObject data)
    {
        obj.name = data.name;
        obj.transform.localPosition = data.position;
        obj.transform.localRotation = data.rotation;
        obj.transform.localScale = data.scale;

        // Rigidbody
        if (data.hasRigidbody)
        {
            Rigidbody rb = obj.GetComponent<Rigidbody>();
            if (rb == null)
            {
                rb = obj.AddComponent<Rigidbody>();
            }

            if (rb != null)
            {
                rb.velocity = data.velocity;
                rb.angularVelocity = data.angularVelocity;
                rb.useGravity = data.useGravity;
                rb.isKinematic = data.isKinematic;
                rb.mass = data.mass;

                // Apply Rigidbody constraints
                RigidbodyConstraints constraints = RigidbodyConstraints.None;
                if (data.freezePositionZ) constraints |= RigidbodyConstraints.FreezePositionZ;
                if (data.freezeRotationX) constraints |= RigidbodyConstraints.FreezeRotationX;
                if (data.freezeRotationY) constraints |= RigidbodyConstraints.FreezeRotationY;
                if (data.freezeRotationZ) constraints |= RigidbodyConstraints.FreezeRotationZ;

                rb.constraints = constraints;
            }
        }

        // Joint deserialization (Updated for anchor points)
        if (data.hasJoint)
        {
            Joint joint = obj.GetComponent<Joint>();
            if (joint == null)
            {
                // Add the correct type of joint based on serialized type
                if (data.jointType == "FixedJoint")
                {
                    joint = obj.AddComponent<FixedJoint>();
                }
                else if (data.jointType == "HingeJoint")
                {
                    joint = obj.AddComponent<HingeJoint>();
                }
                else if (data.jointType == "SpringJoint")
                {
                    joint = obj.AddComponent<SpringJoint>();
                }
                else if (data.jointType == "ConfigurableJoint")
                {
                    joint = obj.AddComponent<ConfigurableJoint>();
                }

                Debug.Log($"Joint of type {data.jointType} added to: {obj.name}");
            }

            if (joint != null)
            {
                joint.anchor = data.anchor;  // Apply the anchor point
                joint.connectedAnchor = data.connectedAnchor;  // Apply the connected anchor point
                joint.breakForce = data.breakForce;
                joint.breakTorque = data.breakTorque;
            }
        }

        // BoxCollider
        if (data.hasBoxCollider)
        {
            BoxCollider box = obj.GetComponent<BoxCollider>();
            if (box == null)
            {
                box = obj.AddComponent<BoxCollider>();
            }

            if (box != null)
            {
                box.center = data.colliderCenter;
                box.size = data.colliderSize;
                box.isTrigger = data.isTrigger;
            }
        }

        // SphereCollider
        if (data.hasSphereCollider)
        {
            SphereCollider sphereCollider = obj.GetComponent<SphereCollider>();
            if (sphereCollider == null)
            {
                sphereCollider = obj.AddComponent<SphereCollider>();
            }

            if (sphereCollider != null)
            {
                sphereCollider.center = data.sphereCenter;
                sphereCollider.radius = data.sphereRadius;
            }
        }

        // CapsuleCollider
        if (data.hasCapsuleCollider)
        {
            CapsuleCollider capsuleCollider = obj.GetComponent<CapsuleCollider>();
            if (capsuleCollider == null)
            {
                capsuleCollider = obj.AddComponent<CapsuleCollider>();
            }

            if (capsuleCollider != null)
            {
                capsuleCollider.center = data.capsuleCenter;
                capsuleCollider.radius = data.capsuleRadius;
                capsuleCollider.height = data.capsuleHeight;
                capsuleCollider.direction = data.capsuleDirection;
            }
        }

        // MeshFilter
        if (data.hasMeshFilter)
        {
            MeshFilter meshFilter = obj.GetComponent<MeshFilter>();
            if (meshFilter == null)
            {
                meshFilter = obj.AddComponent<MeshFilter>();
            }

            if (meshFilter != null && !string.IsNullOrEmpty(data.meshName))
            {
                Mesh mesh = Resources.GetBuiltinResource<Mesh>(data.meshName + ".fbx");
                if (mesh != null)
                {
                    meshFilter.sharedMesh = mesh;
                }
            }
        }

        // MeshRenderer
        if (data.hasMeshRenderer)
        {
            MeshRenderer renderer = obj.GetComponent<MeshRenderer>();
            if (renderer == null)
            {
                renderer = obj.AddComponent<MeshRenderer>();
            }

            if (renderer != null)
            {
                if (renderer.material != null)
                {
                    renderer.material.color = data.materialColor;
                }
                renderer.enabled = data.isVisible;
            }
        }

        // Recursively handle children
        for (int i = 0; i < data.children.Count; i++)
        {
            GameObject childObj;

            if (i < obj.transform.childCount)
            {
                childObj = obj.transform.GetChild(i).gameObject;
            }
            else
            {
                childObj = new GameObject(data.children[i].name);
                childObj.transform.SetParent(obj.transform);
            }

            DeserializeObject(childObj, data.children[i]);
        }
    }
}
