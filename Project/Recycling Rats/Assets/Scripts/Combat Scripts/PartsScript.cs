using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartsScript : MonoBehaviour
{
    private float moveForce = 22.5f;

    private void FixedUpdate()
    {
        ProcessChildren(transform);
    }

    private void ProcessChildren(Transform parent)
    {
        foreach (Transform child in parent)
        {

            // Ignore health bar
            if (child.CompareTag("Health Bar"))
            {
                continue;
            }

            if (gameObject.CompareTag("Right Car"))
            {
                if (child.CompareTag("Wheel"))
                {
                    child.GetComponent<Rigidbody>().rotation *= Quaternion.Euler(0, 10, 0);
                    child.GetComponent<Rigidbody>().AddForce(new Vector3(-moveForce, 0, 0), ForceMode.Acceleration);
                }
            }
            //else
            //{
            //    if (child.CompareTag("Wheel"))
            //    {
            //        child.GetComponent<Rigidbody>().rotation *= Quaternion.Euler(0, -10, 0);
            //    }
            //    child.GetComponent<Rigidbody>().AddForce(new Vector3(reverseForce, 0, 0), ForceMode.Acceleration);
            //}

            ProcessChildren(child);
        }
    }
}
