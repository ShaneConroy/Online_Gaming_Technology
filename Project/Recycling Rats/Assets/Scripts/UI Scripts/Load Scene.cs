using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    public Button myButton;

    private void Start()
    {
        myButton.onClick.AddListener(StartGamePressed);
    }

    void StartGamePressed()
    {
        SceneManager.LoadScene("Battle Scene");
    }

}
