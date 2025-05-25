using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    GameObject enemyCar;
    GameObject playerCar;
    //HealthManager healthManager;
    public UnityEngine.UI.Button startButton;

    CollisionHandler enemyCarCockpitHealth;
    CollisionHandler playerCarCockpitHealth;

    Vector3 playerCarSpawn;
    Vector3 enemyCarSpawn;

    private GameObject playerCarCopy;
    private GameObject enemyCarCopy;
    public GameObject winfx;

    public RawImage ABTestButtonLock;
    public RawImage ABTestButtonLock2;

    public AudioSource boing;

    private GameObject tempCarCopy;
    public TextMeshProUGUI winText;
    public UnityEngine.UI.Button boosterButton;
    public UnityEngine.UI.Button jumpButton;

    public GameObject PlayerHealhBar;
    public GameObject EnemyHealhBar;
    private GameObject[] playerHealthStates;
    private GameObject[] enemyHealthStates;

    public static bool hasBooster = false;
    public static bool hasSpring = false;

    bool jumpUsed = false;

    private int timeAlive = 0;
    private int playerPartCount = 0;
    private int enemyPartCount = 0;

    private int currentPlayerPartCount = 0;
    private int currentEnemyPartCount = 0;

    int playerWins = -1;
    int enemyWins = 0;

    private bool dataSent = false;

    bool allowedToStart = false;

    public static bool leftPlayerLeftWallHit = true;//used so we can tell if the main player has hit a wall
    public static bool leftPlayerRightWallHit = false;

    public static bool rightPlayerLeftWallHit = false;
    public static bool rightPlayerRightWallHit = true;

    public Canvas roundCanvas;
    public TextMeshProUGUI roundText;

    public static bool ABTestActive = false;


    public void StartGame()
    {

        allowedToStart = true;

        enemyCar.SetActive(true);
        playerCar.SetActive(true);

        startButton.gameObject.SetActive(false);
    }

    private void setupEnemyCar()
    {
        enemyCar = GameObject.Find("CarBuild (1)");

        enemyCarSpawn = enemyCar.transform.position;
        enemyCarCopy = Instantiate(enemyCar, enemyCarSpawn, enemyCar.transform.rotation);

        enemyCarCopy.SetActive(false);

    }

    private void setupPlayerCar()
    {
        playerCar = GameObject.Find("CarBuild");

        playerCarSpawn = playerCar.transform.position;
        playerCarCopy = Instantiate(playerCar, playerCarSpawn, playerCar.transform.rotation);

        playerCarCopy.SetActive(false);

    }

    private void Start()
    {
        dataSent = false;
        //if (Random.Range(0, 2) == 1)
        //{
        //    ABTestActive = true;
        //    ABTestButtonLock.gameObject.SetActive(true);
        //    ABTestButtonLock2.gameObject.SetActive(true);

        //    Debug.Log("AB Test is active");
        //}
        //else
        //{
        //    ABTestActive = false;
        //    ABTestButtonLock.gameObject.SetActive(false);
        //    ABTestButtonLock2.gameObject.SetActive(false);

        //    Debug.Log("AB Test is not active");
        //}

        enemyCar = GameObject.Find("CarBuild (1)");
        if(enemyCar.activeInHierarchy == false)
        {
            Debug.Log("wrong Enemy car was found");
            enemyCar = GameObject.FindGameObjectWithTag("Right Car");
        }

        if (enemyCar == null)
        {
            Debug.LogError("ENEMY CAR NOT FOUND");
        }

        playerCar = GameObject.Find("CarBuild");

        playerCarSpawn = enemyCar.transform.position;
        playerCarCopy = Instantiate(enemyCar, playerCarSpawn, enemyCar.transform.rotation);
        //playerCarCopy = carObject.gameObject;
        playerCarCopy.SetActive(false);
        winfx.SetActive(false);

        playerPartCount = playerCar.transform.childCount;
        enemyPartCount = enemyCar.transform.childCount;

        currentPlayerPartCount = playerPartCount;
        currentEnemyPartCount = enemyPartCount;

        //healthManager = enemyCar.GetComponentInChildren<HealthManager>();

        foreach (Transform child in playerCar.transform)
        {
            if (child.tag == "Booster")
            {
                hasBooster = true;  
            }
            if (child.tag == "Sproing")
            {
                hasSpring = true;
            }
        }

        enemyCar.SetActive(false);
        playerCar.SetActive(false);

        if (hasBooster)
        {
            boosterButton.gameObject.SetActive(true); // Activate the button
        }
        else
        {
            boosterButton.gameObject.SetActive(false); // Deactivate the button
        }

        if (hasSpring == true)
        {
            jumpButton.gameObject.SetActive(true);
        }
        else
        {
            jumpButton.gameObject.SetActive(false);
        }

        if (PlayerHealhBar != null)
        {
            int childCount = PlayerHealhBar.transform.childCount;
            playerHealthStates = new GameObject[childCount];
            for (int i = 0; i < childCount; i++)
            {
                playerHealthStates[i] = PlayerHealhBar.transform.GetChild(i).gameObject;
            }
        }
        else Debug.LogError("Player Healtbar not found");
        if (EnemyHealhBar != null)
        {
            int childCount = EnemyHealhBar.transform.childCount;
            enemyHealthStates = new GameObject[childCount];
            for (int i = 0; i < childCount; i++)
            {
                enemyHealthStates[i] = EnemyHealhBar.transform.GetChild(i).gameObject;
            }
            Debug.Log(enemyHealthStates.Length);
        }
        else Debug.LogError("Enemy Healtbar not found");
    }

    private void FixedUpdate()
    {
        if (allowedToStart == true)
        {
            timeAlive++;
        }
        //if(wallHit == true)
        //{
        //    foreach (Transform child in playerCar.transform)
        //    {
        //        Rigidbody rb = child.GetComponent<Rigidbody>();
        //        if (rb != null)
        //        {
        //            rb.AddForce(new Vector3(-1, 0, 0), ForceMode.VelocityChange);
        //        }
        //    }
        //}
        if (playerCar != null)
        {
            currentPlayerPartCount = playerCar.transform.childCount;
            float healthPercentage = (float)(playerPartCount - currentPlayerPartCount) / playerPartCount * 100;
            //Debug.Log("Player health " + healthPercentage);
            UpdateHealthVisual(healthPercentage,true);
        }
        if (enemyCar != null)
        {
            currentEnemyPartCount = enemyCar.transform.childCount;
            float healthPercentage = (float)(enemyPartCount - currentEnemyPartCount) / enemyPartCount * 100;
            //Debug.Log("health " + healthPercentage + "   current parts " + currentEnemyPartCount + "    total parts " + enemyPartCount);
            UpdateHealthVisual(healthPercentage,false);
        }
    }

    private void UpdateHealthVisual(float t_healthPercentage,bool t_player)
    {
        if(t_player == true)
        {
            //Debug.LogError("Player");
            if (playerHealthStates == null || playerHealthStates.Length == 0) return;

            int index = Mathf.FloorToInt(t_healthPercentage / 12.5f);
            index = Mathf.Clamp(index, 0, playerHealthStates.Length - 1);

            for (int i = 0; i < playerHealthStates.Length; i++)
            {
                playerHealthStates[i].SetActive(i == index);
            }
        }
        else
        {
            //Debug.LogError("Enemy");
            if (enemyHealthStates == null || enemyHealthStates.Length == 0) return;

            int index = Mathf.FloorToInt(t_healthPercentage / 12.5f);
            index = Mathf.Clamp(index, 0, enemyHealthStates.Length - 1);

            for (int i = 0; i < enemyHealthStates.Length; i++)
            {
                enemyHealthStates[i].SetActive(i == index);
            }
        }
    }

    private void Update()
    {
        if(allowedToStart == true)
        {
            if (enemyWins + playerWins == 3)
            {
                if (enemyWins > playerWins)
                {
                    win("Enemy");
                }
                else
                {
                    win("Player");
                }
            }

            if (enemyCarCopy == null)
            {
                setupEnemyCar();
            }
            if (playerCarCopy == null)
            {
                setupPlayerCar();
            }

            if (Input.GetKeyDown(KeyCode.Space))//debug
            {
                Destroy(enemyCar);
                //enemyCar = Instantiate(enemyCarCopy, enemyCarSpawn, this.transform.rotation);
                enemyCar.SetActive(true);

                Destroy(playerCar);
                playerCar = Instantiate(playerCarCopy, playerCarSpawn, this.transform.rotation);
                playerCar.SetActive(true);
            }
            check();
        }
    }

    public void JumpCar()
    {
        if(ABTestActive==false)
        {
            if (jumpUsed == false)
            {
                boing.Play();
                foreach (Transform child in playerCar.transform)
                {
                    Rigidbody rb = child.GetComponent<Rigidbody>();
                    if (rb != null)
                    {
                        rb.AddForce(new Vector3(0, 12, 0), ForceMode.Impulse);
                    }
                }
            }
            jumpUsed = true;
            hasSpring = false;
        }
    }


    public void ActivateBooster()
    {
        if(ABTestActive == false)
        {
            foreach (Transform booster in playerCar.GetComponentsInChildren<Transform>())
            {
                if (booster.name == "Booster Jet")
                {
                    ParticleSystem boosterParticles = booster.GetComponent<ParticleSystem>();
                    if (boosterParticles != null)
                    {
                        boosterParticles.Play();
                        StartCoroutine(StopParticles(boosterParticles, 2f)); // Stops after 2 seconds
                    }
                }
            }

            foreach (Transform child in playerCar.transform)
            {
                Rigidbody rb = child.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.AddForce(new Vector3(10, 0, 0), ForceMode.Impulse);
                }
            }

            hasBooster = false;
        }
    }
    private IEnumerator StopParticles(ParticleSystem particles, float delay)
    {
        yield return new WaitForSeconds(delay);
        particles.Stop();
    }

    private void ResetGame(string lastWinner)
    {
        // Take in last winner, display who won on card
        if (lastWinner == "player")
        {
            roundCanvas.gameObject.SetActive(true);
            roundText.SetText("Player won");
        }
        else if (lastWinner == "enemy")
        {
            roundCanvas.gameObject.SetActive(true);
            roundText.SetText("Enemy won");
        }

        Destroy(enemyCar);
        Destroy(playerCar);

        enemyCar = Instantiate(enemyCarCopy, enemyCarSpawn, enemyCarCopy.transform.rotation);
        enemyCar.SetActive(true);

        playerCar = Instantiate(playerCarCopy, playerCarSpawn, playerCarCopy.transform.rotation);
        playerCar.SetActive(true);


        foreach (Transform child in playerCar.transform)
        {
            if (child.tag == "Cockpit")
            {
                playerCarCockpitHealth = child.gameObject.GetComponent<CollisionHandler>();
            }
        }
        if(playerCarCockpitHealth == null)
        {
            //Debug.Log("Player cockpit health is null");

            foreach (Transform child in playerCarCopy.transform)
            {
                if (child.tag == "Cockpit")
                {
                    playerCarCockpitHealth = child.gameObject.GetComponent<CollisionHandler>();
                }
            }
        }
        if (playerCarCockpitHealth == null)
        {
            //Debug.Log("STILL FUCKING NULL RRRAAAAHHHHHH");
        }

        foreach (Transform child in enemyCar.transform)
        {
            if (child.tag == "Cockpit")
            {
                enemyCarCockpitHealth = child.gameObject.GetComponent<CollisionHandler>();
            }
        }

        jumpUsed = false;
        StartCoroutine(hideCanvas());
    }

    // Make the canvas displaying last winner go after 2 seconds
    IEnumerator hideCanvas()
    {
        yield return new WaitForSeconds(2f);

        roundCanvas.gameObject.SetActive(false);
    }

    public void Exit()
    {
        Application.Quit();
    }

    private void check()
    {
        if (enemyCarCockpitHealth == null)
        {
            playerWins++;
            //Debug.Log("player wins " + playerWins + "  Enemy wins " + enemyWins);
            ResetGame("player");
        }
        if (playerCarCockpitHealth == null)
        {
            enemyWins++;
            //Debug.Log("player wins " + playerWins + "  Enemy wins " + enemyWins);
            ResetGame("enemy");
        }

        if (hasBooster)
        {
            boosterButton.gameObject.SetActive(true); // Activate the button
        }
        else
        {
            boosterButton.gameObject.SetActive(false); // Deactivate the button
        }

        if(hasSpring == true)
        {
            jumpButton.gameObject.SetActive(true);
        }
        else
        {
            jumpButton.gameObject.SetActive(false);
        }
    }
    // Takes in the winner (gotten in healht manager) and does shtuff
    private void win(string winner)
    {
        roundCanvas.gameObject.SetActive(false);
        winfx.SetActive(true);

        playerCar.SetActive(false);
        enemyCar.SetActive(false);

        winText.text = "You win " + winner;

        //Debug.Log(winner + " wins!");

        //Debug.Log("sending player data");
        //string jsonData = JsonUtility.ToJson(sendGameAnalytics());
        //StartCoroutine(GameAnalytics.PostMethod(jsonData));

        //if (dataSent == false)
        //{
        //    dataSent = true;
        //    Debug.Log("sending player data");
        //    string jsonData = JsonUtility.ToJson(sendGameAnalytics());
        //    StartCoroutine(GameAnalytics.PostMethod(jsonData));
        //}
    }

    public void loadBuildScene()
    {
        //Debug.Log("Tried to load build");
        SceneManager.LoadScene("BuildingPrototype");
    }

    public GameState sendGameAnalytics()
    {
        //should run more often than just end of game
        //

        int ABTestState = 0;
        if (ABTestActive == true)
        {
            ABTestState = 1;
        }

        GameState data = new GameState
        {
            time_alive = timeAlive / 60,
            player = "Jonsey",
            player_wins = playerWins,
            enemy_wins = enemyWins,
            number_of_player_parts = playerPartCount,
            number_of_enemy_parts = enemyPartCount,
            player_parts_lost = 0,
            enemy_parts_lost = 0,
            current_round = playerWins + enemyWins,
            device_id = SystemInfo.deviceUniqueIdentifier,
            key = "MonterEnergy",
            AB_test = ABTestState
        };


        /*
            time_alive = timeAlive/60,
            player = "Jonsey",
            player_wins = playerWins,
            enemy_wins= enemyWins,
            number_of_player_parts = playerPartCount,
            number_of_enemy_parts = 5,
            parts_lost = 0,
            current_round = playerWins+enemyWins,
            device_id = SystemInfo.deviceUniqueIdentifier,
            key = "MonterEnergy"
         */

        //timeAlive = 0;

        return data;
    }
}
