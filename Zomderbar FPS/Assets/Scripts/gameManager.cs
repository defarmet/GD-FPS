using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class gameManager : MonoBehaviour
{
    [Header("---------- Instance -----------")]
    public static gameManager instance;

    [Header("---------- Player Components -----------")]
    public GameObject       player;
    public playerController playerScript;
    public GameObject       playerSpawnPoint;
    
    [Header("---------- Enemy Components -----------")]
    public EnemySpawners spawnerScript;
    public int           enemyKilled;

    [Header("---------- Menus -----------")]
    public GameObject pauseMenu;
    public GameObject currentMenuOpen;
    public GameObject playerDamageFlash;
    public GameObject playerDeadMenu;
    public GameObject winMenu;
    public GameObject d_arrow;
    public Vector3 d_angle;

    [Header("---------- HUD -----------")]
    public GameObject   currentGunHUD;
    public GameObject[] gunHUD;
    public Image        playerHpBar;

    [Header("---------- Booleans -----------")]
    public bool isPaused = false;

    /*
     * Some variables are set at start for easy development in the Unity editor.
     */
    void Awake()
    {
        instance = this;
        player = GameObject.FindGameObjectWithTag("Player");
        playerScript = player.GetComponent<playerController>();
        playerSpawnPoint = GameObject.FindGameObjectWithTag("Player Spawn Point");
        playerScript.respawn();

    }

    void Update()
    {
        if (Input.GetButtonDown("Cancel") && (!currentMenuOpen || currentMenuOpen == pauseMenu)) {
                currentMenuOpen = pauseMenu;
                currentMenuOpen.SetActive(!isPaused);
                pause_game(!isPaused);
        }
    }

    /*
     * Functions kept for compatibility with existing code
     */
    public void cursorLockPause()
    {
        pause_game(true);
    }

    public void cursorUnlockUnpause()
    {
        pause_game(false);
    }

    /*
     * All pausing and unpausing code goes here.
     */
    public void pause_game(bool p)
    {
        if (currentGunHUD)
            currentGunHUD.SetActive(!p);

        Cursor.visible = p;
        isPaused = p;
        if (p) {
            Cursor.lockState = CursorLockMode.Confined;
            Time.timeScale = 0;
        } else {
            currentMenuOpen.SetActive(false);
            currentMenuOpen = null;
            Cursor.lockState = CursorLockMode.Locked;
            Time.timeScale = 1;
        }
    }
}
