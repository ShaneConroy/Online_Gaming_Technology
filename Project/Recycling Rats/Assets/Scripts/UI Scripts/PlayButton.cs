using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;

[System.Serializable]
public class SerializableObject
{
    public string name;

    // Transform properties
    public Vector3 position;
    public Quaternion rotation;
    public Vector3 scale;

    // Rigidbody properties
    public bool hasRigidbody;
    public Vector3 velocity;
    public Vector3 angularVelocity;
    public bool useGravity;
    public bool isKinematic;
    public float mass;

    // Add fields for constraints
    public bool freezePositionX;
    public bool freezePositionY;
    public bool freezePositionZ;
    public bool freezeRotationX;
    public bool freezeRotationY;
    public bool freezeRotationZ;

    // Joint properties
    public bool hasJoint;
    public string jointType;  // Store the joint type (Fixed, Hinge, etc.)
    public Vector3 anchor;  // Serialize the anchor position
    public Vector3 connectedAnchor;  // Serialize the connected anchor position
    public float breakForce;
    public float breakTorque;

    // BoxCollider properties
    public bool hasBoxCollider;
    public Vector3 colliderCenter;
    public Vector3 colliderSize;
    public bool isTrigger;

    public bool hasSphereCollider;
    public Vector3 sphereCenter;
    public float sphereRadius;

    public bool hasCapsuleCollider;
    public Vector3 capsuleCenter;
    public float capsuleRadius;
    public float capsuleHeight;
    public int capsuleDirection;


    // MeshFilter properties
    public bool hasMeshFilter;
    public string meshName;

    // MeshRenderer properties
    public bool hasMeshRenderer;
    public Color materialColor;
    public bool isVisible;

    // Recursively handle children
    public List<SerializableObject> children = new List<SerializableObject>();
}

public class PlayButton : MonoBehaviour
{
    public Button button;
    public GameObject prefabInstance;

    private string savePath;

    // Start is called before the first frame update
    void Start()
    {
        savePath = Application.persistentDataPath + "/prefabData.json";
    }

    // Function to change the scene
    public void ChangeScene()
    {
        if (prefabInstance != null)
        {
            SerializableObject data = SerializeObject(prefabInstance);
            string jsonData = JsonUtility.ToJson(data, true);
            File.WriteAllText(savePath, jsonData);

            Debug.Log("Prefab state saved to: " + savePath);
        }
        else
        {
            Debug.LogWarning("Prefab instance not assigned.");
        }

        // Load the new scene
        SceneManager.LoadScene("Battle Scene");
    }

    SerializableObject SerializeObject(GameObject obj)
    {
        SerializableObject serialized = new SerializableObject
        {
            name = obj.name,
            position = obj.transform.localPosition,
            rotation = obj.transform.localRotation,
            scale = obj.transform.localScale
        };

        // Rigidbody
        Rigidbody rb = obj.GetComponent<Rigidbody>();
        if (rb != null)
        {
            serialized.hasRigidbody = true;
            serialized.velocity = rb.velocity;
            serialized.angularVelocity = rb.angularVelocity;
            serialized.useGravity = rb.useGravity;
            serialized.isKinematic = rb.isKinematic;
            serialized.mass = rb.mass;

            // Save Rigidbody constraints
            serialized.freezePositionZ = rb.constraints.HasFlag(RigidbodyConstraints.FreezePositionZ);
            serialized.freezeRotationX = rb.constraints.HasFlag(RigidbodyConstraints.FreezeRotationX);
            serialized.freezeRotationY = rb.constraints.HasFlag(RigidbodyConstraints.FreezeRotationY);
            serialized.freezeRotationZ = rb.constraints.HasFlag(RigidbodyConstraints.FreezeRotationZ);
        }

        // Joint serialization (Updated for anchor points)
        Joint joint = obj.GetComponent<Joint>();
        if (joint != null)
        {
            serialized.hasJoint = true;
            serialized.anchor = joint.anchor;  // Capture the anchor point
            serialized.connectedAnchor = joint.connectedAnchor;  // Capture the connected anchor
            serialized.breakForce = joint.breakForce;
            serialized.breakTorque = joint.breakTorque;

            // Save the joint type (FixedJoint, HingeJoint, etc.)
            serialized.jointType = joint.GetType().Name;
        }

        // BoxCollider
        BoxCollider box = obj.GetComponent<BoxCollider>();
        if (box != null)
        {
            serialized.hasBoxCollider = true;
            serialized.colliderCenter = box.center;
            serialized.colliderSize = box.size;
            serialized.isTrigger = box.isTrigger;
        }

        // SphereCollider
        SphereCollider sphereCollider = obj.GetComponent<SphereCollider>();
        if (sphereCollider != null)
        {
            serialized.hasSphereCollider = true;
            serialized.sphereCenter = sphereCollider.center;
            serialized.sphereRadius = sphereCollider.radius;
        }

        // CapsuleCollider
        CapsuleCollider capsuleCollider = obj.GetComponent<CapsuleCollider>();
        if (capsuleCollider != null)
        {
            serialized.hasCapsuleCollider = true;
            serialized.capsuleCenter = capsuleCollider.center;
            serialized.capsuleRadius = capsuleCollider.radius;
            serialized.capsuleHeight = capsuleCollider.height;
            serialized.capsuleDirection = capsuleCollider.direction;
        }

        // MeshFilter
        MeshFilter meshFilter = obj.GetComponent<MeshFilter>();
        if (meshFilter != null && meshFilter.sharedMesh != null)
        {
            serialized.hasMeshFilter = true;
            serialized.meshName = meshFilter.sharedMesh.name;
        }

        // MeshRenderer
        MeshRenderer renderer = obj.GetComponent<MeshRenderer>();
        if (renderer != null && renderer.material != null)
        {
            serialized.hasMeshRenderer = true;
            serialized.materialColor = renderer.material.color;
            serialized.isVisible = renderer.enabled;
        }

        // Recursively handle children
        foreach (Transform child in obj.transform)
        {
            serialized.children.Add(SerializeObject(child.gameObject));
        }

        return serialized;
    }

}
