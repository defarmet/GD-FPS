using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameManager : MonoBehaviour
{

	public static gameManager instance;

	public GameObject player;
	public playerController playerScript;
	
	public GameObject pauseMenu;
	public GameObject currentMenuOpen;
	public GameObject playerDamageFlash;
	public GameObject playerDeadMenu;
	public GameObject currentGunHUD;

	public GameObject playerSpawnPoint;

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
            //pause_game(!isPaused);
		}
	}

	public void cursorLockPause()
	{
		Cursor.visible = true;
		Cursor.lockState = CursorLockMode.Confined;
		Time.timeScale = 0;
	}

	public void cursorUnlockUnpause()
	{
		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;
		Time.timeScale = 1;
		currentMenuOpen.SetActive(isPaused);
		currentMenuOpen = null;
	}

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
