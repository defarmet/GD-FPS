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


    public GameObject currentGunHUD;
    public GameObject[] gunHUD;

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
        if (Input.GetButtonDown("Cancel") && playerScript.hp > 0)
        {
            isPaused = !isPaused;
            currentMenuOpen = pauseMenu;
            currentMenuOpen.SetActive(isPaused);

            if (isPaused)
                cursorLockPause();
            else
                cursorUnlockUnpause();

        }

        if(enemyCount > 0)
        {

        }
    }

    public void cursorLockPause()
    {
        if (currentGunHUD != null)
            currentGunHUD.SetActive(false);

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
        Time.timeScale = 0;
    }

    public void cursorUnlockUnpause()
    {
        if (currentGunHUD != null)
            currentGunHUD.SetActive(true);

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1;
        currentMenuOpen.SetActive(isPaused);
        currentMenuOpen = null;
    }

    //public IEnumerator checkEnemyTotal()
    //{
    //    enemyCount--;

    //    if (enemyCount <= 0)
    //    {
    //        yield return new WaitForSeconds(2);
    //        winMenu.SetActive(true);
    //        currentMenuOpen = winMenu;
    //        cursorLockPause();
    //    }
    //}

    //public void pause_game(bool p)
    //{
    //	Cursor.visible = p;
    //	isPaused = p;
    //	if (p) {
    //		Cursor.lockState = CursorLockMode.Confined;
    //		Time.timeScale = 0;
    //	} else {
    //		Cursor.lockState = CursorLockMode.Locked;
    //		Time.timeScale = 1;
    //	}
    //	currentMenuOpen.SetActive(p);
    //}
}
