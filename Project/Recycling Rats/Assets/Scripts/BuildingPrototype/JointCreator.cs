using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JointCreator : MonoBehaviour
{
    public GameObject cockpit;
    public List<GameObject> carCompnents;
    public GameObject CarGameObj;

    bool turnOffCreateJoint = false;

    // Start is called before the first frame update
    void Start()
    {
        CarGameObj = GetTopmostParent(this.gameObject);

        cockpit = FindCockpit(CarGameObj);

        carCompnents = new List<GameObject>();

        //get all the children but do not add ourself
        foreach (Transform child in CarGameObj.transform)
        {
            if (child.gameObject != this.gameObject)
            {
                carCompnents.Add(child.gameObject);
            }
        }
    }

    private GameObject FindCockpit(GameObject root)
    {
        foreach (Transform child in root.GetComponentsInChildren<Transform>())
        {
            if (child.CompareTag("Cockpit"))
            {
                return child.gameObject;
            }
        }

        Debug.LogWarning("No Cockpit found under car root.");
        return null;
    }

    private GameObject GetTopmostParent(GameObject obj)
    {
        Transform current = obj.transform;
        while (current.parent != null)
        {
            current = current.parent;
        }
        return current.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (cockpit != null && LoadPart.createJoint == true)
        {
            //remove all joints so we can redo them as a change to the cars structure has been made
            Joint[] joints = GetComponents<Joint>();
            foreach (Joint joint in joints)
            {
                Destroy(joint);
            }

            //get the list of components again as a change has been made
            carCompnents = new List<GameObject>();
            foreach (Transform child in CarGameObj.transform)
            {
                if (child.gameObject != this.gameObject)
                {
                    carCompnents.Add(child.gameObject);
                }
            }

            //go through all car pieces and if they are close enough generate a joint between them
            foreach (GameObject component in carCompnents)
            {
                float distance = Vector3.Distance(transform.position, component.transform.position);
                if (distance < 1.1f)
                {
                    if (this.tag == "Wheel")
                    {
                        if (component.tag == "Wheel")
                        {
                            continue;
                        }
                        FixedJoint fixedJoint = gameObject.AddComponent<FixedJoint>();
                        fixedJoint.connectedBody = component.GetComponent<Rigidbody>();
                        //SpringJoint springJoint = gameObject.AddComponent<SpringJoint>();
                        //springJoint.connectedBody = component.GetComponent<Rigidbody>();
                        //springJoint.spring = 20000;
                        //springJoint.damper = 0;
                        //springJoint.minDistance = 0;
                        //springJoint.maxDistance = 0.05f;
                        //springJoint.tolerance = 0.25f;
                        
                    }
                    //else if(component.tag == "Wheel")
                    //{
                    //    Debug.Log("Blocks dont attatch to wheelies");
                    //}
                    else
                    {
                        if(component.tag == "Wheel")
                        {
                            continue;
                        }
                        FixedJoint fixedJoint = gameObject.AddComponent<FixedJoint>();
                        fixedJoint.connectedBody = component.GetComponent<Rigidbody>();
                    }
                }
            }
            //can now stop trying to add joints as they are all now connected or none were found
            turnOffCreateJoint = true;
        }
    }
    private void LateUpdate()
    {
        //make sure all other pieces can do their joints before we swap the bool
        if (turnOffCreateJoint == true)
        {
            turnOffCreateJoint = false;
            LoadPart.createJoint = false;
        }
    }
}
