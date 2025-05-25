using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarMovement : MonoBehaviour
{
    private float moveForce = 50f;
    private float reverseForce = 3.0f;
    public static bool movingAllowed = false;
    bool mainPlayer = false;

    private void Start()
    {
        mainPlayer = this.tag == "Left Car";
    }

    private void FixedUpdate()
    {
        if (movingAllowed == true)
        {
            ProcessChildren(transform);
        }
    }

    private void ProcessChildren(Transform parent)
    {
        foreach (Transform child in parent)
        {
            if (child.CompareTag("Wheel") && mainPlayer == true)
            {
                if(GameManager.leftPlayerLeftWallHit == true)
                {
                    child.GetComponent<Rigidbody>().rotation *= Quaternion.Euler(0, -10, 0);
                    child.GetComponent<Rigidbody>().AddForce(new Vector3(moveForce, 0, 0), ForceMode.Acceleration);
                }
                else if(GameManager.leftPlayerRightWallHit == true)
                {
                    child.GetComponent<Rigidbody>().rotation *= Quaternion.Euler(0, 10, 0);
                    child.GetComponent<Rigidbody>().AddForce(new Vector3(-moveForce, 0, 0), ForceMode.Acceleration);
                }
            }
            else if (child.CompareTag("Wheel") && mainPlayer == false)
            {
                if (GameManager.rightPlayerRightWallHit == true)
                {
                    child.GetComponent<Rigidbody>().rotation *= Quaternion.Euler(0, -10, 0);
                    child.GetComponent<Rigidbody>().AddForce(new Vector3(moveForce, 0, 0), ForceMode.Acceleration);
                }
                else if (GameManager.rightPlayerLeftWallHit == true)
                {
                    child.GetComponent<Rigidbody>().rotation *= Quaternion.Euler(0, 10, 0);
                    child.GetComponent<Rigidbody>().AddForce(new Vector3(-moveForce, 0, 0), ForceMode.Acceleration);
                }
            }
        }
    }
}