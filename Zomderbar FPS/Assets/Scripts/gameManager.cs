using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class gameManager : MonoBehaviour
{

    public static gameManager instance;

    public GameObject player;
    public playerController playerScript;
    public EnemySpawners spawnerScript;

    public GameObject pauseMenu;
    public GameObject currentMenuOpen;
    public GameObject playerDamageFlash;
    public GameObject playerDeadMenu;
    public GameObject winMenu;

    public GameObject   currentGunHUD;
    public GameObject[] gunHUD;
	
    public GameObject checkpointHUD;

    public Image playerHpBar;
    public Image ammoBar;

    public GameObject playerSpawnPoint;

    public int enemyCount;
    public int enemyKilled;

    public bool isPaused = false;

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
            currentMenuOpen.SetActive(true);
            pause_game(!isPaused);
        }
    }

    public void cursorLockPause()
    {
        pause_game(true);
    }

    public void cursorUnlockUnpause()
    {
        pause_game(false);
    }

    public void pause_game(bool p)
    {
        if (currentGunHUD != null)
            currentGunHUD.SetActive(!p);

        Cursor.visible = p;
        isPaused = p;
        if (p) {
            Cursor.lockState = CursorLockMode.Confined;
            Time.timeScale = 0;
        } else {
            Cursor.lockState = CursorLockMode.Locked;
            Time.timeScale = 1;
            currentMenuOpen.SetActive(false);
            currentMenuOpen = null;
        }
    }
}
