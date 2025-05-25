using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class CollisionHandler : MonoBehaviour
{
    private Rigidbody rb;
    public Transform parentObj;
    public int health = 100;
    public float collisionForce = 3;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        //while (parentObj.parent != null)
        //{
        //    parentObj = parentObj.parent;
        //}
    }

    private void FixedUpdate()
    {
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Get the topmost parent's tag for both objects
        string myTopParentTag = GetTopParentTag(this.transform);
        string otherTopParentTag = GetTopParentTag(collision.transform);
        if (myTopParentTag != otherTopParentTag)
        {
            if(collision.gameObject.tag == "Ground" || collision.gameObject.tag == "Wall")
            {
                //exclude collisions not related to the cars
            }


            else if (collision.gameObject.tag == "LeftWall" && myTopParentTag == "Left Car")
            {
                GameManager.leftPlayerLeftWallHit = true;
                GameManager.leftPlayerRightWallHit = false;
            }
            else if (collision.gameObject.tag == "RightWall" && myTopParentTag == "Left Car")
            {
                GameManager.leftPlayerRightWallHit = true;
                GameManager.leftPlayerLeftWallHit = false;
            }

            else if (collision.gameObject.tag == "LeftWall" && myTopParentTag == "Right Car")
            {
                GameManager.rightPlayerLeftWallHit = false;
                GameManager.rightPlayerRightWallHit = true;
            }
            else if (collision.gameObject.tag == "RightWall" && myTopParentTag == "Right Car")
            {
                GameManager.rightPlayerLeftWallHit = true;
                GameManager.rightPlayerRightWallHit = false;
            }


            else if (this.CompareTag("Armour") && collision.gameObject.CompareTag("Spike")) // CarBody -> Carbody
            {
                rb.AddForce(collision.relativeVelocity * collisionForce, ForceMode.Impulse);
                health -= 101;
            }
            else if (this.CompareTag("Spike") && collision.gameObject.CompareTag("Spike")) // CarBody -> Carbody
            {
                rb.AddForce(collision.relativeVelocity * collisionForce, ForceMode.Impulse);
                health -= 75;
            }
            else if(collision.gameObject.CompareTag("Spike"))
            {
                rb.AddForce(collision.relativeVelocity * collisionForce, ForceMode.Impulse);
                health -= 75;
            }
            else
            {
                rb.AddForce(collision.relativeVelocity * collisionForce, ForceMode.Impulse);
                health -= 36;
            }


        }
        //else Debug.Log("Colliding with ourselves");
    }

    // Recursive function to find the topmost parent
    private string GetTopParentTag(Transform obj)
    {
        while (obj.parent != null)
        {
            //parentObj = obj.parent;
            obj = obj.parent; // Keep moving up the hierarchy
        }
        return obj.tag;
    }
}
