using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class gameManager : MonoBehaviour
{

    public static gameManager instance;

    public GameObject       player;
    public playerController playerScript;
    public EnemySpawners    spawnerScript;

    public GameObject currentMenuOpen;
    public GameObject oldMenu;
    public GameObject pauseMenu;
    public GameObject playerDamageFlash;
    public GameObject playerDeadMenu;
    public GameObject settingsMenu;
    public GameObject winMenu;
    public GameObject gameCompleteMenu;

    public GameObject   currentGunHUD;
    public GameObject[] gunHUD;

    public GameObject checkpointHUD;

    public Image playerHpBar;
    public Image ammoBar;

    public GameObject playerSpawnPoint;

    public int enemyCount;
    public int enemyKilled;

    public AudioMixer masterAudio;

    public bool isPaused = false;
           int  firstCount = 3;
           bool openSettings = false;

    [Header("-----Menu Navigation-----")]
    public GameObject pauseFirstButton;
    public GameObject settingsFirstButton;
    public GameObject settingsClosedButton;
    public GameObject deadFirstButton;
    public GameObject winFirstButton;
    public GameObject completeFirstButton;

    static public List<gunStats> statsStart = new List<gunStats>();
    
    void Awake()
    {
        instance = this;
        player = GameObject.FindGameObjectWithTag("Player");

        if (player) {
            playerScript = player.GetComponent<playerController>();
            for (int i = 0; i < statsStart.Count; i++)
                playerScript.gunPickup(statsStart[i], statsStart[i].gunHUD);

            playerSpawnPoint = GameObject.FindGameObjectWithTag("Player Spawn Point");
            playerScript.respawn();
        }
    }

    void Start()
    {
        masterAudio.SetFloat("Music Slider", Mathf.Log10(PlayerPrefs.GetFloat("musicVol", 0.5f)) * 20);
        masterAudio.SetFloat("SFX Slider", Mathf.Log10(PlayerPrefs.GetFloat("sfxVol", 0.5f)) * 20);
    }

    void Update()
    {
        if (Input.GetButtonDown("Cancel") && (!currentMenuOpen || currentMenuOpen == pauseMenu) && SceneManager.GetActiveScene().buildIndex > 0) {
            currentMenuOpen = pauseMenu;
            currentMenuOpen.SetActive(true);
            pause_game(!isPaused);
        }

        /*
         * MAGIC
         * DO NOT TOUCH
         * Required for the settings menu to open the first time.
         */
        if (firstCount > 0 && openSettings) {
            open_settings();
            firstCount--;
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

            /*
             * Menu Navigation
             * Clear selected object first.
             */
            EventSystem.current.SetSelectedGameObject(null);

            /*
             * set new selected object
             */
            EventSystem.current.SetSelectedGameObject(pauseFirstButton);
        } else {
            Cursor.lockState = CursorLockMode.Locked;
            Time.timeScale = 1;
            currentMenuOpen.SetActive(false);
            currentMenuOpen = null;
        }
    }

    public void open_settings()
    {
        currentMenuOpen.SetActive(false);
        if (!oldMenu)
            oldMenu = currentMenuOpen;
        
        currentMenuOpen = settingsMenu;
        currentMenuOpen.SetActive(true);
        openSettings = true;

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(settingsFirstButton);
    }

    public void close_settings()
    {
        masterAudio.GetFloat("Music Slider", out float vol);
        PlayerPrefs.SetFloat("musicVol", Mathf.Pow(10, vol / 20));
        masterAudio.GetFloat("SFX Slider", out vol);
        PlayerPrefs.SetFloat("sfxVol", Mathf.Pow(10, vol / 20));
        PlayerPrefs.Save();

        currentMenuOpen.SetActive(false);
        currentMenuOpen = oldMenu;
        oldMenu = null;
        currentMenuOpen.SetActive(true);
        openSettings = false;

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(settingsClosedButton);
    }
}
