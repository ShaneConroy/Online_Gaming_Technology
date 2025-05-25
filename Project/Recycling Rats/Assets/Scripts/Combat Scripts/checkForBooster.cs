using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class checkForBooster : MonoBehaviour
{
    public Button boosterButton; // Reference to the button

    // Start is called before the first frame update
    void Start()
    {
        boosterButton.gameObject.SetActive(false); // Deactivate the button initially
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.hasBooster)
        {
            boosterButton.gameObject.SetActive(true); // Activate the button
        }
        else
        {
            boosterButton.gameObject.SetActive(false); // Deactivate the button
        }
    }
}
