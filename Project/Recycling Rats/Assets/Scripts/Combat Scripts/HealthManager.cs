using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    private int myMaxHealth = 3;
    public int myHealth;

    private Slider healthBarSlider;
    private Canvas healthBarCanvas;
    private FixedJoint fixedJoint;

    private void Start()
    {
        myHealth = myMaxHealth;

        healthBarCanvas = GetComponentInChildren<Canvas>();
        fixedJoint = GetComponent<FixedJoint>();


        if (healthBarCanvas != null)
        {
            healthBarSlider = healthBarCanvas.GetComponentInChildren<Slider>();
            UpdateHealthBar();
        }
    }

    public void TakeDamage(int damage)
    {
        myHealth -= damage;

        if (myHealth <= 0)
        {
            BreakOff();
        }
        else
        {
            UpdateHealthBar();
        }
    }

    public void BreakOff()
    {
        Collider[] childColliders = GetComponentsInChildren<Collider>();

        foreach (Collider childCollider in childColliders)
        {
            childCollider.enabled = false;
        }

        // Disable collisions
        gameObject.GetComponent<Collider>().enabled = false;
        fixedJoint.breakForce = 0;

        Destroy(healthBarCanvas.gameObject);
    }

    private void UpdateHealthBar()
    {
        if (healthBarSlider != null)
        {
            healthBarSlider.value = (float)myHealth / myMaxHealth;

            //Only show when health not full
            if (myHealth == myMaxHealth)
            {
                healthBarCanvas.gameObject.SetActive(false);
            }
            else
            {
                healthBarCanvas.gameObject.SetActive(true);
            }
        }
    }

    public int GetHealth()
    {
        Transform obj = this.gameObject.transform;

        while (obj.parent != null)
        {
            obj = obj.parent; // Keep moving up the hierarchy
        }
        if (obj.CompareTag("Left Car"))
        {
            if (myHealth <= 0)
            {
                return 1;//player dead
            }
        }
        else if (obj.CompareTag("Right Car"))
        {
            if (myHealth <= 0)
            {
                return 2;//enemy dead
            }
        }
        return 0;//no one dead
    }
}
