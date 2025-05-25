using UnityEngine;
using System.Collections;
//using UnityEditor.Experimental.GraphView;

public class ForwardReverseMovement : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float moveForce = 5f;
    public float reverseForce = 0.1f;

    public float reverseDuration = 0.5f;
    public float speedThreshold = 3f;
    public bool inverted = false;
    private bool reversing = false;
    private bool allowAcceleration = true;
    private Rigidbody rb;
    private int hitsCounts;
    private float bigBoom = 300f;

    float explosionOffset = 1;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if(inverted == true)
        {
            explosionOffset = -explosionOffset;
        }
    }

    void FixedUpdate()
    {
        if (reversing == false && allowAcceleration == true)
        {
            MoveForward();
        }
        else if(reversing == true)
        {
            MoveBackwards();
        }

    }

    void OnCollisionEnter(Collision collision)
    {
        if (reversing != true && collision.gameObject.tag == "CarBody")
        {
            hitsCounts++;
            Debug.Log(hitsCounts);
            StartCoroutine(ReverseMovement());
        }
    }

    void MoveForward()
    {
        if(inverted == false)
        {
            rb.AddForce(new Vector3(moveForce, 0, 0), ForceMode.Acceleration);
        }
        else{
            rb.AddForce(new Vector3(-moveForce, 0, 0), ForceMode.Acceleration);
        }
    }

    void MoveBackwards()
    {
        if (inverted == false)
        {
            rb.AddForce(new Vector3(-reverseForce, 0, 0), ForceMode.Acceleration);
        }
        else
        {
            rb.AddForce(new Vector3(reverseForce, 0, 0), ForceMode.Acceleration);
        }
    }

    IEnumerator ReverseMovement()
    {
        Vector3 explosionPoint = this.transform.position;
        explosionPoint.y = explosionPoint.y - 0.5f;
        explosionPoint.x = explosionPoint.x + explosionOffset;
        if(hitsCounts < 3)
        {
            rb.AddExplosionForce(100f, explosionPoint, 10f);//add a burst of force
        }
        else
        {
            rb.AddExplosionForce(bigBoom, explosionPoint, 10f);//add a burst of force
            if(bigBoom < 500)
                bigBoom = bigBoom + 100f;

            hitsCounts = 0;
        }
        allowAcceleration = false;
        yield return new WaitForSeconds(0.1f);
        reversing = true;
        reversing = false;
        allowAcceleration = true;
    }
}